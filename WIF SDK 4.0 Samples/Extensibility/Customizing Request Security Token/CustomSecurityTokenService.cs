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

namespace Microsoft.IdentityModel.Samples.CustomRequestSecurityToken
{
    /// <summary>
    /// This class will validate the custom element inside the request 
    /// </summary>
    public class CustomSecurityTokenService : Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService
    {
        public const string _addressExpected = "http://localhost:8080/EchoService";

        /// <summary>
        /// Constructors a security token service object with the specified configuration.
        /// </summary>
        public CustomSecurityTokenService( SecurityTokenServiceConfiguration configuration )
            : base( configuration )
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
        /// This overriden method returns a collection of claims to be included in the issued token.
        /// </summary>
        /// <param name="principal">The IClaimsPrincipal that represents the identity of the requestor.</param>
        /// <param name="request">The token request that arrived in the call.</param>
        /// <param name="scope">The scope information about the relying party.</param>
        /// <returns>The claims collection that will be placed inside the issued token.</returns>        
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            return new ClaimsIdentity( new Claim[]{ new Claim( ClaimTypes.Name, "bob" ) } );
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

            if ( !appliesTo.Uri.Equals( new Uri( _addressExpected ) ) )
            {
                throw new InvalidRequestException( String.Format( "The relying party address is not valid. Expected value is {0}, the actual value is {1}.", _addressExpected, appliesTo.Uri.AbsoluteUri ) );
            }
        }
        /// <summary>
        /// Override the ValidateRequest method to validate the custom element
        /// </summary>
        /// <param name="request">The security token request.</param>
        protected override void ValidateRequest( RequestSecurityToken request )
        {
            base.ValidateRequest( request );

            //
            // here we validate the custom element
            //
            string customElementValue = null;
            CustomRequestSecurityToken customRST = request as CustomRequestSecurityToken;            
            if ( null != customRST )
            {
                customElementValue = customRST.CustomElement;    
            }

            if ( string.IsNullOrEmpty( customElementValue ) )
            {
                throw new InvalidRequestException( "The custom element is missing" );
            }

            if ( customElementValue != CustomElementConstants.DefaultElementValue )
            {
                throw new InvalidRequestException( string.Format( "The custom element's value is not expected. The value received is \'{0}\', but expected value is \'{1}\'.", customElementValue, CustomElementConstants.DefaultElementValue ) );
            }

            Console.WriteLine( "The STS received the custom element. \n" );
        }
    }
}
