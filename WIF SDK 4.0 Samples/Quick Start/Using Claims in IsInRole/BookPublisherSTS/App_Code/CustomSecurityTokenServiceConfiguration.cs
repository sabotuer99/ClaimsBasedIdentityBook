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
using System.Web;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;

/// <summary>
/// A custom SecurityTokenServiceConfiguration implementation.
/// </summary>
public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
{
    static readonly object syncRoot = new object();
    static string CustomSecurityTokenServiceConfigurationKey = "CustomSecurityTokenServiceConfigurationKey";

    const string issuerName = "PassiveSigninSTS";
    const string SigningCertificateName = "CN=localhost";

    public static CustomSecurityTokenServiceConfiguration Current
    {
        get
        {
            HttpApplicationState httpAppState = HttpContext.Current.Application;

            CustomSecurityTokenServiceConfiguration myConfiguration = httpAppState.Get( CustomSecurityTokenServiceConfigurationKey ) as CustomSecurityTokenServiceConfiguration;

            if ( myConfiguration != null )
            {
                return myConfiguration;
            }

            lock ( syncRoot )
            {
                myConfiguration = httpAppState.Get( CustomSecurityTokenServiceConfigurationKey ) as CustomSecurityTokenServiceConfiguration;

                if ( myConfiguration == null )
                {
                    myConfiguration = new CustomSecurityTokenServiceConfiguration();
                    httpAppState.Add( CustomSecurityTokenServiceConfigurationKey, myConfiguration );
                }

                return myConfiguration;
            }
        }
    }

    /// <summary>
    /// Initializes an instance of CustomSecurityTokenServiceConfiguration.
    /// </summary>
    public CustomSecurityTokenServiceConfiguration()
        : base( issuerName, new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, SigningCertificateName ) ) )
    {
        this.SecurityTokenService = typeof( CustomSecurityTokenService );
    }
}
