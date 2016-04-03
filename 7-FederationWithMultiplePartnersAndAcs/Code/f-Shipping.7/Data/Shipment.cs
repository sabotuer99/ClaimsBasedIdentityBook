//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping.Data
{
    using System;

    public enum ShipmentStatus
    {
        NotStarted = 0, 
        InTransit, 
        Delivered, 
        Canceled
    }

    public enum ShipmentServiceType
    {
        Overnight = 0, 
        TwoDays, 
        FiveDays
    }

    public class Shipment
    {
        public string Details { get; set; }

        public DateTime EstimatedDateOfArrival
        {
            get
            {
                switch (this.ServiceType)
                {
                    case ShipmentServiceType.Overnight:
                        return this.PickupDate.AddDays(1);
                    case ShipmentServiceType.TwoDays:
                        return this.PickupDate.AddDays(2);
                    default:
                        return this.PickupDate.AddDays(5);
                }
            }
        }

        public decimal Fee { get; set; }
        public Guid Id { get; set; }

        public string Organization { get; set; }
        public DateTime PickupDate { get; set; }
        public User Sender { get; set; }

        public ShipmentServiceType ServiceType { get; set; }
        public ShipmentStatus Status { get; set; }
        public string ToAddress { get; set; }

        public void CalculateFee()
        {
            this.Fee = Math.Round(Convert.ToDecimal((new Random()).NextDouble()*100), 2);
        }
    }
}