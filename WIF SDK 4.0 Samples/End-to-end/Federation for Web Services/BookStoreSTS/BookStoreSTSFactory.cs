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

using Microsoft.IdentityModel.Protocols.WSTrust;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    /// <summary>
    /// Creates service instance to handle incoming request.
    /// </summary>
    public class BookStoreSTSFactory : WSTrustServiceHostFactory
    {
        /// <summary>
        /// Overrides the base class method. Configures the appropriate Issuer token validation semantics on
        /// the ServiceHost.
        /// </summary>
        /// <param name="constructorString">Custom parameter specified in the 'Service' attribute of the .svc files.</param>
        /// <param name="baseAddresses">Collection of base address of the service obtained from the hosting layer (IIS).</param>
        /// <returns>Instance of ServiceHostBase.</returns>
        public override ServiceHostBase CreateServiceHost( string constructorString, Uri[] baseAddresses )
        {
            ServiceHostBase serviceHost = base.CreateServiceHost( constructorString, baseAddresses );
            return serviceHost;
        }
    }
}
