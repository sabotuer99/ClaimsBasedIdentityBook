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
using System.ServiceModel.Description;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.SecurityTokenService;


namespace ClaimsAwareWebService
{
    class Program
    {
        static readonly string SigningCertificateName = "CN=localhost";

        static void Main( string[] args )
        {
            // Create and setup the configuration for our STS
            SigningCredentials signingCreds = new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, SigningCertificateName ) );
            SecurityTokenServiceConfiguration config = new SecurityTokenServiceConfiguration( "http://SecurityTokenService", signingCreds );

            // Add the STS endoint information
            config.TrustEndpoints.Add( 
                new ServiceHostEndpointConfiguration( typeof( IWSTrust13SyncContract ), new WindowsWSTrustBinding(), "http://localhost:6000/SecurityTokenService" ) );

            // Set the STS implementation class type
            config.SecurityTokenService = typeof( MySecurityTokenService );

            // Create the WS-Trust service host with our STS configuration
            using ( WSTrustServiceHost host = new WSTrustServiceHost( config, new Uri( "http://localhost:6000/SecurityTokenService" ) ) )
            {
                host.Open();
                Console.WriteLine( "SecurityTokenService started, press ENTER to stop ..." );
                Console.ReadLine();
                host.Close();
            }
        }
    }
}
