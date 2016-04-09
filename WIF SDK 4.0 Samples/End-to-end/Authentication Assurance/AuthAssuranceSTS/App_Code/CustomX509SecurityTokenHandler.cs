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
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AuthAssuranceSTS
{
    /// <summary>
    /// Custom X509 token handler for certificate-based authentication.
    /// </summary>
    public class CustomX509SecurityTokenHandler : X509SecurityTokenHandler
    {
        const string _acceptedClientCertificateSubjectName = "CN=bobclient";

        public CustomX509SecurityTokenHandler()
            : base()
        {
        }

        /// <summary>
        /// Overrides the abstract base class function to validate the X509 token.
        /// </summary>
        /// <param name="token">The X509 token.</param>
        /// <exception cref="ArgumentNullException">If the security token is null.</exception>
        /// <returns>ClaimsIdentityCollection for the user.</returns>
        public override ClaimsIdentityCollection ValidateToken( SecurityToken token )
        {
            if ( token == null )
            {
                throw new ArgumentNullException( "token" );
            }

            X509SecurityToken x509Token = token as X509SecurityToken;
            if ( x509Token == null )
            {
                throw new ArgumentException( "token", "The security token is not a valid X509 security token." );
            }

            //
            // NOTE: For illustrative purposes, this validator accepts any certificate
            // with subject name "CN=bobclient" that is not expired. Do not use this
            // in production environment.
            //
            X509Certificate2 certificate = x509Token.Certificate;

            if ( certificate.Subject != _acceptedClientCertificateSubjectName )
            {
                throw new InvalidOperationException( String.Format( "The certificate subject name '{0}' does not match '{1}'", certificate.Subject, _acceptedClientCertificateSubjectName ) );
            }

            DateTime now = DateTime.Now;
            if ( certificate.NotBefore > now || certificate.NotAfter < now )
            {
                throw new InvalidOperationException( "The certificate is expired or not yet valid." );
            }

            return new ClaimsIdentityCollection(
                            new ClaimsIdentity[]{
                            new ClaimsIdentity(
                                new Claim[]{    
                                    new Claim( ClaimTypes.Name, certificate.Subject ),
                                    new Claim( ClaimTypes.AuthenticationMethod, AuthenticationMethods.X509 ),
                                    } ) } );
        }
    }
}
