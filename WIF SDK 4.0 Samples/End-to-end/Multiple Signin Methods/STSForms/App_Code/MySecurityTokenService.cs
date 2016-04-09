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
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace PassiveFlowSTS
{
    public class MySecurityTokenService : SecurityTokenService
    {
        const string SigningCertificateName = "CN=localhost";
        const string EncryptingCertificateName = "CN=localhost";
        const string AddressExpected = "https://localhost/MultiAuthRP";

        SigningCredentials _signingCreds;
        EncryptingCredentials _encryptingCreds;

        public MySecurityTokenService( SecurityTokenServiceConfiguration configuration )
            : base( configuration )
        {
            // Setup the certificate our STS is going to use to sign the issued tokens
            _signingCreds = new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, SigningCertificateName ) );

            // We only support a single RP identity represented by the _encryptingCreds
            _encryptingCreds = new X509EncryptingCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, EncryptingCertificateName ) );
        }


        /// <summary>
        /// This method returns the configuration for the token issuance request. The configuration
        /// is represented by the Scope class. In our case, we are only capable of issuing a token to a
        /// single RP identity represented by the _encryptingCreds field.
        /// </summary>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST</param>
        /// <returns>The scope.</returns>
        protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
        {
            ValidateAppliesTo( request.AppliesTo );

            // Create the scope using the request AppliesTo address and the RP identity
            Scope scope = new Scope( request.AppliesTo.Uri.ToString(), _signingCreds );
            scope.ReplyToAddress = scope.AppliesToAddress + "/Login.aspx";

            scope.EncryptingCredentials = _encryptingCreds;
            return scope;
        }


        /// <summary>
        /// This method returns the claims to be included in the issued token encapsulated by an IClaimsIdentity.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST, we don't use this in our implementation.</param>
        /// <param name="scope">The scope that was previously returned by GetScope method.</param>
        /// <returns>The claims to be included in the issued token encapsulated by an IClaimsIdentity.</returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            Claim claim;
            IClaimsIdentity callerIdentity = ( IClaimsIdentity )principal.Identity;
            ClaimsIdentity outputIdentity = new ClaimsIdentity();
            IEnumerable<Claim> claimCollection = ( from c in callerIdentity.Claims
                                                   where
                                                   c.ClaimType == ClaimTypes.Name
                                                   select c );

            if ( claimCollection.Count<Claim>() > 0 )
            {
                claim = new Claim( ClaimTypes.Name, claimCollection.First<Claim>().Value );
            }
            else
            {
                claim = new Claim( ClaimTypes.Name, "Anonymous" );
            }

            outputIdentity.Claims.Add( claim );
            outputIdentity.Claims.Add( new Claim( "http://PassiveFlowNamespace/2008/05/AgeClaim", "25", ClaimValueTypes.Integer ) );

            return outputIdentity;
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

            if ( !appliesTo.Uri.Equals( new Uri( AddressExpected ) ) )
            {
                throw new InvalidRequestException( String.Format( "The relying party address is not valid. Expected value is {0}, the actual value is {1}.", AddressExpected, appliesTo.Uri.AbsoluteUri ) );
            }
        }

    }
}
