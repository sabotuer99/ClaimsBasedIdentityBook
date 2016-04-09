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
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using System.IdentityModel.Selectors;

namespace STS
{
    class STS
    {
        static void Main( string[] args )
        {
            // Create and setup the configuration for our STS
            SecurityTokenServiceConfiguration config = new SecurityTokenServiceConfiguration( "STS" );
            
            // Add the STS endpoint information
            config.TrustEndpoints.Add( 
                new ServiceHostEndpointConfiguration( typeof( IWSTrust13SyncContract ), GetSTSBinding(), "http://localhost:6000/STS" ) );

            // Set the STS implementation class type
            config.SecurityTokenService = typeof( CustomSecurityTokenService );
            SecurityTokenHandlerCollection actAsHandlerCollection = config.SecurityTokenHandlerCollectionManager[SecurityTokenHandlerCollectionManager.Usage.ActAs];

            actAsHandlerCollection.Configuration.IssuerNameRegistry = new ActAsIssuerNameRegistry();
            // The token that we receive in the <RequestSecurityToken><ActAs> element was issued to the service proxies. 
            // By adding the proxy audience URIs here we are enforcing the implicit contract that the STS will accept
            // only tokens issued to the proxy as an ActAs token.
            actAsHandlerCollection.Configuration.AudienceRestriction.AllowedAudienceUris.Add( new Uri( "https://localhost/WFE/default.aspx" ) );
            actAsHandlerCollection.Configuration.AudienceRestriction.AllowedAudienceUris.Add( new Uri( "http://localhost/Service1/Service1.svc" ) );

            // Create the WS-Trust service host with our STS configuration
            using ( WSTrustServiceHost host = new WSTrustServiceHost( config, new Uri( "http://localhost:6000/STS" ) ) )
            {
                host.Open();

                Console.WriteLine( "STS started, press ENTER to stop ..." );
                Console.ReadLine();

                host.Close();
            }
        }

        static Binding GetSTSBinding()
        {
            return new WindowsWSTrustBinding();
        }
    }
}
