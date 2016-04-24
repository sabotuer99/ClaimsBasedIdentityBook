//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.Client.OrderTrackingService
{
    using System.ServiceModel;
    using AOrder.OrderTracking.Contracts;
    using AOrder.OrderTracking.Contracts.Data;

    public class OrderTrackingServiceClient : ClientBase<IOrderTrackingService>, IOrderTrackingService
    {
        private readonly string acsNamespace;
        private readonly string acsRelyingParty;
        private readonly string stsEndpoint;

        public OrderTrackingServiceClient(string acsNamespace, string acsRelyingParty, string stsEndpoint)
        {
            this.acsNamespace = acsNamespace;
            this.acsRelyingParty = acsRelyingParty;
            this.stsEndpoint = stsEndpoint;
        }

        public Order[] GetAllOrders()
        {
            return this.Channel.GetAllOrders();
        }

        public Order[] GetOrdersFromMyOrganization()
        {
            return this.Channel.GetOrdersFromMyOrganization();
        }

        protected override IOrderTrackingService CreateChannel()
        {
            this.ChannelFactory.Endpoint.Behaviors.Add(new CustomHeaderBehavior(this.ClientCredentials, this.acsNamespace, this.acsRelyingParty, this.stsEndpoint));
            return base.CreateChannel();
        }
    }
}
