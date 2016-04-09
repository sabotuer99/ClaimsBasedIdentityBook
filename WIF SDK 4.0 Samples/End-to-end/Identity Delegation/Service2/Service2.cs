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
using System.ServiceModel.Security.Tokens;
using System.Threading;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Service2
{
    [ServiceContract]
    public interface IService2
    {
        [OperationContract]
        string ComputeResponse( string input );
    }

    class Service2 : IService2
    {
        static void Main( string[] args )
        {
            // Configure the issued token parameters with the correct settings
            IssuedSecurityTokenParameters itp = new IssuedSecurityTokenParameters( "http://docs.oasis-open.org/wss/oasis-wss-saml-token-profile-1.1#SAMLV1.1" );
            itp.IssuerMetadataAddress = new EndpointAddress( "http://localhost:6000/STS/mex" );
            itp.IssuerAddress = new EndpointAddress( "http://localhost:6000/STS" );

            // Create the security binding element
            SecurityBindingElement sbe = SecurityBindingElement.CreateIssuedTokenForCertificateBindingElement( itp );
            sbe.MessageSecurityVersion = MessageSecurityVersion.WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;

            // Create the HTTP transport binding element
            HttpTransportBindingElement httpBE = new HttpTransportBindingElement();

            // Create the custom binding using the prepared binding elements
            CustomBinding binding = new CustomBinding( sbe, httpBE );

            Uri serviceUri = new Uri( "http://localhost:6002/Service2" );
            using ( ServiceHost host = new ServiceHost( typeof( Service2 ), serviceUri ) )
            {
                host.AddServiceEndpoint( typeof( IService2 ), binding, "" );
                host.Credentials.ServiceCertificate.SetCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );

                // Enable metadata generation via HTTP GET
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                host.Description.Behaviors.Add( smb );
                host.AddServiceEndpoint( typeof( IMetadataExchange ), MetadataExchangeBindings.CreateMexHttpBinding(), "mex" );

                // Configure the service host to use the Windows Identity Foundation
                ServiceConfiguration configuration = new ServiceConfiguration();
                configuration.IssuerNameRegistry = new TrustedIssuerNameRegistry();
                configuration.SecurityTokenHandlers.Configuration.AudienceRestriction.AllowedAudienceUris.Add( serviceUri );

                FederatedServiceCredentials.ConfigureServiceHost( host, configuration );

                host.Open();

                Console.WriteLine( "Service2 started, press ENTER to stop ..." );
                Console.ReadLine();

                host.Close();
            }
        }

        #region IService2 Members

        public string ComputeResponse( string input )
        {
            // Get the caller's identity.
            IClaimsIdentity identity = (IClaimsIdentity)( (IClaimsPrincipal)Thread.CurrentPrincipal ).Identity;

            PrintCallerIdentity( identity );

            return String.Format( "Computed by Service2 for {0} via {1}: {2} ", identity.Name, identity.Actor == null ? "''" : identity.Actor.Name, input );
        }

        #endregion

        public void PrintCallerIdentity( IClaimsIdentity identity )
        {
            Console.WriteLine( "Caller's name: " + identity.Name );
            foreach ( Claim claim in identity.Claims )
            {
                Console.WriteLine( "ClaimType  : " + claim.ClaimType );
                Console.WriteLine( "ClaimValue : " + claim.Value );
                Console.WriteLine();
            }
            while ( identity.Actor != null )
            {
                Console.WriteLine( "Acting Via: " + identity.Actor.Name );
                foreach ( Claim claim in identity.Actor.Claims )
                {
                    Console.WriteLine( "ClaimType  : " + claim.ClaimType );
                    Console.WriteLine( "ClaimValue : " + claim.Value );
                    Console.WriteLine();
                }
                identity = identity.Actor;
            }
            Console.WriteLine( "===========================" );
        }
    }
}
