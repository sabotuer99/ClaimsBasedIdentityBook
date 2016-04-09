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


using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    /// <summary>
    /// Custom SecurityTokenServiceConfiguration for the HomeRealm SecurityTokenService.
    /// </summary>
    class CustomHomeRealmSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        public static X509SigningCredentials x509Cert = new X509SigningCredentials( X509Helper.GetX509Certificate2( HomeRealmSTSServiceConfig.CertStoreName,
                                                                HomeRealmSTSServiceConfig.CertStoreLocation,
                                                                HomeRealmSTSServiceConfig.CertDistinguishedName ) );

        /// <summary>
        /// Creates an instance of CustomHomeRealmSecurityTokenServiceConfiguration.
        /// </summary>
        public CustomHomeRealmSecurityTokenServiceConfiguration()
            : base( HomeRealmSTSServiceConfig.IssuerAddress, x509Cert )
        {
            SecurityTokenService = typeof( HomeRealmSecurityTokenService );
        }
    }
}
