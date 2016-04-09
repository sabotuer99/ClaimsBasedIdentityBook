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
using System.ServiceModel;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace ClaimsAwareWebService
{
    public class MySecurityTokenService : SecurityTokenService
    {
        const string addressExpected = "http://localhost:6020/ClaimsAwareWebService";
        const string encryptingCertificateName = "CN=localhost";

        /// <summary>
        /// Creates an instance of the MySecurityTokenService class.
        /// </summary>
        /// <param name="configuration">SecurityTokenServiceConfiguration element.</param>
 
        public MySecurityTokenService( SecurityTokenServiceConfiguration configuration )
            : base( configuration )
        {
        }

        /// <summary>
        /// This method returns the configuration for the token issuance request. The configuration
        /// is represented by the Scope class. In our case, we are only capable of issuing a token to a
        /// single RP identity represented by CN=localhost.
        /// </summary>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST</param>
        /// <returns>The configuration for the token issuance request.</returns>
        protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
        {
            // Validate the AppliesTo on the incoming request
            ValidateAppliesTo( request.AppliesTo );

            // Normally the STS will have a trust relationship with the RP and can look up a trusted encrypting certficate 
            // using the AppliesTo endpoint. This is necessary to ensure that only the RP will be able to read the claims.
            //
            // In this sample the certificate of the AppliesTo Identity is used to encrypt the contents, so there is no
            // validation of any trust relationship with the RP. Since the certificate is not validated, 
            // a malicious client can provide a known certificate allowing it to read the returned claims.
            // For this reason, THIS APPROACH SHOULD NOT BE USED if the claims should be kept private. It may be reasonable,
            // though, if the STS is simply verifying public information such as the client's email address.

            // Get RP certificate
            X509CertificateEndpointIdentity appliesToIdentity = (X509CertificateEndpointIdentity)request.AppliesTo.Identity;

            X509EncryptingCredentials encryptingCredentials = new X509EncryptingCredentials( appliesToIdentity.Certificates[0] );
            // Create the scope using the request AppliesTo address and the STS signing certificate
            Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri, SecurityTokenServiceConfiguration.SigningCredentials, encryptingCredentials );
            return scope;
        }

        /// <summary>
        /// Validates the appliesTo and throws an exception if the appliesTo is null or appliesTo contains some unexpected address.  
        /// </summary>
        void ValidateAppliesTo( EndpointAddress appliesTo )
        {
            if ( appliesTo == null )
            {
                throw new InvalidRequestException( "The appliesTo is null." );
            }

            if ( !appliesTo.Uri.Equals( new Uri( addressExpected ) ) )
            {
                Console.WriteLine( "The relying party address is not valid. " );
                throw new InvalidRequestException( String.Format( "The relying party address is not valid. Expected value is {0}, the actual value is {1}.", addressExpected, appliesTo.Uri.AbsoluteUri ) );
            }
        }

        /// <summary>
        /// This method returns the claims to be included in the issued token. 
        /// </summary>
        /// <param name="scope">The scope that was previously returned by GetScope method</param>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST</param>
        /// <returns>The claims to be included in the issued token.</returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            IClaimsIdentity callerIdentity = (IClaimsIdentity)principal.Identity;
            Console.WriteLine( "\nRequest from: " + callerIdentity.Name + "\n" );

            IClaimsIdentity outputIdentity = new ClaimsIdentity();

            // Create a name claim from the incoming identity.
            Claim nameClaim = new Claim( ClaimTypes.Name, callerIdentity.Name );

            // Create an 'Age' claim with a value of 25. In a real scenario, this may likely be looked up from a database.
            Claim ageClaim = new Claim( "http://WindowsIdentityFoundationSamples/2008/05/AgeClaim", "25", ClaimValueTypes.Integer );

            outputIdentity.Claims.Add( nameClaim );
            Console.WriteLine( "ClaimType  : " + nameClaim.ClaimType );
            Console.WriteLine( "ClaimValue : " + nameClaim.Value );
            Console.WriteLine();

            Console.WriteLine( "ClaimType  : " + ageClaim.ClaimType );
            Console.WriteLine( "ClaimValue : " + ageClaim.Value );
            Console.WriteLine( "===========================" );

            outputIdentity.Claims.Add( ageClaim );

            return outputIdentity;
        }
    }
}
