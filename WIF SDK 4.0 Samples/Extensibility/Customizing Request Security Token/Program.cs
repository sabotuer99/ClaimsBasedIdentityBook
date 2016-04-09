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
using System.Xml;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Samples.CustomRequestSecurityToken
{ 
    class Program
    {
        public static string securityTokenServiceAddress = "http://localhost:8081/STS";
        static void Main()
        {
            ServiceHost serviceHost = null;
            ChannelFactory<IEcho> echoChannelFactory = null;
            WSTrustServiceHost securityTokenServiceHost = null;

            try
            {
                //
                // Start the service
                //
                serviceHost = new ServiceHost( typeof( EchoService ) );
                string serviceAddress = "http://localhost:8080/EchoService";

                serviceHost.AddServiceEndpoint( typeof( IEcho ), GetServiceBinding(), serviceAddress );
                ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior();
                metadataBehavior.HttpGetEnabled = true;
                metadataBehavior.HttpGetUrl = new Uri( serviceAddress );
                serviceHost.Description.Behaviors.Add( metadataBehavior );
                serviceHost.AddServiceEndpoint( typeof( IMetadataExchange ), MetadataExchangeBindings.CreateMexHttpBinding(), serviceAddress + "/mex" );
                serviceHost.Credentials.ServiceCertificate.SetCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );
                
                serviceHost.Open();
                Console.WriteLine( "The echo service has started at {0}.\n", serviceAddress );

                //
                // Start the STS
                //
                SecurityTokenServiceConfiguration securityTokenServiceConfiguration = new SecurityTokenServiceConfiguration( securityTokenServiceAddress, new X509SigningCredentials( serviceHost.Credentials.ServiceCertificate.Certificate ) );
                securityTokenServiceConfiguration.WSTrust13RequestSerializer = new CustomWSTrust13RequestSerializer();
                securityTokenServiceConfiguration.SecurityTokenService = typeof( CustomSecurityTokenService );

                // Add the STS endpoint information
                securityTokenServiceConfiguration.TrustEndpoints.Add( 
                    new ServiceHostEndpointConfiguration( typeof ( IWSTrust13SyncContract ), GetSecurityTokenServiceBinding(), securityTokenServiceAddress ) );

                securityTokenServiceHost = new WSTrustServiceHost( securityTokenServiceConfiguration, new Uri( securityTokenServiceAddress ) );                               
                securityTokenServiceHost.Open();

                Console.WriteLine( "The security token service has started at {0}.\n", securityTokenServiceAddress );

                //
                // Invoke the client
                //
                echoChannelFactory = new ChannelFactory<IEcho>( GetClientBinding(), new EndpointAddress( new Uri( serviceAddress ), EndpointIdentity.CreateDnsIdentity( "localhost" ) ) );

                IEcho client = echoChannelFactory.CreateChannel();
                ( (IClientChannel)client ).OperationTimeout = TimeSpan.MaxValue;

                Console.WriteLine( "The client sent a request to the STS to retrieve a SAML token and then sent the hello request to the echo service.\n" );
                Console.WriteLine( "The echo service finally returned '{0}'.\n", client.Echo( "Hello" ) );

                Console.WriteLine( "Press [Enter] to continue." );
                Console.ReadLine();
            }
            catch ( Exception ex )
            {
                Console.WriteLine( ex.ToString() );
            }
            finally
            {
                try
                {
                    if ( echoChannelFactory != null )
                    {
                        echoChannelFactory.Close();
                    }

                    if ( serviceHost != null )
                    {
                        serviceHost.Close();
                    }

                    if ( securityTokenServiceHost != null )
                    {
                        securityTokenServiceHost.Close();
                    }
                }
                catch ( CommunicationException )
                {   
                }
            }
        }

        /// <summary>
        /// Returns the service binding.
        /// </summary>
        /// <returns>A WS2007FederationHttpBinding object for the service.</returns>
        static Binding GetServiceBinding()
        {
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding( WSFederationHttpSecurityMode.Message );
            binding.Security.Message.IssuerMetadataAddress = new EndpointAddress( securityTokenServiceAddress + "/mex" );
            return binding;
        }

        /// <summary>
        /// Returns the binding for the client. This method also modifies the security token request to include
        /// the custom element.
        /// </summary>
        /// <returns>The client's binding.</returns>
        static Binding GetClientBinding()
        {
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding( WSFederationHttpSecurityMode.Message );

            binding.Security.Message.IssuerAddress = new EndpointAddress( "http://localhost:8081/STS" );
            binding.Security.Message.IssuerBinding = GetSecurityTokenServiceBinding();

            #region Make the client send out additional RST element
            //
            // The TokenRequestParameters is a place where you can send some custom element in the issue request
            // Comment out this section will make the sample fail
            //
            XmlDocument doc = new XmlDocument();
            XmlElement customElement = doc.CreateElement( CustomElementConstants.Prefix, CustomElementConstants.LocalName, CustomElementConstants.Namespace );

            bool sendCorrectValue = true; // change this to false will make the sample fail

            if ( sendCorrectValue )
            {
                customElement.InnerText = CustomElementConstants.DefaultElementValue;
            }
            else
            {
                customElement.InnerText = "IncorrectValue";
            }            

            binding.Security.Message.TokenRequestParameters.Add( customElement );

            #endregion

            return binding;
        }

        /// <summary>
        /// Returns the security token service binding.
        /// </summary>
        /// <returns>A WindowsWSTrustBinding object for the STS's binding.</returns>
        static Binding GetSecurityTokenServiceBinding()
        {
            return new WindowsWSTrustBinding();
        }
    }
}
