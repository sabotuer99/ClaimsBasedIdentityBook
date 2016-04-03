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
    using System.Collections.Generic;
    using AOrder.OrderTracking.Contracts.Data;

    public class OrderTrackingViewModel
    {
        public OrderTrackingViewModel(IEnumerable<Order> orders)
        {
            this.Orders = orders;
        }

        public IEnumerable<Order> Orders { get; private set; }
    }
}