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
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Samples.TrustChannel
{
    class Program
    {
        const string ServiceAddress                     = "http://localhost:8080/CalcService";
        const string STSAddress                         = "http://localhost:8081/STS";
        const string LocalhostCertificateSubjectName    = "CN=localhost";
        const string STSCertificateSubjectName          = "CN=STS";

        static X509Certificate2 RPCertificate = CertificateUtil.GetCertificate( LocalhostCertificateSubjectName,
                                                                                StoreLocation.LocalMachine,
                                                                                StoreName.My );

        static X509Certificate2 STSCertificate = CertificateUtil.GetCertificate( STSCertificateSubjectName,
                                                                                 StoreLocation.LocalMachine,
                                                                                 StoreName.My );

        static void Main( string[] args )
        {
            ServiceHost serviceHost = null;
            WSTrustServiceHost securityTokenServiceHost = null;

            try
            {
                //
                // Open the calculator service host.
                //
                serviceHost = CreateServiceHost();
                serviceHost.Open();
                Console.WriteLine( "Started the calculator service." );
                WriteEndpoints( serviceHost );

                //
                // Start the STS
                //
                securityTokenServiceHost = CreateSecurityTokenServiceHost();
                securityTokenServiceHost.Open();
                Console.WriteLine( "Started the STS." );
                WriteEndpoints( securityTokenServiceHost );

                //
                // Call the service and let the framework request the
                // token from the STS automatically.
                //
                Console.WriteLine( "Calling the service with an issued token implicitly acquired using WCF..." );
                CallService();

                //
                // Use the WSTrustChannel component to manually acquire
                // the issued token and use it to secure a request to
                // the web service.
                //
                Console.WriteLine( "Calling the service with an issued token explicitly acquired using the WSTrustChannel..." );
                CallServiceWithExplicitToken( GetIssuedToken() );

                serviceHost.Close();
                serviceHost = null;

                securityTokenServiceHost.Close();
                securityTokenServiceHost = null;
            }
            catch ( Exception ex )
            {
                Console.WriteLine( "=== Unexpected exception caught ===" );
                Console.WriteLine( ex.ToString() );
            }
            finally
            {
                if ( serviceHost != null )
                {
                    serviceHost.Abort();
                }
                if ( securityTokenServiceHost != null )
                {
                    securityTokenServiceHost.Abort();
                }
            }

            Console.WriteLine( "Press <ENTER> to continue." );
            Console.ReadLine();

        }

        /// <summary>
        /// Uses the WSTrustChannel to retrieve an issued token from the STS.
        /// </summary>
        /// <returns>The SecurityToken issued by the STS.</returns>
        private static SecurityToken GetIssuedToken()
        {
            //
            // Note that the default trust version used by the WSTrustChannel
            // is the trust version found on any security binding element in the
            // WSTrustChannelFactory's binding.
            //
            // However, set the TrustVersion property directly on the WSTrustChannelFactory
            // to be explicit.
            //
            WSTrustChannelFactory trustChannelFactory = new WSTrustChannelFactory( GetSecurityTokenServiceBinding(), new EndpointAddress( STSAddress ) );
            trustChannelFactory.TrustVersion = TrustVersion.WSTrust13;

            WSTrustChannel channel = null;

            try
            {
                //
                // Instantiate the RST object used for the issue request
                // to the STS.
                //
                // To use the February 2005 spec:
                //
                //  RequestSecurityToken rst = new RequestSecurityToken( WSTrustFeb2005Constants.RequestTypes.Issue );
                //
                RequestSecurityToken rst = new RequestSecurityToken( WSTrust13Constants.RequestTypes.Issue );
                rst.AppliesTo = new EndpointAddress( ServiceAddress );

                // Set client entropy, protect with STS's cert.
                // It is not necessary to encrypt the entropy as the message body is encrypted.  
                // This sample shows how to encrypt the entropy for scenarios where it is required.
                rst.Entropy = new Entropy( CreateEntropy(), new X509EncryptingCredentials( STSCertificate ) );

                // Set key type to symmetric.
                rst.KeyType = KeyTypes.Symmetric;

                // Set key size for the symmetric proof key.
                rst.KeySizeInBits = 256;

                //
                // Sends the RST message to the STS and extracts the
                // issued security token in accordance with the WS-Trust
                // specification.
                //
                channel = (WSTrustChannel) trustChannelFactory.CreateChannel();

                SecurityToken token = channel.Issue( rst );

                ((IChannel)channel).Close();
                channel = null;

                trustChannelFactory.Close();
                trustChannelFactory = null;

                return token;
            }
            finally
            {
                if ( channel != null )
                {
                    ( (IChannel)channel ).Abort();
                }

                if ( trustChannelFactory != null )
                {
                    trustChannelFactory.Abort();
                }
            }
        }

        /// <summary>
        /// Helper method to create random entropy.
        /// </summary>
        /// <returns>32 bytes of random data for entropy.</returns>
        static byte[] CreateEntropy()
        {
            byte[] entropy = new byte[32];
            RandomNumberGenerator.Create().GetNonZeroBytes( entropy );
            return entropy;
        }

        /// <summary>
        /// Calls the Calculator service and uses an issued token explicitly rather
        /// than allowing WCF and the Windows Identity Foundation manage the issued token
        /// retrieval process.
        /// </summary>
        /// <param name="token">The security token.</param>
        private static void CallServiceWithExplicitToken( SecurityToken token )
        {
            //
            // Instantiate the ChannelFactory as usual.
            //
            ChannelFactory<ICalculator> calcFactory = new ChannelFactory<ICalculator>( GetClientBinding(), ServiceAddress );
            //
            // Make sure to call this prior to using the CreateChannelWith...()
            // extension methods on the channel factory that the Windows Identity Foundation
            // provides.
            //
            calcFactory.ConfigureChannelFactory();

            ICommunicationObject channel = null;
            bool succeeded = false;
            try
            {
                //
                // Creates a channel that will use the provided issued token to
                // secure the messages sent to the calculator service.
                // Note that the configuration of this channel factory is identical 
                // to the "fully automated" scenario except for the use of this
                // extension method to create the actual channel.
                //
                ICalculator calc = calcFactory.CreateChannelWithIssuedToken( token );
                channel = (ICommunicationObject) calc;

                double sum = calc.Add( 40.0, 2.0 );
                Console.WriteLine( "The sum is {0}", sum );

                channel.Close();
                succeeded = true;
            }
            catch ( CommunicationException e )
            {
                Console.WriteLine( "=== Communication exception caught ===" );
                Console.WriteLine( e.ToString() );
                channel.Abort();
            }
            catch ( TimeoutException )
            {
                Console.WriteLine( "Timed out..." );
                channel.Abort();
            }
            finally
            {
                if ( !succeeded )
                {
                    channel.Abort();
                }
            }
        }

        /// <summary>
        /// Calls the Calculator using WCF and the Windows Identity Foundation to
        /// secure the channel with an issued token.
        /// </summary>
        private static void CallService()
        {            
            ChannelFactory<ICalculator> calcFactory = new ChannelFactory<ICalculator>( GetClientBinding(), ServiceAddress );
            ICommunicationObject channel = null;            

            bool succeeded = false;
            try
            {
                ICalculator calc = calcFactory.CreateChannel();
                channel = (ICommunicationObject) calc;

                double sum = calc.Add( 40.0, 2.0 );
                Console.WriteLine( "The sum is {0}", sum );

                channel.Close();
                succeeded = true;
            }
            catch ( CommunicationException e )
            {
                Console.WriteLine( "=== Communication exception caught ===" );
                Console.WriteLine( e.ToString() );
                channel.Abort();
            }
            catch ( TimeoutException )
            {
                Console.WriteLine( "Timed out..." );
                channel.Abort();
            }
            finally
            {
                if ( !succeeded )
                {
                    channel.Abort();
                }
            }
        }

        private static void WriteEndpoints( ServiceHost host )
        {
            Console.WriteLine( "=>" );
            foreach ( ServiceEndpoint ep in host.Description.Endpoints )
            {
                Console.WriteLine( "    {0}", ep.Address );
            }
            Console.WriteLine();
        }

        private static ServiceHost CreateServiceHost()
        {
            ServiceHost serviceHost = new ServiceHost( typeof( CalculatorService ) );

            serviceHost.Credentials.ServiceCertificate.SetCertificate( LocalhostCertificateSubjectName,
                                                                       StoreLocation.LocalMachine,
                                                                       StoreName.My );

            ServiceEndpoint serviceEndpoint = serviceHost.AddServiceEndpoint( typeof( ICalculator ),
                                                                              GetServiceBinding(),
                                                                              ServiceAddress );
            ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior();
            metadataBehavior.HttpGetEnabled = true;
            metadataBehavior.HttpGetUrl = new Uri( ServiceAddress );
            serviceHost.Description.Behaviors.Add( metadataBehavior );
            serviceHost.AddServiceEndpoint( typeof( IMetadataExchange ), MetadataExchangeBindings.CreateMexHttpBinding(), ServiceAddress + "/mex" );
            //
            // This must be called after all WCF settings are set on the service host so the
            // Windows Identity Foundation token handlers can pick up the relevant settings.
            //
            ServiceConfiguration serviceConfiguration = new ServiceConfiguration();
            serviceConfiguration.SecurityTokenHandlers.Configuration.AudienceRestriction.AllowedAudienceUris.Add( new Uri( ServiceAddress ) );
            serviceConfiguration.IssuerNameRegistry = new X509IssuerNameRegistry( STSCertificateSubjectName );
            FederatedServiceCredentials.ConfigureServiceHost( serviceHost, serviceConfiguration );

            return serviceHost;
        }

        private static WSTrustServiceHost CreateSecurityTokenServiceHost()
        {
            var signingCredentials = new X509SigningCredentials( STSCertificate );
            var stsConfig = new SecurityTokenServiceConfiguration( STSAddress, signingCredentials );
            stsConfig.SecurityTokenService = typeof( CustomSecurityTokenService );
            
            // Add the STS endpoint information
            stsConfig.TrustEndpoints.Add( 
                new ServiceHostEndpointConfiguration( typeof( IWSTrust13SyncContract ), GetSecurityTokenServiceBinding(), STSAddress ) );

            // Setting the certificate will automatically populate the token resolver 
            // that is used to decrypt the client entropy in the RST.
            stsConfig.ServiceCertificate = STSCertificate;

            WSTrustServiceHost stsHost = new WSTrustServiceHost( stsConfig, new Uri( STSAddress ) );

            return stsHost;
        }

        private static Binding GetServiceBinding()
        {
            //
            // Use the standard WS2007FederationHttpBinding
            //
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding();

            binding.Security.Message.IssuerAddress = new EndpointAddress( STSAddress );
            binding.Security.Message.IssuerBinding = GetSecurityTokenServiceBinding();
            binding.Security.Message.IssuerMetadataAddress = new EndpointAddress( STSAddress + "/mex" );

            return binding;
        }

        private static Binding GetSecurityTokenServiceBinding()
        {
            //
            // Use the standard WindowsWSTrustBinding
            //
            return new WindowsWSTrustBinding();
        }

        private static Binding GetClientBinding()
        {
            return GetServiceBinding();
        }
    }
}
