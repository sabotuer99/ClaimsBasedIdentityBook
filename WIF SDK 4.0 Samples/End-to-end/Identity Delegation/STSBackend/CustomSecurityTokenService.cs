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


using System.IdentityModel.Selectors;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml11;

namespace STS
{
    /// <summary>
    /// Implementation of a Custom SecurityTokenService.
    /// </summary>
    public class CustomSecurityTokenService : SecurityTokenService
    {
        static readonly string SigningCertificateName = "CN=STS";
        static readonly string EncryptingCertificateName = "CN=localhost";

        /// <summary>
        /// Creates an instance of CustomSecurityTokenService. 
        /// </summary>
        /// <param name="configuration">Configuration for this SecurityTokenService.</param>
        public CustomSecurityTokenService( SecurityTokenServiceConfiguration configuration )
            : base( configuration )
        {
            // Setup the certificate our STS is going to use to sign the issued tokens
            configuration.SigningCredentials = new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, SigningCertificateName ) );
        }

        /// <summary>
        /// This method returns the configuration for the token issuance request. The configuration
        /// is represented by the Scope class. In our case, we are only capable of issuing a token to a
        /// single RP identity represented by CN=localhost.
        /// </summary>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST</param>
        /// <returns></returns>
        protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
        {
            // We only support a single RP identity represented by CN=localhost. Set the RP certificate for encryption
            X509EncryptingCredentials encryptingCredentials = new X509EncryptingCredentials(
                                              CertificateUtil.GetCertificate( StoreName.My,
                                                                              StoreLocation.LocalMachine,
                                                                              EncryptingCertificateName ) );

            
            // Create the scope using the request AppliesTo address, the STS signing certificate and the encryptingCredentials for the RP.
            if ( request.AppliesTo == null || request.AppliesTo.Uri == null )
            {
                throw new InvalidRequestException( "Cannot determine the AppliesTo address from the RequestSecurityToken." );
            }
            Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri, SecurityTokenServiceConfiguration.SigningCredentials, encryptingCredentials );
            
            // Set the replyTo address. In WS-Federation passive case this value is used as the endpoint
            // where the user is redirected to.
            scope.ReplyToAddress = scope.AppliesToAddress;

            return scope;
        }

        /// <summary>
        /// This method returns the content of the issued token. The content is represented as a set of
        /// IClaimIdentity intances, each instance corresponds to a single issued token. 
        /// </summary>
        /// <param name="scope">The scope that was previously returned by GetScope method.</param>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST.</param>
        /// <returns></returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            // Create new identity and copy content of the caller's identity into it (including the existing delegate chain)
            IClaimsIdentity callerIdentity = (IClaimsIdentity)principal.Identity;
            IClaimsIdentity outputIdentity = callerIdentity.Copy();

            // There may be many GroupSid claims which we ignore to reduce the token size
            // Just select the PrimarySid and Name claims for the purpose of this sample
            Claim[] claims = (from c in outputIdentity.Claims
                                       where c.ClaimType == ClaimTypes.PrimarySid || c.ClaimType == ClaimTypes.Name
                                       select c ).ToArray<Claim>();

            outputIdentity.Claims.Clear();
            outputIdentity.Claims.AddRange( claims );

            // If there is an ActAs token in the RST, return a copy of it as the top-most identity
            // and put the caller's identity into the Actor property of this identity.
            if ( request.ActAs != null )
            {
                IClaimsIdentity actAsSubject = request.ActAs.GetSubject()[0];
                IClaimsIdentity actAsIdentity = actAsSubject.Copy();

                // Find the last actor in the actAs identity
                IClaimsIdentity lastActor = actAsIdentity;
                while ( lastActor.Actor != null )
                {
                    lastActor = lastActor.Actor;
                }

                // Set the caller's identity as the last actor in the delegation chain
                lastActor.Actor = outputIdentity;

                // Return the actAsIdentity instead of the caller's identity in this case
                outputIdentity = actAsIdentity;
            }

            return outputIdentity;
        }

    }
}
