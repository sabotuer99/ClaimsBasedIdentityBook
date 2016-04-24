//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Fabrikam.SimulatedIssuer
{
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Permissions;
    using System.Web;
    using System.Web.Configuration;
    using Microsoft.IdentityModel.Configuration;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Microsoft.IdentityModel.Tokens;
    using Samples.Web.ClaimsUtilities;

    public class IdentityProviderSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        private const string CustomSecurityTokenServiceConfigurationKey = "IdentityProviderSecurityTokenServiceConfigurationKey";
        private static readonly object syncRoot = new object();

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public IdentityProviderSecurityTokenServiceConfiguration()
            : base(
                WebConfigurationManager.AppSettings[ApplicationSettingsNames.IssuerName], 
                new X509SigningCredentials(
                    CertificateUtilities.GetCertificate(
                        StoreName.My, 
                        StoreLocation.LocalMachine, 
                        WebConfigurationManager.AppSettings[ApplicationSettingsNames.SigningCertificateName])))
        {
            this.SecurityTokenService = typeof(IdentityProviderSecurityTokenService);
            this.DefaultTokenType = SecurityTokenTypes.Saml2TokenProfile11;
        }

        public static IdentityProviderSecurityTokenServiceConfiguration Current
        {
            get
            {
                var httpAppState = HttpContext.Current.Application;

                var customConfiguration = httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as IdentityProviderSecurityTokenServiceConfiguration;

                if (customConfiguration == null)
                {
                    lock (syncRoot)
                    {
                        customConfiguration = httpAppState.Get(CustomSecurityTokenServiceConfigurationKey) as IdentityProviderSecurityTokenServiceConfiguration;

                        if (customConfiguration == null)
                        {
                            customConfiguration = new IdentityProviderSecurityTokenServiceConfiguration();
                            httpAppState.Add(CustomSecurityTokenServiceConfigurationKey, customConfiguration);
                        }
                    }
                }

                return customConfiguration;
            }
        }
    }
}