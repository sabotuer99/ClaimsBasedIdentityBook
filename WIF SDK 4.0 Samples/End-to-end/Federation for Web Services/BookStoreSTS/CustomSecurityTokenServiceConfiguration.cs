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
    /// Custom SecurityTokenServiceConfiguration for the BookStore SecurityTokenService.
    /// </summary>
    class CustomBookStoreSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        public static X509SigningCredentials x509Cert = new X509SigningCredentials( X509Helper.GetX509Certificate2( BookStoreSTSServiceConfig.CertStoreName,
                                                                        BookStoreSTSServiceConfig.CertStoreLocation,
                                                                        BookStoreSTSServiceConfig.CertDistinguishedName ) );

        /// <summary>
        /// Creates an instance of CustomBookStoreSecurityTokenServiceConfiguration.
        /// </summary>
        public CustomBookStoreSecurityTokenServiceConfiguration()
            : base( BookStoreSTSServiceConfig.IssuerAddress, x509Cert )
        {
            SecurityTokenService = typeof( BookStoreSecurityTokenService );
        }
    }
}
