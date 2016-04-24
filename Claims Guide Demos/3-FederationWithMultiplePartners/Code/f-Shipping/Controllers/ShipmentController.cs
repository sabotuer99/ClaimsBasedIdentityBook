//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using FShipping.Data;
    using FShipping.Models;
    using FShipping.Security;
    using Microsoft.IdentityModel.Claims;
    using Samples.Web.ClaimsUtilities;

    // by adding this attribute we are enabling access to all 
    // the Shipment actions only to users that belong to the Shipment Creator role
    [AuthenticateAndAuthorize(Roles = "Shipment Creator")]
    public class ShipmentController : BaseController
    {
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add()
        {
            // TODO: sanitize input and add AntiForgeryToken
            var userRepository = new UserRepository();

            User user = userRepository.GetUser(this.User.Identity.Name);
            this.ViewData.Add("UserFound", user != null);

            if (user == null)
            {
                return this.View("New");
            }
            EnrichUserWithClaims(user);
            var shipment = new Shipment
                               {
                                   Id = Guid.NewGuid(), 
                                   ToAddress = this.Request.Form["ShippingToAddress"], 
                                   PickupDate = DateTime.Parse(this.Request.Form["PickupDate"], CultureInfo.CurrentUICulture), 
                                   ServiceType = (ShipmentServiceType)Enum.Parse(typeof(ShipmentServiceType), this.Request.Form["ServiceType"]), 
                                   Details = this.Request.Form["Details"], 
                                   Organization = user.Organization, 
                                   Status = ShipmentStatus.NotStarted, 
                                   Sender = user
                               };

            shipment.CalculateFee();

            var repository = new ShipmentRepository();
            repository.SaveShipment(shipment);

            user.PreferredShippingServiceType = shipment.ServiceType;
            userRepository.UpdatePreferredShippingServiceType(user);

            return this.RedirectToAction("Index");
        }

        [AuthenticateAndAuthorize(Roles = "Shipment Manager")]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Cancel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidOperationException("The value of id can not be null or empty.");
            }

            Guid shipmentId;
            try
            {
                shipmentId = new Guid(id);
            }
            catch (FormatException)
            {
                throw new InvalidOperationException("The value of id is incorrect.");
            }

            var repository = new ShipmentRepository();
            repository.DeleteShipment(shipmentId);

            return this.RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            var repository = new ShipmentRepository();

            // users in Shipment Manager role can see all the shipments, 
            // while others can see only its own shipments
            IEnumerable<Shipment> shipments;
            var organization = ClaimHelper.GetCurrentUserClaim(Fabrikam.ClaimTypes.Organization).Value;
            if (this.User.IsInRole(Fabrikam.Roles.ShipmentManager))
            {
                shipments = repository.GetShipmentsByOrganization(organization);
            }
            else
            {
                var userName = this.User.Identity.Name;
                shipments = repository.GetShipmentsByOrganizationAndUserName(organization, userName);
            }

            var model = new ShipmentListViewModel
                            {
                                Shipments = shipments
                            };

            return View(model);
        }

        public ActionResult New()
        {
            var userRepository = new UserRepository();

            // obtain the User from the repository that contains
            // application specific data like PreferredShippingServiceType
            User user = userRepository.GetUser(this.User.Identity.Name);
            this.ViewData.Add("UserFound", user != null);

            if (user != null)
            {
                // enrich User with claims from ClaimsIdentity
                EnrichUserWithClaims(user);

                this.ViewData.Add("SenderName", user.FullName);
                this.ViewData.Add("SenderAddress", user.Address);
                this.ViewData.Add("SenderCostCenter", user.CostCenter);
                var serviceTypeListItems = Enum.GetNames(typeof(ShipmentServiceType)).Select(n => new SelectListItem { Selected = Enum.GetName(typeof(ShipmentServiceType), user.PreferredShippingServiceType) == n, Text = n, Value = n });
                this.ViewData.Add("ServiceTypeListItems", serviceTypeListItems);
            }

            return this.View();
        }

        private static void EnrichUserWithClaims(User user)
        {
            user.CostCenter = ClaimHelper.GetCurrentUserClaim(Fabrikam.ClaimTypes.CostCenter).Value ?? string.Empty;
            user.Organization = ClaimHelper.GetCurrentUserClaim(Fabrikam.ClaimTypes.Organization).Value ?? string.Empty;

            string givenName = ClaimHelper.GetCurrentUserClaim(ClaimTypes.GivenName).Value ?? string.Empty;
            string surname = ClaimHelper.GetCurrentUserClaim(ClaimTypes.Surname).Value ?? string.Empty;
            user.FullName = string.Format(CultureInfo.CurrentUICulture, "{0} {1}", givenName, surname);

            string streetAddress = ClaimHelper.GetCurrentUserClaim(ClaimTypes.StreetAddress).Value ?? string.Empty;
            string state = ClaimHelper.GetCurrentUserClaim(ClaimTypes.StateOrProvince).Value ?? string.Empty;
            string country = ClaimHelper.GetCurrentUserClaim(ClaimTypes.Country).Value ?? string.Empty;
            user.Address = string.Format(CultureInfo.CurrentUICulture, "{0}, {1}, {2}", streetAddress, state, country);
        }
    }
}