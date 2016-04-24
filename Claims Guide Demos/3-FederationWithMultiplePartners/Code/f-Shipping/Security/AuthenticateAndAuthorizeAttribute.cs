//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping.Security
{
    using System;
    using System.Configuration;
    using System.Globalization;
    using System.Text;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FShipping.Data;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.Web;
    using Samples.Web.ClaimsUtilities;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class AuthenticateAndAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public string Roles { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsSecureConnection)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, "https is required to browse the page: '{0}'.", filterContext.HttpContext.Request.Url.AbsoluteUri));
            }

            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                AuthenticateUser(filterContext);
            }
            else
            {
                this.AuthorizeUser(filterContext);
            }
        }

        private static void AuthenticateUser(AuthorizationContext context)
        {
            // TODO: validate/sanitize querystring input (http://msdn.microsoft.com/en-us/library/bb355989.aspx)
            var organizationName = (string)context.RouteData.Values["organization"];

            if (!string.IsNullOrEmpty(organizationName))
            {
                if (!IsValidTenant(organizationName))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, "'{0}' is not a valid tenant.", organizationName));
                }

                var returnUrl = GetReturnUrl(context.RequestContext);

                // user is not authenticated and it's entering for the first time
                var fam = FederatedAuthentication.WSFederationAuthenticationModule;
                var signIn = new SignInRequestMessage(new Uri(fam.Issuer), fam.Realm)
                                 {
                                     Context = returnUrl.ToString(), 
                                     HomeRealm = RetrieveHomeRealmForTenant(organizationName)
                                 };

                context.Result = new RedirectResult(signIn.WriteQueryString());
            }
        }

        private static Uri GetReturnUrl(RequestContext context)
        {
            var request = context.HttpContext.Request;
            var reqUrl = request.Url;
            var wreply = new StringBuilder();

            wreply.Append(reqUrl.Scheme); // e.g. "http"
            wreply.Append("://");
            wreply.Append(request.Headers["Host"] ?? reqUrl.Authority);
            wreply.Append(request.RawUrl);

            if (!request.ApplicationPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                wreply.Append("/");
            }

            return new Uri(wreply.ToString());
        }

        private static bool IsValidTenant(string organizationName)
        {
            var repository = new OrganizationRepository();
            var organization = repository.GetOrganization(organizationName);

            return organization != null && organization.Name.ToUpperInvariant() == organizationName.ToUpperInvariant();
        }

        private static string RetrieveHomeRealmForTenant(string tenantName)
        {
            string tenantHomeRealm = ConfigurationManager.AppSettings[tenantName];
            if (string.IsNullOrEmpty(tenantHomeRealm))
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentUICulture, "No home realm is defined on .config file for tenant '{0}'.", tenantName));
            }

            return tenantHomeRealm;
        }

        private void AuthorizeUser(AuthorizationContext context)
        {
            var organizationRequested = (string)context.RouteData.Values["organization"];
            var userOrganiation = ClaimHelper.GetCurrentUserClaim(Fabrikam.ClaimTypes.Organization).Value;
            if (!organizationRequested.Equals(userOrganiation, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new HttpUnauthorizedResult();
                return;
            }

            var authorizedRoles = this.Roles.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            bool hasValidRole = false;
            foreach (var role in authorizedRoles)
            {
                if (context.HttpContext.User.IsInRole(role.Trim()))
                {
                    hasValidRole = true;
                    break;
                }
            }

            if (!hasValidRole)
            {
                context.Result = new HttpUnauthorizedResult();
                return;
            }
        }
    }
}