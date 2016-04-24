//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Web;
    using Microsoft.IdentityModel.Web.Configuration;

    public class MvcApplication : HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("favicon.ico");
            routes.IgnoreRoute("*.htm");

            routes.MapRoute(
                "Default", 
                string.Empty, 
                new { controller = "Home", action = "Index" });

            routes.MapRoute(
                "JoinNow", 
                "JoinNow", 
                new { controller = "Home", action = "JoinNow" });

            routes.MapRoute(
                "EnrollWithSocialProvider",
                "EnrollWithSocialProvider/{socialip}/",
                new { controller = "Enrollment", action = "EnrollWithSocialProvider" });

            routes.MapRoute(
                "CompleteEnrollment",
                "CompleteEnrollment",
                new { controller = "Enrollment", action = "CompleteEnrollment" });

            routes.MapRoute(
                "EnrollManually",
                "EnrollManually",
                new { controller = "Enrollment", action = "EnrollManually" });

            routes.MapRoute(
                "CreateTenantWithSocialProvider",
                "CreateTenantWithSocialProvider",
                new { controller = "Enrollment", action = "CreateTenantWithSocialProvider" });

            routes.MapRoute(
                "CreateTenantManually",
                "CreateTenantManually",
                new { controller = "Enrollment", action = "CreateTenantManually" });

            routes.MapRoute(
                "EnrollWithFedMetadataFile",
                "EnrollWithFedMetadataFile",
                new { controller = "Enrollment", action = "EnrollWithFedMetadataFile" });

            routes.MapRoute(
                "CreateTenantFromFedMetadaFile",
                "CreateTenantFromFedMetadaFile",
                new { controller = "Enrollment", action = "CreateTenantFromFedMetadaFile" });

            routes.MapRoute(
                "CompletePrivateEnrollment",
                "CompletePrivateEnrollment",
                new { controller = "Enrollment", action = "CompletePrivateEnrollment" });

            routes.MapRoute(
                "FederationResult", 
                "FederationResult", 
                new { controller = "Home", action = "FederationResult" });

            routes.MapRoute(
                "Logout", 
                "home/{action}", 
                new { controller = "Home" });

            //routes.MapRoute(
            //    "NewShipment", 
            //    "{organization}/shipment/new", 
            //    new { controller = "Shipment", action = "New" });

            //routes.MapRoute(
            //    "AddShipment", 
            //    "{organization}/shipment/add", 
            //    new { controller = "Shipment", action = "Add" });

            //routes.MapRoute(
            //    "CancelShipment", 
            //    "{organization}/shipment/{id}/cancel", 
            //    new { controller = "Shipment", action = "Cancel", id = string.Empty });

            //routes.MapRoute(
            //    "ClaimMappings", 
            //    "{organization}/admin/{action}", 
            //    new { controller = "Admin" });

            //routes.MapRoute(
            //    "OrganizationDefault", 
            //    "{organization}/", 
            //    new { controller = "Shipment", action = "Index" });
        }

        protected void Application_Start()
        {
            FederatedAuthentication.ServiceConfigurationCreated += OnServiceConfigurationCreated;

            RegisterRoutes(RouteTable.Routes);
        }


        private static void OnServiceConfigurationCreated(object sender, ServiceConfigurationCreatedEventArgs e)
        {
            // Use the <serviceCertificate> to protect the cookies that are
            // sent to the client.
            var sessionTransforms =
                new List<CookieTransform>(
                    new CookieTransform[]
                        {
                            new DeflateCookieTransform(), 
                            new RsaEncryptionCookieTransform(e.ServiceConfiguration.ServiceCertificate), 
                            new RsaSignatureCookieTransform(e.ServiceConfiguration.ServiceCertificate)
                        });
            var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());

            e.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
        }
    }
}