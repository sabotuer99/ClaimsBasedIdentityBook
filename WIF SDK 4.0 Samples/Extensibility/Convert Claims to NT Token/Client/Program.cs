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
using System.ServiceModel.Security;

using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.Samples.WindowsTokenService.Common;

namespace Client
{
    /// <summary>
    /// Hosts the client application.
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            ChannelFactory<IAccess> channel = null;
            
            //
            // NOTE: Replace the string below with Address.ServiceAddress2 to utilize AccessService2.
            //
            string serviceAddressUsed = Address.ServiceAddress1;

            try
            {
                channel = new ChannelFactory<IAccess>( GetClientBinding(), 
                                                       new EndpointAddress( new Uri( serviceAddressUsed ), 
                                                                            EndpointIdentity.CreateDnsIdentity( "localhost" ) ) );
                IAccess client = channel.CreateChannel();
                ( ( IClientChannel )client ).OperationTimeout = TimeSpan.MaxValue;

                Console.WriteLine( "The client has started and is accessing the service hosted at address {0}.\n", serviceAddressUsed );
                client.Access( Address.PrintServiceAddress );
                
                Console.WriteLine( "Press [Enter] to stop.\n" );
                Console.ReadLine();
            }
            catch( MessageSecurityException )
            {
                Console.WriteLine( "Message security exception. Possibly the STS issued an invalid UPN claim." );
                Console.WriteLine( "Press [Enter] to stop.\n" );
                Console.ReadLine();
            }
            finally
            {
                try
                {
                    if ( channel != null )
                    {
                        channel.Close();
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

        static Binding GetClientBinding()
        {
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding( WSFederationHttpSecurityMode.Message );

            binding.Security.Message.IssuerAddress = new EndpointAddress( "http://localhost:8081/STS" );

            binding.Security.Message.IssuerBinding = new WindowsWSTrustBinding();

            return binding;
        }

    }
}
