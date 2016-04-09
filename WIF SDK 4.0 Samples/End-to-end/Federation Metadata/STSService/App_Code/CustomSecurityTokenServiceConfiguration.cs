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
using System.Web.Configuration;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;

/// <summary>
/// A custom SecurityTokenServiceConfiguration implementation.
/// </summary>
public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
{
    /// <summary>
    /// CustomSecurityTokenServiceConfiguration constructor.
    /// </summary>
    public CustomSecurityTokenServiceConfiguration()
        : base( WebConfigurationManager.AppSettings[Common.IssuerName],
                new X509SigningCredentials( CertificateUtil.GetCertificate(
                    StoreName.My, StoreLocation.LocalMachine,
                    WebConfigurationManager.AppSettings[Common.SigningCertificateName] ) ) )
    {
        this.SecurityTokenService = typeof( CustomSecurityTokenService );
    }
}
