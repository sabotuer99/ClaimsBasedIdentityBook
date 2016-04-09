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
using Microsoft.IdentityModel.Samples.WindowsTokenService.Common;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.SecurityTokenService
{
    /// <summary>
    /// The security token service implementation that issues a UPN claim.
    /// </summary>
    class CustomSecurityTokenService : Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="config">The <see cref="SecurityTokenServiceConfiguration"/> object to be passed to the base class.</param>
        public CustomSecurityTokenService( SecurityTokenServiceConfiguration config )
            : base( config )
        {
        }

        /// <summary>
        /// Override this method to provide scope specific information.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="request">The request.</param>
        /// <returns>The scope.</returns>
        protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
        {
            ValidateAppliesTo( request.AppliesTo );
            Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri );
            scope.SigningCredentials = new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" ) );
            scope.EncryptingCredentials = new X509EncryptingCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" ) );

            return scope;
        }

        /// <summary>
        /// This overriden method returns a collection of output subjects to be included in the issued token.
        /// </summary>
        /// <param name="scope">The scope information about the Relying Party.</param>
        /// <param name="principal">The principal that represents the identity of the requestor.</param>
        /// <param name="request">The token request parameters that arrived in the call.</param>
        /// <returns>The claimsets collection that will be placed inside the issued token.</returns>        
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            // NOTE: replace the claim value with a valid upn claim of your domain. Else, the sample will not work.
            return new ClaimsIdentity( new Claim[]{ new Claim( Microsoft.IdentityModel.Claims.ClaimTypes.Upn, "name@yourdomain.com" ) } );
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

            if ( !appliesTo.Uri.Equals( new Uri( Address.ServiceAddress1 ) ) & !appliesTo.Uri.Equals( new Uri( Address.ServiceAddress2 ) ) )
            {
                throw new InvalidRequestException( String.Format( "The relying party address is not valid. Expected value is either {0} or {1}, the actual value is {2}.", Address.ServiceAddress1, Address.ServiceAddress2, appliesTo.Uri.AbsoluteUri ) );
            }
        }
    }
}
