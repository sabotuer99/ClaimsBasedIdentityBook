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
    public override string GetIssuerName( SecurityToken securityToken )
    {
        X509SecurityToken x509Token = securityToken as X509SecurityToken;
        if ( x509Token != null )
        {
            if ( String.Equals( x509Token.Certificate.SubjectName.Name, "CN=localhost" ) )
            {
                return x509Token.Certificate.SubjectName.Name;
            }
        }

        throw new SecurityTokenException( "Untrusted issuer." );
    }
}
