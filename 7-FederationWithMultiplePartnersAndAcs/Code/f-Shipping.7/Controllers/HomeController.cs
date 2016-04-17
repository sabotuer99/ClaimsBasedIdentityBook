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
    using System.Web.Mvc;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.Web;
    using System.Collections.Generic;
    using FShipping.Security;
    using FShipping.Data;
    using System.Configuration;

    public class HomeController : Controller
    {

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

        public ActionResult Logout()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);

                string issuer = FederatedAuthentication.WSFederationAuthenticationModule.Issuer;
                var signOut = new SignOutRequestMessage(new Uri(issuer));


                var redirectUrl = signOut.WriteQueryString();

                return this.Redirect(redirectUrl);
            }

            return this.RedirectToAction("Index");
        }

        private static string GetReturnUrlFromCtx()
        {
            // this is the same as doing HttpContext.Request.Form["wctx"];
            var response = FederatedAuthentication.WSFederationAuthenticationModule.GetSignInResponseMessage(System.Web.HttpContext.Current.Request);
            return response.Context;
        }
    }
}