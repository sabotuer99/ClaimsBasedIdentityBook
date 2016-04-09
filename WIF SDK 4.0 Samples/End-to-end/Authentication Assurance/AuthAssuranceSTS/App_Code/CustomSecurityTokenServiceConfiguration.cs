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
using System.ServiceModel.Channels;
using System.Web;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;

namespace AuthAssuranceSTS
{
    /// <summary>
    /// Extends the Microsoft.IdentityModel.Services.SecurityTokenServiceConfiguration class to 
    /// be consumed by the CustomSecurityTokenService.
    /// </summary>
    public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        public static string StsIssuerAddress = "https://localhost/AuthAssuranceSTS/";

        static readonly object syncRoot = new object();
        static string CustomSecurityTokenServiceConfigurationKey = "CustomSecurityTokenServiceConfigurationKey";

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

        public CustomSecurityTokenServiceConfiguration()
            : base( StsIssuerAddress, new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" ) ) )
        {
            SecurityTokenService = typeof(AuthAssuranceSTS.CustomSecurityTokenService);
        }
    }
}
