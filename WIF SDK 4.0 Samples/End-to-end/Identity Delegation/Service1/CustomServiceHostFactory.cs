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
using System.ServiceModel;
using System.ServiceModel.Activation;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Service1
{
    /// <summary>
    /// A Custom ServiceHostFactory that creates a ServiceHost and sets the correct IssuerNameRegistry.
    /// </summary>
    public class CustomServiceHostFactory : ServiceHostFactory
    {
        /// <summary>
        /// Overrides the base class method. Configures the appropriate IssuerNameRegistry on
        /// the ServiceHost created by the base class.
        /// </summary>
        /// <param name="constructorString">Custom parameter specified in the 'Service' attribute of the .svc files.</param>
        /// <param name="baseAddresses">Collection of base address of the service obtained from the hosting layer (IIS).</param>
        /// <returns>Instance of ServiceHostBase.</returns>
        public override ServiceHostBase CreateServiceHost( string constructorString, Uri[] baseAddresses )
        {
            ServiceHostBase host = base.CreateServiceHost( constructorString, baseAddresses );

            // setup a trusted issuer name registry
            ServiceConfiguration configuration = new ServiceConfiguration();
            configuration.IssuerNameRegistry = new TrustedIssuerNameRegistry();

            // The default audience restriction's AudienceMode is set to Always, so we must 
            // specify the precise audiences we will accept on issued tokens.
            configuration.SecurityTokenHandlers.Configuration.AudienceRestriction.AllowedAudienceUris.Add( new Uri( "http://localhost/Service1/Service1.svc" ) );

            // Save any bootstrap tokens in session state.
            configuration.SaveBootstrapTokens = true;

            FederatedServiceCredentials.ConfigureServiceHost( host, configuration );

            return host;
        }
    }
}
