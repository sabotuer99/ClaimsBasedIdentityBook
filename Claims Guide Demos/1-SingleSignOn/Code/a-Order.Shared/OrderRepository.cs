//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.Data
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    public class OrderRepository
    {
        private static readonly ICollection<AuditTrail> AuditTrailsStore = new List<AuditTrail>();
        private static readonly ICollection<Order> OrdersStore = InitializeOrderStoreAndAddData();

        public IEnumerable<string> GetAllOrderStatus()
        {
            return Enum.GetNames(typeof(OrderStatus));
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return OrdersStore;
        }

        public void SaveOrder(Order order)
        {
            OrdersStore.Add(order);
        }

        public void UpdateStatus(Order updatedOrder)
        {
            var order = OrdersStore.Single(o => o.Id == updatedOrder.Id);
            order.Status = updatedOrder.Status;

            var auditTrail = new AuditTrail
                                 {
                                     Id = Guid.NewGuid(), 
                                     Action = string.Format(CultureInfo.InvariantCulture, "Changed to: {0}", order.Status), 
                                     Order = order, 
                                     Principal = Thread.CurrentPrincipal.Identity.Name, 
                                     Timestamp = DateTime.Now
                                 };

            AuditTrailsStore.Add(auditTrail);
        }

        private static ICollection<Order> InitializeOrderStoreAndAddData()
        {
            var ordersStoreList = new List<Order>();

            var litware = new Customer
                              {
                                  Name = "Litware", 
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
                        Id = new Guid("AC28D078-3DDA-49b7-B1C7-7F23A1445F10"), 
                        Details = "2 items with package", 
                        Amount = 10.05m, 
                        Date = new DateTime(2009, 11, 10), 
                        Status = OrderStatus.Waiting, 
                        Customer = litware
                    });

            ordersStoreList.Add(
                new Order
                    {
                        Id = new Guid("BAFC7E3B-27B6-49a0-AC16-CC5C9E13E52E"), 
                        Details = "7 items without package", 
                        Amount = 38.25m, 
                        Date = new DateTime(2009, 11, 05), 
                        Status = OrderStatus.Waiting, 
                        Customer = litware
                    });

            ordersStoreList.Add(
                new Order
                    {
                        Id = new Guid("6ACAEEE7-01FE-4e8f-BB68-0A26A95D6C57"), 
                        Details = "5 flashy items full version", 
                        Amount = 60.00m, 
                        Date = new DateTime(2009, 10, 07), 
                        Status = OrderStatus.Waiting, 
                        Customer = contoso
                    });

            return ordersStoreList;
        }
    }
}