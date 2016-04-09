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
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Samples.CustomToken
{
    /// <summary>
    /// Overrides the Microsoft.IdentityModel.Services.SecurityTokenService.SecurityTokenService class to provide
    /// the relying party related information, such as encryption credentials to encrypt the issued
    /// token, signing credentials to sign the issued token, claims that the STS wants to issue for a 
    /// certain token request, as well as the claim types that this STS is capable
    /// of issuing.
    /// </summary>
    class SampleTokenService : Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService
    {
        string _addressExpected = "http://localhost:8080/EchoService";

        public SampleTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// Override this method to provide scope specific information.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="request">The security token request.</param>
        /// <returns>The scope object.</returns>
        protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
        {
            ValidateAppliesTo( request.AppliesTo );

            Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri );
            
            scope.SigningCredentials = new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" ) );
            
            // Note: In this sample app only a single RP identity is shown, which is localhost, and the certificate of that RP is 
            // populated as _encryptingCreds
            // If you have multiple RPs for the STS you would select the certificate that is specific to 
            // the RP that requests the token and then use that for _encryptingCreds
            scope.EncryptingCredentials = new X509EncryptingCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" ) );

            return scope;
        }

        /// <summary>
        /// This method returns the content of the issued token. The content is represented as a set of
        /// IClaimIdentity intances, each instance corresponds to a single issued token.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST.</param>
        /// <param name="scope">The scope that was previously returned by GetScope method.</param>
        /// <returns>A collection of claims that can be sent back inside a token.</returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            //
            // Return a default claim set which contains a custom decision claim
            // Here you can actually examine the user by looking at the IClaimsPrincipal and 
            // return the right decision based on that. 
            //
            return new ClaimsIdentity( new Claim[]{ new Claim( ClaimTypes.Name, "CustomToken" ) } );
        }

        /// <summary>
        /// Validates the appliesTo and throws an exception if the appliesTo is null or appliesTo contains some unexpected address.  
        /// </summary>
        /// <param name="appliesTo">The relying party address.</param>
        void ValidateAppliesTo( EndpointAddress appliesTo )
        {
            if ( appliesTo == null )
            {
                throw new InvalidRequestException( "The appliesTo is null." );
            }

            if ( !appliesTo.Uri.Equals( new Uri( _addressExpected ) ) )
            {
                throw new InvalidRequestException( String.Format( "The relying party address is not valid. Expected value is {0}, the actual value is {1}.", _addressExpected, appliesTo.Uri.AbsoluteUri ) );
            }
        }
    }
}
