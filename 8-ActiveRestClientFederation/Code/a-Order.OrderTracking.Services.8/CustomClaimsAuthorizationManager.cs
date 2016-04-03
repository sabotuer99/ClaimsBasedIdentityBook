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
    using Microsoft.IdentityModel.Claims;
    using Samples.Web.ClaimsUtilities;

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
                if (!principal.IsInRole(Adatum.Roles.OrderTracker))
                {
                    return false;
                }
            }

            if (action == "GET" && resource.PathAndQuery.Contains("/all"))
            {
                if (!principal.IsInRole(Adatum.Roles.OrderApprover))
                {
                    return false;
                }
            }

            return true;
        }
    }
}