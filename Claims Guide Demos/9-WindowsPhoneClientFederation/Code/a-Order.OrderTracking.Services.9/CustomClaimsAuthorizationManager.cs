//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.Services
{
    using System;
    using System.Linq;
    using System.Net;
    using System.ServiceModel.Web;
    using Microsoft.IdentityModel.Claims;

    public class CustomClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            Claim actionClaim = context.Action.Where(x => x.ClaimType == ClaimTypes.Name).FirstOrDefault();
            Claim resourceClaim = context.Resource.Where(x => x.ClaimType == ClaimTypes.Name).FirstOrDefault();
            IClaimsPrincipal principal = context.Principal;

            var resource = new Uri(resourceClaim.Value);
            string action = actionClaim.Value;

            if (action == "GET" && resource.PathAndQuery.Contains("/frommyorganization"))
            {
                if (!principal.IsInRole("Sales"))
                {
                    return false;
                }
            }

            if (action == "GET" && resource.PathAndQuery.Contains("/all"))
            {
                if (!principal.IsInRole("Sales Manager"))
                {
                    return false;
                }
            }

            return true;
        }
    }
}