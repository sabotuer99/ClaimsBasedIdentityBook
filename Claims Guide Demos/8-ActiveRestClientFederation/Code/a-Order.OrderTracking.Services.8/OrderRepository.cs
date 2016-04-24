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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AOrder.OrderTracking.Contracts.Data;
    using Samples.Web.ClaimsUtilities;

    public class OrderRepository
    {
        private static readonly ICollection<Order> OrdersStore = InitializeOrderStoreAndAddData();

        public IEnumerable<Order> GetAllOrders()
        {
            return OrdersStore;
        }

        public IEnumerable<Order> GetOrdersByCompanyName(string company)
        {
            return OrdersStore.Where(o => o.Customer.Name == company);
        }

        private static ICollection<Order> InitializeOrderStoreAndAddData()
        {
            var ordersStoreList = new List<Order>();

            var litware = new Customer
                              {
                                  Name = Litware.OrganizationName, 
                                  Phone = "01234 1111"
                              };

            var contoso = new Customer
                              {
                                  Name = "Contoso", 
                                  Phone = "01234 2222"
                              };

            ordersStoreList.Add(
                new Order
                    {
                        Id = Guid.NewGuid(), 
                        Details = "2 items with package", 
                        Amount = 10.05m, 
                        Date = new DateTime(2009, 11, 10), 
                        Status = OrderStatus.Waiting, 
                        Customer = litware
                    });

            ordersStoreList.Add(
                new Order
                    {
                        Id = Guid.NewGuid(), 
                        Details = "7 items without package", 
                        Amount = 38.25m, 
                        Date = new DateTime(2009, 11, 05), 
                        Status = OrderStatus.Waiting, 
                        Customer = litware
                    });

            ordersStoreList.Add(
                new Order
                    {
                        Id = Guid.NewGuid(), 
                        Details = "5 flashy items full version", 
                        Amount = 60.00m, 
                        Date = new DateTime(2009, 10, 07), 
                        Status = OrderStatus.Waiting, 
                        Customer = contoso
                    });
            ordersStoreList.Add(
                new Order
                    {
                        Id = Guid.NewGuid(), 
                        Details = "4 premium items", 
                        Amount = 60.00m, 
                        Date = new DateTime(2009, 11, 01), 
                        Status = OrderStatus.Rejected, 
                        Customer = litware
                    });
            ordersStoreList.Add(
                new Order
                    {
                        Id = Guid.NewGuid(), 
                        Details = "20 regular items", 
                        Amount = 60.00m, 
                        Date = new DateTime(2009, 11, 05), 
                        Status = OrderStatus.Approved, 
                        Customer = litware
                    });

            return ordersStoreList;
        }
    }
}