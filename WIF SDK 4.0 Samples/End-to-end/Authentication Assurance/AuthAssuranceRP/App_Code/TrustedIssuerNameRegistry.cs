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
using System.Web;
using Microsoft.IdentityModel.Tokens;

public class TrustedIssuerNameRegistry : IssuerNameRegistry
{
    public override string GetIssuerName( SecurityToken securityToken )
    {
        X509SecurityToken x509Token = securityToken as X509SecurityToken;
        if ( x509Token != null )
        {
            // The following check is to allow only trusted issuers of the STS token
            if ( String.Equals( x509Token.Certificate.SubjectName.Name, "CN=localhost" ) )
            {
                return x509Token.Certificate.SubjectName.Name;
            }

            // The following check is to allow only trusted issuers for the client certificate. 
            if ( HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.ClientCertificate != null &&
                 HttpContext.Current.Request.ClientCertificate.IsPresent && HttpContext.Current.Request.ClientCertificate.IsValid )
            {
                X509Certificate2 clientCertificate = new X509Certificate2( HttpContext.Current.Request.ClientCertificate.Certificate );

                // Get the issuer of the client certificate
                X509Chain chain = new X509Chain();
                chain.ChainPolicy.RevocationMode = X509RevocationMode.NoCheck;
                chain.Build( clientCertificate );
                X509ChainElementCollection elements = chain.ChainElements;

                X509Certificate2 issuerCert = null;
                if ( elements.Count > 1 )
                {
                    issuerCert = elements[1].Certificate;
                }
                else
                {
                    // This is a self-issued certificate.
                    issuerCert = clientCertificate;
                }

                string issuerCertThumbprint = issuerCert.Thumbprint;

                // Reset the state of the certificate and free resources associated with it.
                for ( int i = 1; i < elements.Count; ++i )
                {
                    elements[i].Certificate.Reset();
                }

                // The implementation below currently accepts all client certificates and returns the SubjectName as the issuer name. This is intended only for illustration purposes. 
                // In production, consider doing additional validation based on the issuer of the client certificate to return an appropriate issuer name.
                // DO NOT use this sample code ‘as is’ in production code.

                // Ensure that the issuer of the client certificate as obtained from the transport, matches the issuer token passed in to this method.
                if ( !StringComparer.Ordinal.Equals( issuerCertThumbprint, x509Token.Certificate.Thumbprint ) )
                {
                    throw new SecurityTokenException( "Issuer of the client certificate as obtained from the transport does not match the issuer token passed in to this method." );
                }

                return x509Token.Certificate.SubjectName.Name;
            }
        }

        throw new SecurityTokenException( "Untrusted issuer." );
    }
}
