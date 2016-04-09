//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------


using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Security.Principal;
using System.ServiceModel;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace PassiveFlowIPSTS1
{
    /// <summary>
    /// Extends the Microsoft.IdentityModel.Services.SecurityTokenService class to provide
    /// the relying party related information, such as encryption credentials to encrypt the issued
    /// token, signing credentials to sign the issued token, claims that STS wants to issue for a 
    /// certain token request, as well as the claim types that this STS is capable
    /// of issuing.
    /// </summary>
    public class CustomSecurityTokenService : SecurityTokenService
    {
        // Certificate Constants
        private const string SIGNING_CERTIFICATE_NAME = "CN=localhost";
        private const string ENCRYPTING_CERTIFICATE_NAME = "CN=localhost";

        // Custom Claims that this IP STS is capable of issuing
        private const string IPSTS1_ID_CLAIM = "http://WindowsIdentityFoundationSamples/IPSTS1/2008/myID";
        private const string IPSTS1_AGE_CLAIM = "http://WindowsIdentityFoundationSamples/IPSTS1/2008/myAgeClaim";
        private const string IPSTS1_IDENTIFIER_CLAIM = "http://WindowsIdentityFoundationSamples/IPSTS1/2008/IdentityProviderIdentifier";

        private SigningCredentials _signingCreds;
        private EncryptingCredentials _encryptingCreds;
        private string _addressExpected = "https://localhost/PassiveFPSTS/Default.aspx";

        /// <summary>
        /// Construct a new token service using the specified configuration.
        /// </summary>
        /// <param name="configuration"></param>
        public CustomSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
            // Setup the certificate our STS is going to use to sign the issued tokens
            _signingCreds = new X509SigningCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine, SIGNING_CERTIFICATE_NAME));

            // Note: In this sample app only a single RP identity is shown, which is localhost, and the certificate of that RP is 
            // populated as _encryptingCreds
            // If you have multiple RPs for the STS you would select the certificate that is specific to 
            // the RP that requests the token and then use that for _encryptingCreds
            _encryptingCreds = new X509EncryptingCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine, ENCRYPTING_CERTIFICATE_NAME));
        }


        /// <summary>
        /// This method returns the configuration for the token issuance request. The configuration
        /// is represented by the Scope class. In our case, we are only capable of issuing a token to a
        /// single RP identity represented by the _encryptingCreds field.
        /// </summary>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST</param>
        /// <returns></returns>
        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            // Validate the AppliesTo address
            ValidateAppliesTo(request.AppliesTo);

            // Create the scope using the request AppliesTo address and the RP identity
            Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri, _signingCreds );

            if (Uri.IsWellFormedUriString(request.ReplyTo, UriKind.Absolute))
            {
                if (request.AppliesTo.Uri.Host != new Uri(request.ReplyTo).Host)
                    scope.ReplyToAddress = request.AppliesTo.Uri.AbsoluteUri;
                else
                    scope.ReplyToAddress = request.ReplyTo;
            }
            else
            {
                Uri resultUri = null;
                if (Uri.TryCreate(request.AppliesTo.Uri, request.ReplyTo, out resultUri))
                    scope.ReplyToAddress = resultUri.AbsoluteUri;
                else
                    scope.ReplyToAddress = request.AppliesTo.Uri.ToString() ;
            }

            // Note: In this sample app only a single RP identity is shown, which is localhost, and the certificate of that RP is 
            // populated as _encryptingCreds
            // If you have multiple RPs for the STS you would select the certificate that is specific to 
            // the RP that requests the token and then use that for _encryptingCreds            
            scope.EncryptingCredentials = _encryptingCreds;

            return scope;
        }

        /// <summary>
        /// This method computes the NT Account name translated from the primary sid claim found
        /// in the provided claims principal.
        /// </summary>
        /// <param name="principal">The claims principal containing the primary sid claim.</param>
        /// <returns>The NT Account name corresponding to the primary sid claim found in the claims principal.</returns>
        private string GetNTAccountName( IClaimsPrincipal principal )
        {
            IClaimsIdentity claimsIdentity = (IClaimsIdentity) principal.Identities[0];

            // The STS has Windows Authentication enabled. With this mode of authentication
            // the user will have a PrimarySid claim.
            List<Claim> primarySidsFound = ( from c in claimsIdentity.Claims
                                             where c.ClaimType == ClaimTypes.PrimarySid
                                             select c ).ToList();

            if (primarySidsFound.Count == 1)
            {
                Claim primarySidClaim = primarySidsFound[0];
                SecurityIdentifier sid = new SecurityIdentifier(primarySidClaim.Value);
                return ((NTAccount)sid.Translate(typeof(NTAccount))).Value;
            }
            else
            {
                throw new UnauthorizedAccessException(
                    String.Format("Found {0} PrimarySid claims.", primarySidsFound.Count));
            }
        }

        /// <summary>
        /// This method returns the content of the issued token. The content is represented as a set of
        /// IClaimIdentity intances, each instance corresponds to a single issued token. Currently, the Windows Identity Foundation only
        /// supports a single token issuance, so the returned collection must always contain only a single instance.
        /// </summary>
        /// <param name="scope">The scope that was previously returned by GetScope method</param>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST, we don't use this in our implementation</param>
        /// <returns></returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            ClaimsIdentity outputIdentity = new ClaimsIdentity();

            // Add the default claims that this STS would issue

            // Name from SID
            string ntAccountValue = GetNTAccountName(principal);
            ntAccountValue = ntAccountValue.Substring(ntAccountValue.IndexOf('\\') + 1);
            outputIdentity.Claims.Add(new Claim(ClaimTypes.Name, ntAccountValue));

            // IP id claim type
            outputIdentity.Claims.Add(new Claim(IPSTS1_IDENTIFIER_CLAIM, "IPSTS1", ClaimValueTypes.String));

            // myID claim
            outputIdentity.Claims.Add(new Claim(IPSTS1_ID_CLAIM, ntAccountValue, ClaimValueTypes.String));
            
            // Age claim
            outputIdentity.Claims.Add(new Claim(IPSTS1_AGE_CLAIM, "25", ClaimValueTypes.Integer));

            return outputIdentity;
        }

        /// <summary>
        /// Validates the appliesTo and throws an exception if the appliesTo is null or appliesTo contains some unexpected address.
        /// </summary>
        /// <param name="appliesTo">The AppliesTo parameter in the request that came in (RST)</param>
        /// <returns></returns>
        void ValidateAppliesTo(EndpointAddress appliesTo)
        {
            if (appliesTo == null)
            {
                throw new InvalidRequestException("The appliesTo is null.");
            }

            if (!appliesTo.Uri.Equals(new Uri(_addressExpected)))
            {
                throw new InvalidRequestException(String.Format("The relying party address is not valid. Expected value is {0}, the actual value is {1}.", _addressExpected, appliesTo.Uri.AbsoluteUri));
            }
        }
    }

}
