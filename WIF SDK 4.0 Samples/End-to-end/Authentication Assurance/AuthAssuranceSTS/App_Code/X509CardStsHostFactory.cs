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

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.SecurityTokenService;

namespace AuthAssuranceSTS
{
    /// <summary>
    /// A custom WSTrustServiceHostFactory that creates a custom SecurityTokenServiceConfiguration.
    /// </summary>
    public class X509CardStsHostFactory : WSTrustServiceHostFactory
    {
        X509Certificate2 _sslCert = CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" );

        public static string StsEndpointAddress = "https://localhost/AuthAssuranceSTS/Service.svc";
        public static string StsMexAddress = "https://localhost/AuthAssuranceSTS/Service.svc/mex";

        /// <summary>
        /// Creates a ServiceHost instance. To register a Security Token Service in IIS create 
        /// a .svc file in the following format.
        /// &lt;%@ServiceHost  language=c# Debug="true" Factory="X509CardStsHostFactory" Service="&lt;CustomSecurityTokenServiceConfiguration>" %>
        /// </summary>
        /// <param name="constructorString">The STSConfiguration name that is passed to the Service argument in the .svc file.</param>
        /// <param name="baseAddresses">The address under which the .svc file is registered.</param>
        /// <returns>Instance of Service Host.</returns>
        /// <exception cref="ArgumentNullException">One of the input arguments is either null or is an empty string.</exception>
        /// <exception cref="InvalidOperationException">The 'constructorString' parameter does not refer to a type 'SecurityTokenServiceConfiguration'.</exception>
        public override ServiceHostBase CreateServiceHost( string constructorString, Uri[] baseAddresses )
        {
            if ( String.IsNullOrEmpty( constructorString ) )
            {
                throw new ArgumentNullException( "constructorString" );
            }

            if ( baseAddresses == null )
            {
                throw new ArgumentNullException( "baseAddresses" );
            }

            SecurityTokenServiceConfiguration securityTokenServiceConfiguration = CreateSecurityTokenServiceConfiguration( constructorString );
            if ( securityTokenServiceConfiguration == null )
            {
                throw new InvalidOperationException( "CreateSecurityTokenServiceConfiguration returned a null SecurityTokenServiceConfiguration object" );
            }

            WSTrustServiceHost serviceHost = new WSTrustServiceHost( securityTokenServiceConfiguration, baseAddresses );

            //
            // Configure an EndpointIdentity for the service host.
            // If no EndpointIdentity is required, you may wish to set the SecurityTokenServiceConfiguration.TrustEndpoints property instead.
            //
            EndpointAddress stsEpr = new EndpointAddress( new Uri( StsEndpointAddress ), 
                                                          EndpointIdentity.CreateX509CertificateIdentity( _sslCert ) );
            serviceHost.Description.Endpoints.Add( new ServiceEndpoint( ContractDescription.GetContract( typeof( IWSTrust13SyncContract ) ),
                                                                        new CertificateWSTrustBinding( SecurityMode.TransportWithMessageCredential ),
                                                                        stsEpr ) );

            serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>().HttpsGetUrl = new Uri( StsMexAddress );

            return serviceHost;
        }
    }
}
