//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.Client
{
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    public class CustomHeaderBehavior : IEndpointBehavior
    {
        private readonly string acsNamespace;
        private readonly string acsRelyingParty;
        private readonly ClientCredentials clientCredentials;
        private readonly string stsEndpoint;

        public CustomHeaderBehavior(ClientCredentials clientCredentials, string acsNamespace, string acsRelyingParty, string stsEndpoint)
        {
            this.clientCredentials = clientCredentials;
            this.acsNamespace = acsNamespace;
            this.acsRelyingParty = acsRelyingParty;
            this.stsEndpoint = stsEndpoint;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var customHeaderMessageInspector = new CustomHeaderMessageInspector(this.clientCredentials, this.acsNamespace, this.acsRelyingParty, this.stsEndpoint);
            clientRuntime.MessageInspectors.Add(customHeaderMessageInspector);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}