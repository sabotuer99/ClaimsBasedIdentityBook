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
using System.ServiceModel.Description;
using System.Threading;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ClaimsAwareWebService
{
    [ServiceContract]
    public interface IClaimsAwareWebService
    {
        [OperationContract]
        string ComputeResponse( string input );
    }

    class ClaimsAwareWebService : IClaimsAwareWebService
    {
        static void Main( string[] args )
        {
            WS2007FederationHttpBinding binding = new WS2007FederationHttpBinding();
            binding.Security.Message.IssuerAddress = new EndpointAddress( "http://localhost:6000/SecurityTokenService" );
            binding.Security.Message.IssuerMetadataAddress = new EndpointAddress( "http://localhost:6000/SecurityTokenService/mex" );

            Uri serviceUri = new Uri( "http://localhost:6020/ClaimsAwareWebService" );
            using ( ServiceHost host = new ServiceHost( typeof( ClaimsAwareWebService ), serviceUri ) )
            {
                host.AddServiceEndpoint( typeof( IClaimsAwareWebService ), binding, "" );

                // Configure our certificate and issuer certificate validation settings on the service credentials
                host.Credentials.ServiceCertificate.SetCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );

                // Enable metadata generation via HTTP GET
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                host.Description.Behaviors.Add( smb );
                            
                host.AddServiceEndpoint(typeof( IMetadataExchange), MetadataExchangeBindings.CreateMexHttpBinding(), "mex" );


                // Configure the service host to use the Windows Identity Foundation
                ServiceConfiguration configuration = new ServiceConfiguration();
                configuration.IssuerNameRegistry = new TrustedIssuerNameRegistry();
                configuration.SecurityTokenHandlers.Configuration.AudienceRestriction.AllowedAudienceUris.Add( serviceUri );

                FederatedServiceCredentials.ConfigureServiceHost( host, configuration );

                host.Open();

                Console.WriteLine( "ClaimsAwareWebService started, press ENTER to stop ..." );
                Console.ReadLine();

                host.Close();
            }
        }

        #region IClaimsAwareWebService Members

        public string ComputeResponse( string input )
        {
            // Get the caller's identity from ClaimsPrincipal.Current
            IClaimsIdentity identity = (IClaimsIdentity)((IClaimsPrincipal)Thread.CurrentPrincipal).Identity;

            /* For illustrative purposes this sample application simply shows all the parameters of 
             * claims (i.e. claim types and claim values), which are issued by a security token 
             * service (STS), in its console window. In production code, security implications of echoing 
             * the properties of claims should be carefully considered. For example, 
             * some of the security considerations are: (i) accepting the only claim types that are 
             * expected by relying party applications; (ii) sanitizing the claim parameters before 
             * using them; and (iii) filtering out claims that contain sensitive personal information). 
             * DO NOT use this sample code ‘as is’ in production code.
            */

            Console.WriteLine( "\nCaller's name: " + identity.Name + "\nClaims issued are:\n" );
            foreach ( Claim claim in identity.Claims )
            {
                Console.WriteLine( "ClaimType  : " + claim.ClaimType );
                Console.WriteLine( "ClaimValue : " + claim.Value );
                Console.WriteLine();
            }
            Console.WriteLine( "===========================" );

            return String.Format( "Computed by ClaimsAwareWebService for {0}: {1} ", identity.Name, input );
        }

        #endregion
    }
}
