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
using System.ServiceModel.Activation;
using Microsoft.IdentityModel.Tokens;


namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    /// <summary>
    /// Creates service instance to handle incoming request.
    /// </summary>
    public class BookStoreServiceHostFactory : ServiceHostFactoryBase
    {
        public override ServiceHostBase CreateServiceHost( string constructorString, Uri[] baseAddresses )
        {
            ServiceHost serviceHost = new BookStoreServiceHost( baseAddresses );
            FederatedServiceCredentials.ConfigureServiceHost(serviceHost);
            return serviceHost;
        }
    }
}
