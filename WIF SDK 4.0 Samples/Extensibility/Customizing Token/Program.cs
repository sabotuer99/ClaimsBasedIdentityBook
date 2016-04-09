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
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Samples.CustomToken
{
    class Program
    {
        public const string securityTokenServiceAddress = "http://localhost:8081/STS";
        static void Main()
        {
            ServiceHost serviceHost = null;
            ChannelFactory<IEcho> echoChannelFactory = null;
            WSTrustServiceHost trustServiceHost = null;

            try
            {
                CustomTokenHandler handler = new CustomTokenHandler();

                //
                // Start the service
                //
                serviceHost = new ServiceHost( typeof( EchoService ) );
                string serviceAddress = "http://localhost:8080/EchoService";

                ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior();
                metadataBehavior.HttpGetEnabled = true;
                metadataBehavior.HttpGetUrl = new Uri( serviceAddress );
                serviceHost.Description.Behaviors.Add( metadataBehavior );
                serviceHost.AddServiceEndpoint( typeof( IEcho ), GetServiceBinding(), serviceAddress );
                serviceHost.AddServiceEndpoint( typeof( IMetadataExchange ), MetadataExchangeBindings.CreateMexHttpBinding(), serviceAddress + "/mex" );
                serviceHost.Credentials.ServiceCertificate.SetCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );

                FederatedServiceCredentials.ConfigureServiceHost( serviceHost );
                //
                // Update the service credentials so that it can deserialize the custom token 
                //
                ( ( FederatedServiceCredentials )serviceHost.Credentials ).SecurityTokenHandlers.Add( handler );
                serviceHost.Open();
                Console.WriteLine( "The echo service has started at {0}.\n", serviceAddress );

                //
                // Start the SecurityTokenService
                //
                X509Certificate2 certificate = CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" );
                SigningCredentials credentials = new X509SigningCredentials( certificate );
                SecurityTokenServiceConfiguration securityTokenServiceConfiguration = new SecurityTokenServiceConfiguration( securityTokenServiceAddress, credentials );
                securityTokenServiceConfiguration.SecurityTokenService = typeof( SampleTokenService );

                // register a handler to the SecurityTokenService here so that it can issue the custom token
                securityTokenServiceConfiguration.SecurityTokenHandlers.Add( handler );

                // Add the STS endpoint information
                securityTokenServiceConfiguration.TrustEndpoints.Add( 
                    new ServiceHostEndpointConfiguration( typeof ( IWSTrust13SyncContract ), GetSecurityTokenServiceBinding(), securityTokenServiceAddress ) );

                securityTokenServiceConfiguration.ServiceCertificate = certificate;

                trustServiceHost = new WSTrustServiceHost( securityTokenServiceConfiguration, new Uri( securityTokenServiceAddress ) );
                trustServiceHost.Open();
                Console.WriteLine( "The security token service has started at {0}.\n", securityTokenServiceAddress );

                //
                // Invoke the client
                //
                echoChannelFactory = new ChannelFactory<IEcho>( GetClientBinding(), new EndpointAddress( new Uri( serviceAddress ), EndpointIdentity.CreateDnsIdentity( "localhost" ) ) );

                IEcho client = echoChannelFactory.CreateChannel();
                ( (IClientChannel)client ).OperationTimeout = TimeSpan.MaxValue;

                string echoedString = client.Echo( "Hello" );
                Console.WriteLine( "The echo service returns '{0}'. \n", echoedString );

                echoChannelFactory.Close();

                Console.WriteLine( "Press [Enter] to continue." );
                Console.ReadLine();
            }
            catch ( CommunicationException e )
            {
                Console.WriteLine( e.Message );
                if ( echoChannelFactory != null )
                {
                    echoChannelFactory.Abort();
                }
            }
            catch ( TimeoutException e )
            {
                Console.WriteLine( e.Message );
                if ( echoChannelFactory != null )
                {
                    echoChannelFactory.Abort();
                }
            }
            finally
            {
                if ( serviceHost != null && serviceHost.State != CommunicationState.Faulted )
                {
                    serviceHost.Close();
                }

                if ( trustServiceHost != null && trustServiceHost.State != CommunicationState.Faulted )
                {
                    trustServiceHost.Close();
                }
            }
        }

        /// <summary>
        /// Gets the service binding.
        /// </summary>
        /// <returns>A WS2007FederationHttpBinding object for the service.</returns>
        static Binding GetServiceBinding()
        {
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding( WSFederationHttpSecurityMode.Message );

            binding.Security.Message.IssuerAddress = new EndpointAddress( securityTokenServiceAddress );
            binding.Security.Message.IssuerBinding = GetSecurityTokenServiceBinding();
            binding.Security.Message.IssuerMetadataAddress = new EndpointAddress( securityTokenServiceAddress + "/mex" );
            //
            // We need to set up the service binding so that it is expecting a custom token
            //
            binding.Security.Message.IssuedTokenType = CustomTokenHandler.DecisionTokenType;

            return binding;
        }

        /// <summary>
        /// Gets the client binding.
        /// </summary>
        /// <returns>Binding object for the client.</returns>
        static Binding GetClientBinding()
        {
            return GetServiceBinding();
        }

        /// <summary>
        /// Gets binding for the security token service.
        /// </summary>
        /// <returns>A WindowsWSTrustBinding object for the security token service.</returns>
        static Binding GetSecurityTokenServiceBinding()
        {
            return new WindowsWSTrustBinding();
        }
    }
}
