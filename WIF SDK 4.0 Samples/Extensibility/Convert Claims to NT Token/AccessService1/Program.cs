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

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.AccessService1
{
    /// <summary>
    /// Hosts the access service. It uses the automatic mapping of the UPN claim to a windows identity using
    /// the security token handler's configuration.
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            ServiceHost serviceHost = null;

            try
            {
                serviceHost = new ServiceHost( typeof( AccessService1 ), new Uri( Address.ServiceAddress1 ) );
                serviceHost.AddServiceEndpoint( typeof( IAccess ), GetServiceBinding(), String.Empty );
                ServiceMetadataBehavior serviceMetadataBehavior = new ServiceMetadataBehavior();
                serviceMetadataBehavior.HttpGetEnabled = true;
                serviceHost.Description.Behaviors.Add( serviceMetadataBehavior );
                serviceHost.AddServiceEndpoint( typeof( IMetadataExchange ), MetadataExchangeBindings.CreateMexHttpBinding(), Address.ServiceAddress1 + "/mex" );
                serviceHost.Credentials.ServiceCertificate.SetCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );

                ServiceConfiguration configuration = new ServiceConfiguration();
                configuration.IssuerNameRegistry = new TrustedIssuerNameRegistry();

                FederatedServiceCredentials.ConfigureServiceHost( serviceHost, configuration );
                serviceHost.Open();

                Console.WriteLine( "The access service 1 has started at {0}.\n", Address.ServiceAddress1 );
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
