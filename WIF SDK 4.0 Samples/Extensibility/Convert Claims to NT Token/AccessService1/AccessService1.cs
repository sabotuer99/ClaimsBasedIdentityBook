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
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.Samples.WindowsTokenService.Common;

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.AccessService1
{
    /// <summary>
    /// The AccessService1 implementation. This service uses the SAML token handler’s map-to-windows 
    /// feature and is configured to use the C2WTS to create an impersonate-level windows identity.
    /// </summary>
    public class AccessService1 : IAccess
    {
        /// <summary>
        /// This method accesses the back-end service PrintIdentityService doing impersonation.
        /// </summary>
        /// <param name="address">The back-end service address.</param>
        /// <exception cref="AccessException">When the address is null or empty string.</exception>
        public void Access( string address )
        {
            if ( string.IsNullOrEmpty( address ) )
            {
                throw new Exception( "Address is null or empty" );
            }

            if ( Thread.CurrentPrincipal.Identity is WindowsIdentity )
            {
                WindowsIdentity currentIdentity = ( WindowsIdentity )Thread.CurrentPrincipal.Identity;

                using ( WindowsImpersonationContext ctxt = currentIdentity.Impersonate() )
                {
                    CallService( address );
                }
            }
            else
            {
                throw new InvalidCastException( "Invalid windows identity." );
            }
        }

        /// <summary>
        /// Calls the back end print identity service.
        /// </summary>
        /// <param name="address">The address of the back end service.</param>
        private static void CallService( string address )
        {
            ChannelFactory<IPrintIdentity> printServiceChannel = null;
            try
            {
                printServiceChannel = new ChannelFactory<IPrintIdentity>( new WindowsWSTrustBinding(),
                                                                          new EndpointAddress( new Uri( address ),
                                                                                               EndpointIdentity.CreateDnsIdentity( "localhost" ) ) );
                IPrintIdentity client = printServiceChannel.CreateChannel();
                ( ( IClientChannel )client ).OperationTimeout = TimeSpan.MaxValue;

                Console.WriteLine( "The access service 1 is accessing the print service.\n" );
                client.Print();
            }
            finally
            {
                try
                {
                    if ( printServiceChannel != null )
                    {
                        printServiceChannel.Close();
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
    }
}

