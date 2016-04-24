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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Samples.Web.ClaimsUtilities;

    public class ShipmentRepository
    {
        private static readonly ICollection<Shipment> ShipmentStore = InitializeShipmentStoreAndAddData();

        public void DeleteShipment(Guid shipmentId)
        {
            var shipmentToRemove = ShipmentStore.SingleOrDefault(s => s.Id == shipmentId);
            if (shipmentToRemove != null)
            {
                ShipmentStore.Remove(shipmentToRemove);
            }
        }

        public IEnumerable<Shipment> GetShipmentsByOrganization(string organization)
        {
            return ShipmentStore.Where(shipment => string.Compare(shipment.Organization, organization, CultureInfo.InvariantCulture, CompareOptions.OrdinalIgnoreCase) == 0);
        }

        public IEnumerable<Shipment> GetShipmentsByOrganizationAndUserName(string organization, string userName)
        {
            return ShipmentStore.Where(shipment => shipment.Sender.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) &&
                                                   shipment.Organization.Equals(organization, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveShipment(Shipment shipment)
        {
            ShipmentStore.Add(shipment);
        }

        private static ICollection<Shipment> InitializeShipmentStoreAndAddData()
        {
            var shipments = new List<Shipment>
                                {
                                    new Shipment
                                        {
                                            Id = new Guid("9925BAAB-6579-4f55-892C-97F19F9B2BAB"),
                                            Sender = UserRepository.JohnDoe,
                                            ToAddress = "123 Park Av., Seattle, WA",
                                            PickupDate = new DateTime(2009, 11, 5),
                                            Details = "3 CDs with the latest release candidate.",
                                            Fee = 10.00m,
                                            Organization = Adatum.OrganizationName,
                                            Status = ShipmentStatus.NotStarted,
                                            ServiceType = ShipmentServiceType.FiveDays
                                        },
                                    new Shipment
                                        {
                                            Id = new Guid("398D0464-05CF-4e25-B623-0907549A11D1"),
                                            Sender = UserRepository.JohnDoe,
                                            ToAddress = "5 Island Rd., Seattle, WA",
                                            PickupDate = new DateTime(2009, 11, 1),
                                            Details = "Product manual v1.1.",
                                            Fee = 5.60m,
                                            Organization = Adatum.OrganizationName,
                                            Status = ShipmentStatus.InTransit,
                                            ServiceType = ShipmentServiceType.Overnight
                                        },
                                    new Shipment
                                        {
                                            Id = new Guid("1DC4F084-7E0C-47bb-8C07-B5E1C1F7EA46"),
                                            Sender = UserRepository.PeterPorter,
                                            ToAddress = "81 Lane Av., Las Vegas, NV",
                                            PickupDate = new DateTime(2009, 8, 29),
                                            Details = "Hardcover just-released book.",
                                            Fee = 25.80m,
                                            Organization = Adatum.OrganizationName,
                                            Status = ShipmentStatus.InTransit,
                                            ServiceType = ShipmentServiceType.FiveDays
                                        },
                                    new Shipment
                                        {
                                            Id = new Guid("A6AFF819-9A73-49b3-9EC3-66E328A84F07"),
                                            Sender = UserRepository.BillAtContoso,
                                            ToAddress = "12 Bottom St., Los Angeles, CA",
                                            PickupDate = new DateTime(2009, 10, 13),
                                            Details = "10 fresh flowers.",
                                            Fee = 3.20m,
                                            Organization = Contoso.OrganizationName,
                                            Status = ShipmentStatus.NotStarted,
                                            ServiceType = ShipmentServiceType.FiveDays
                                        }
                                };

            return shipments;
        }
    }
}