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
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Samples.TrustChannel
{
    class CustomSecurityTokenService : Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService
    {
        public const string ExpectedAddress = "http://localhost:8080/CalcService";
        
        public static X509Certificate2 RPCert = CertificateUtil.GetCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="config">The <see cref="SecurityTokenServiceConfiguration"/> object to be passed to the base class.</param>
        public CustomSecurityTokenService( SecurityTokenServiceConfiguration config )
            : base( config )
        {
        }

        /// <summary>
        /// Override this method to provide a scope object that includes information that is
        /// specific to the relying party corresponding to the request.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="request">The request.</param>
        /// <returns>A Scope object that includes the relying party specific data.</returns>
        protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
        {
            ValidateAppliesTo( request.AppliesTo );

            Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri );
            scope.SigningCredentials = this.SecurityTokenServiceConfiguration.SigningCredentials;
            //
            // In this sample app only a single RP identity is shown, which is localhost, and the certificate of that RP is 
            // populated as the EncryptingCredentials.
            //
            // If you have multiple RPs for the STS you would select the certificate that is specific to 
            // the RP that requests the token and then use that for EncryptingCredentials.
            //
            scope.EncryptingCredentials = new X509EncryptingCredentials( RPCert );

            return scope;
        }

        /// <summary>
        /// This overriden method returns a claims identity to be included in the issued token.
        /// </summary>
        /// <param name="principal">The principal that represents the identity of the requestor.</param>
        /// <param name="request">The token request parameters that arrived in the call.</param>
        /// <param name="scope">The scope information about the Relying Party.</param>
        /// <returns>The claims identity that will be placed inside the issued token.</returns>        
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            var issuedClaims = new List<Claim>() {
                new Claim( ClaimTypes.Name, "Bob" ),
            };

            return new ClaimsIdentity( issuedClaims );
        }

        /// <summary>
        /// This application is hard wired to work with only one particular RP.
        /// </summary>
        void ValidateAppliesTo( EndpointAddress appliesTo )
        {
            if ( appliesTo == null )
            {
                throw new InvalidRequestException( "The appliesTo is null." );
            }

            if ( !appliesTo.Uri.Equals( new Uri( ExpectedAddress ) ) )
            {
                throw new InvalidRequestException( String.Format( "The relying party address is not valid. Expected value is {0}, the actual value is {1}.", ExpectedAddress, appliesTo.Uri.AbsoluteUri ) );
            }
        }

    }
}
