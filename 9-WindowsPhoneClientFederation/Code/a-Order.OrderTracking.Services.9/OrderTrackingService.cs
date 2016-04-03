//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.Services
{
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.Web;
    using System.Xml.Linq;
    using AOrder.OrderTracking.Contracts;
    using AOrder.OrderTracking.Contracts.Data;
    using Samples.Web.ClaimsUtilities;

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class OrderTrackingService : IOrderTrackingService
    {
        public Order[] GetAllOrders()
        {
            var repository = new OrderRepository();
            return repository.GetAllOrders().ToArray();
        }

        public Order[] GetOrdersFromMyOrganization()
        {
            string organization = ClaimHelper.GetClaimsFromPrincipal(HttpContext.Current.User, Adatum.ClaimTypes.Organization).Value;

            var repository = new OrderRepository();
            return repository.GetOrdersByCompanyName(organization).ToArray();
        }

        public void Token(XElement token)
        {
        }
    }
}