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

public class TrustedIssuerNameRegistry : IssuerNameRegistry
{
    /// <summary>
    ///  Returns the issuer Name from the security token.
    /// </summary>
    /// <param name="securityToken">The security token that contains the STS's certificates.</param>
    /// <returns>The name of the issuer who signed the security token.</returns>
    public override string GetIssuerName( SecurityToken securityToken )
    {
        X509SecurityToken x509Token = securityToken as X509SecurityToken;
        if ( x509Token != null )
        {
            //Note: This piece of code is for illustrative purposes only. Validating certificates based on 
            //subject name is not a good practice.  This code should not be used as is in production.
            if ( String.Equals( x509Token.Certificate.SubjectName.Name, "CN=localhost" ) )
            {
                return x509Token.Certificate.SubjectName.Name;
            }
        }

        throw new SecurityTokenException( "Untrusted issuer." );
    }
}
