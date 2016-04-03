//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Litware.SimulatedIssuer
{
    using System;
    using System.Collections.Generic;
    using System.Web;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Web;
    using Microsoft.IdentityModel.Web.Configuration;

    public class Global : HttpApplication
    {
        private static void OnServiceConfigurationCreated(object sender, ServiceConfigurationCreatedEventArgs e)
        {
            // Use the <serviceCertificate> to protect the cookies that are
            // sent to the client.
            var sessionTransforms =
                new List<CookieTransform>(
                    new CookieTransform[]
                        {
                            new DeflateCookieTransform(), 
                            new RsaEncryptionCookieTransform(e.ServiceConfiguration.ServiceCertificate), 
                            new RsaSignatureCookieTransform(e.ServiceConfiguration.ServiceCertificate)
                        });

            var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());

            e.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
        }

        private void Application_Start(object sender, EventArgs e)
        {
            FederatedAuthentication.ServiceConfigurationCreated += OnServiceConfigurationCreated;
        }
    }
}