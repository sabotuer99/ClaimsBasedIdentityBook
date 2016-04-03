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
    using AOrder.OrderTracking.Contracts;
    using AOrder.OrderTracking.Contracts.Data;
    using Samples.Web.ClaimsUtilities;

    public class OrderTrackingService : IOrderTrackingService
    {
        public Order[] GetAllOrders()
        {
            var repository = new OrderRepository();
            return repository.GetAllOrders().ToArray();
        }

        public Order[] GetOrdersFromMyOrganization()
        {
            string organization = ClaimHelper.GetCurrentUserClaim(Adatum.ClaimTypes.Organization).Value;

            var repository = new OrderRepository();
            return repository.GetOrdersByCompanyName(organization).ToArray();
        }
    }
}