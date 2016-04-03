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

    public class HomeController : Controller
    {
        public ActionResult FederationMetadataSample()
        {
            return this.View();
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult FederationResult(string wresult)
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;
            if (fam.CanReadSignInResponse(System.Web.HttpContext.Current.Request, true))
            {
                string returnUrl = GetReturnUrlFromCtx();

                return new RedirectResult(returnUrl);
            }

            return this.View("Index");
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult FederationResult()
        {
            return this.RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult JoinNow()
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