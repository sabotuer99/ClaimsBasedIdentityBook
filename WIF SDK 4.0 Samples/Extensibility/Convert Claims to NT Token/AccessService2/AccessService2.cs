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
using System.ServiceModel.Security;
using System.Threading;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.Samples.WindowsTokenService.Common;
using Microsoft.IdentityModel.WindowsTokenService;

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.AccessService2
{
    /// <summary>
    /// The AccessService2 implementation. This implementation explicitly does the UPN logon through the S4UClient.
    /// The S4UClient.UpnLogon gets an NT token from the C2WTS service using the UPN claim 
    /// and constructs a WindowsIdentity from the NT token.
    /// </summary>
    public class AccessService2 : IAccess
    {
        /// <summary>
        /// This method accesses the back-end service PrintIdentityService doing impersonation.
        /// </summary>
        /// <param name="address">The back end service address.</param>
        /// <exception cref="AccessException">When the address is null or empty string.</exception>
        public void Access( string address )
        {
            if ( string.IsNullOrEmpty( address ) )
            {
                throw new Exception( "Address is null or empty" );
            }

            // Gets the current identity and extracts the UPN claim.
            IClaimsIdentity identity = ( ClaimsIdentity )Thread.CurrentPrincipal.Identity;
            string upn = null;
            foreach ( Claim claim in identity.Claims )
            {
                if ( StringComparer.Ordinal.Equals( Microsoft.IdentityModel.Claims.ClaimTypes.Upn, claim.ClaimType ) )
                {
                    upn = claim.Value;
                }
            }

            // Performs the UPN logon through the C2WTS service.
            WindowsIdentity windowsIdentity = null;
            if ( !String.IsNullOrEmpty( upn ) )
            {
                try
                {
                    windowsIdentity = S4UClient.UpnLogon( upn );
                }
                catch ( SecurityAccessDeniedException )
                {
                    Console.WriteLine( "Could not map the upn claim to a valid windows identity." );
                    return;
                }
            }
            else
            {
                throw new Exception( "No UPN claim found" );
            }

            using ( WindowsImpersonationContext ctxt = windowsIdentity.Impersonate() )
            {
                CallService( address );
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

                Console.WriteLine( "The access service 2 is accessing the print service.\n" );
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
