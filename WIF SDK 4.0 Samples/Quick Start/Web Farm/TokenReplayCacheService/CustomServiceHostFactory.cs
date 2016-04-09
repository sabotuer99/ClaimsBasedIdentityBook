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

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace TokenReplayCacheService
{
    /// <summary>
    /// A Custom ServiceHostFactory that creates a Configured ServiceHost.
    /// </summary>
    public class CustomServiceHostFactory : ServiceHostFactory
    {
        /// <summary>
        /// Overrides the base class method. 
        /// </summary>
        /// <param name="constructorString">Custom parameter specified in the 'Service' attribute of the .svc files.</param>
        /// <param name="baseAddresses">Collection of base address of the service obtained from the hosting layer (IIS).</param>
        /// <returns>Instance of ServiceHostBase.</returns>
        public override ServiceHostBase CreateServiceHost( string constructorString, Uri[] baseAddresses )
        {
            ServiceHostBase host = base.CreateServiceHost( constructorString, baseAddresses );
            FederatedServiceCredentials.ConfigureServiceHost( host );

            return host;
        }
    }
}
