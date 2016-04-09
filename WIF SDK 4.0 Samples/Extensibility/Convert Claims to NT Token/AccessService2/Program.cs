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
using System.ServiceModel.Description;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Samples.WindowsTokenService.Common;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.AccessService2
{
    /// <summary>
    /// Hosts the access service. It uses the explicit mapping of the UPN claim to a windows identity.
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            ServiceHost serviceHost = null;
            try
            {
                serviceHost = new ServiceHost( typeof( AccessService2 ), new Uri( Address.ServiceAddress2 ) );
                serviceHost.AddServiceEndpoint( typeof( IAccess ), GetServiceBinding(), String.Empty );
                ServiceMetadataBehavior serviceMetadataBehavior = new ServiceMetadataBehavior();
                serviceMetadataBehavior.HttpGetEnabled = true;
                serviceHost.Description.Behaviors.Add( serviceMetadataBehavior );
                serviceHost.AddServiceEndpoint( typeof( IMetadataExchange ), MetadataExchangeBindings.CreateMexHttpBinding(), Address.ServiceAddress2 + "/mex" );
                serviceHost.Credentials.ServiceCertificate.SetCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );

                ServiceConfiguration configuration = new ServiceConfiguration();
                configuration.IssuerNameRegistry = new TrustedIssuerNameRegistry();
                configuration.SecurityTokenHandlers.Configuration.AudienceRestriction.AllowedAudienceUris.Add( new Uri( Address.ServiceAddress2 ) );

                FederatedServiceCredentials.ConfigureServiceHost( serviceHost, configuration );
                serviceHost.Open();

                Console.WriteLine( "The access service 2 has started at {0}.\n", Address.ServiceAddress2 );
                Console.WriteLine( "Press [Enter] to stop.\n" );
                Console.ReadLine();
            }
            finally
            {
                try
                {
                    if ( serviceHost != null )
                    {
                        serviceHost.Close();
                    }
                }
                catch ( CommunicationException )
                {
                }
                catch ( TimeoutException )
                {
                }
            }
        }

        private static Binding GetServiceBinding()
        {
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding( WSFederationHttpSecurityMode.Message );
            binding.Security.Message.IssuerMetadataAddress = new EndpointAddress( "http://localhost:8081/STS/mex" );
            return binding;
        }
    }
}
