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

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.Samples.WindowsTokenService.Common;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.SecurityTokenService
{
    /// <summary>
    /// Hosts the security token service.
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            WSTrustServiceHost stsHost = null;
            try
            {
                SecurityTokenServiceConfiguration stsConfiguration = new SecurityTokenServiceConfiguration( Address.StsAddress, 
                                                                                                            new X509SigningCredentials( 
                                                                                                               CertificateUtil.GetCertificate( StoreName.My,                                                                                                                                                                     StoreLocation.LocalMachine, 
                                                                                                                                               "CN=localhost" ) ) );
                stsConfiguration.SecurityTokenService = typeof( CustomSecurityTokenService );
                
                // Add the STS endpoint information
                stsConfiguration.TrustEndpoints.Add( 
                    new ServiceHostEndpointConfiguration( typeof( IWSTrust13SyncContract ), new WindowsWSTrustBinding(), Address.StsAddress ) );

                stsHost = new WSTrustServiceHost( stsConfiguration, new Uri( Address.StsAddress ) );
                stsHost.Open();

                Console.WriteLine( "The security token service has started at {0}.\n", Address.StsAddress );
                Console.WriteLine( "Press [Enter] to stop.\n" );
                Console.ReadLine();
            }
            finally
            {
                try
                {
                    if ( stsHost != null )
                    {
                        stsHost.Close();
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
