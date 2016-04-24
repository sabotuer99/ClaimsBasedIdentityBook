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
    using System.IdentityModel.Claims;
    using System.Web.Mvc;
    using FShipping.Data;
    using FShipping.Models;
    using Samples.Web.ClaimsUtilities;

    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }

            if (filterContext.Controller.ViewData.Model == null)
            {
                filterContext.Controller.ViewData.Model = new MasterPageViewModel();
            }

            var model = (MasterPageViewModel)filterContext.Controller.ViewData.Model;

            var organizationName = ClaimHelper.GetCurrentUserClaim(Fabrikam.ClaimTypes.Organization).Value;
            var organizationRepository = new OrganizationRepository();
            var tenantLogoPath = organizationRepository.GetOrganization(organizationName).LogoPath;
            model.TenantLogoPath = tenantLogoPath;

            model.ClaimsIssuer =
                ClaimHelper.GetCurrentUserClaim(ClaimTypes.Name).
                    Issuer;
            model.ClaimsOriginalIssuer =
                ClaimHelper.GetCurrentUserClaim(ClaimTypes.Name).
                    OriginalIssuer;

            base.OnActionExecuted(filterContext);
        }
    }
}