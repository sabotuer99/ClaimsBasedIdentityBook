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
    using System.Configuration;
    using System.Web.Mvc;
    using ACS.ServiceManagementWrapper;
    using FShipping.Data;
    using FShipping.Security;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.Web;

    public class HomeController : Controller
    {
        private readonly string acsServiceNamespace;
        private readonly string acsUsername;
        private readonly string acsPassword;

        public HomeController()
        {
            this.acsServiceNamespace = ConfigurationManager.AppSettings["acs_servicenamespace"];
            this.acsUsername = ConfigurationManager.AppSettings["acs_username"];
            this.acsPassword = ConfigurationManager.AppSettings["acs_password"];
        }

        public ActionResult FederationMetadataSample()
        {
            return this.View();
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FederationResult()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            if (fam.CanReadSignInResponse(System.Web.HttpContext.Current.Request, true))
            {
                string returnUrl = GetReturnUrlFromCtx();

                return new RedirectResult(returnUrl);
            }

            return this.View("Index");
        }

        public ActionResult Index()
        {
            OrganizationRepository organizationRepository = new OrganizationRepository();
            return this.View(organizationRepository.GetOrganizations());
        }

        public ActionResult JoinNow()
        {

            ServiceManagementWrapper acs = new ServiceManagementWrapper(acsServiceNamespace, acsUsername, acsPassword);

            var ips = acs.RetrieveIdentityProviders();

            var listItems = new List<SelectListItem>();

            foreach (var ip in ips)
            {
                if (ip.IsSocial())
                {
                    listItems.Add(new SelectListItem { Text = ip.LoginLinkName, Value = ip.DisplayName.Split(' ')[0]});
                }
            }

            return this.View(listItems);
        }

        [AuthenticateAndAuthorize()]
        public ActionResult JoinWithSocialIdentity()
        {
            
           
            return this.View();
        }

        public ActionResult Logout()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);

                string issuer = FederatedAuthentication.WSFederationAuthenticationModule.Issuer;
                var signOut = new SignOutRequestMessage(new Uri(issuer));
                return this.Redirect(signOut.WriteQueryString());
            }

            return this.RedirectToAction("JoinNow");
        }

        private static string GetReturnUrlFromCtx()
        {
            // this is the same as doing HttpContext.Request.Form["wctx"];
            var response = FederatedAuthentication.WSFederationAuthenticationModule.GetSignInResponseMessage(System.Web.HttpContext.Current.Request);
            return response.Context;
        }
    }
}