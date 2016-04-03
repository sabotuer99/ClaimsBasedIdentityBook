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

    public enum OrderStatus
    {
        Waiting = 0, 
        Approved, 
        Rejected
    }

    public class Order
    {
        public decimal Amount { get; set; }
        public Customer Customer { get; set; }
        public DateTime Date { get; set; }

        public string Details { get; set; }
        public Guid Id { get; set; }

        public OrderStatus Status { get; set; }
    }
}