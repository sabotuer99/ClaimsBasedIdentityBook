//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.Contracts.Data
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public enum OrderStatus
    {
        Waiting = 0, 
        Approved, 
        Rejected
    }

    [DataContract]
    public class Order
    {
        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public Customer Customer { get; set; }

        [DataMember]
        public DateTime Date { get; set; }

        [DataMember]
        public string Details { get; set; }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public OrderStatus Status { get; set; }
    }
}