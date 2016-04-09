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
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Implements an IssuerNameRegistry that checks whether the given Issuer Token is trusted.
/// If the token is trusted it returns a name for the issuer.
/// </summary>
public class TrustedIssuerNameRegistry : IssuerNameRegistry
{
    /// <summary>
    /// Checks whether the given token is an X509SecurityToken and the associated Certificate has a subject name of "CN=localhost". 
    /// </summary>
    /// <param name="securityToken">Issuer token.</param>
    /// <returns>Name of the Issuer.</returns>
    /// <exception cref="SecurityTokenException">The given issuer token is unknown.</exception>
    public override string GetIssuerName( SecurityToken securityToken )
    {
        X509SecurityToken x509Token = securityToken as X509SecurityToken;
        if ( x509Token != null )
        {
            //
            // WARNING: This demonstrates a simple implementation that just checks the 
            // certificate subject name. In production systems this might look up the 
            // certificate in a database(of trusted certificates) and might do additional 
            // validation like checking the certificate for revocation.
            //
            // DO NOT use this sample code 'as is' in production code.
            //
            if ( String.Equals( x509Token.Certificate.SubjectName.Name, "CN=localhost" ) )
            {
                return x509Token.Certificate.SubjectName.Name;
            }
        }

        throw new SecurityTokenException( "Untrusted issuer." );
    }
}
