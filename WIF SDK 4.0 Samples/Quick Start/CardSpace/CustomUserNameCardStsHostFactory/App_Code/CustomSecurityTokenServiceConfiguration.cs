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

using System.Security.Cryptography.X509Certificates;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;

/// <summary>
/// A Custom SecurityTokenServiceConfiguration that specifies the CustomSecurityTokenService.
/// </summary>
public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
{
	public CustomSecurityTokenServiceConfiguration( )
        : base( UserNameCardStsHostFactory.StsAddress, new X509SigningCredentials( UserNameCardStsHostFactory.SslCert ) )
	{
        SecurityTokenService = typeof( CustomSecurityTokenService );
    }
}
