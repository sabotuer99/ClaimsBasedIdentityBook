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

using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Web;
using Microsoft.IdentityModel.Web.Configuration;

namespace WFE
{
    public class Global : System.Web.HttpApplication
    {
        public static readonly string CachedChannelFactory = "WFE_CachedChannelFactory";

        void Application_Start( object sender, EventArgs e )
        {
            // Code that runs on application startup
            FederatedAuthentication.ServiceConfigurationCreated += new EventHandler<ServiceConfigurationCreatedEventArgs>( FederatedAuthentication_ServiceConfigurationCreated );
        }

        void FederatedAuthentication_ServiceConfigurationCreated( object sender, EventArgs e )
        {
            ChannelFactory<IService2Channel> service2CF = new ChannelFactory<IService2Channel>( "CustomBinding_IService2" );
            service2CF.Credentials.ServiceCertificate.SetDefaultCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );
            service2CF.ConfigureChannelFactory();

            Application[CachedChannelFactory] = service2CF;
        }

    }
}
