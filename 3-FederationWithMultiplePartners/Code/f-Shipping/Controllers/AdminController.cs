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
    using System.Globalization;
    using System.IdentityModel.Claims;
    using System.Web.Mvc;
    using FShipping.Data;
    using FShipping.Models;
    using FShipping.Security;
    using Samples.Web.ClaimsUtilities;

    [AuthenticateAndAuthorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        public ActionResult AddClaimMapping()
        {
            // TODO: sanitize input and add AntiForgeryToken
            var incomingClaimType = this.Request.Form["IncomingClaimType"];
            var incomingValue = this.Request.Form["IncomingValue"];
            var roleName = this.Request.Form["NewRole"];
            var organization = ClaimHelper.GetCurrentUserClaim(Fabrikam.ClaimTypes.Organization).Value;

            var claimMappingsRepository = new ClaimMappingRepository();
            var role = claimMappingsRepository.GetRoleByName(roleName);

            if (this.ValidateClaimMapping(incomingClaimType, incomingValue, role, organization, claimMappingsRepository))
            {
                claimMappingsRepository.SaveClaimMapping(
                    new ClaimMapping
                        {
                            IncomingClaimType = incomingClaimType, 
                            IncomingValue = incomingValue, 
                            OutputRole = role, 
                            Organization = organization
                        });
            }

            return this.ClaimMappings();
        }

        public ActionResult ClaimMappings()
        {
            var claimMappingsRepository = new ClaimMappingRepository();
            var organization = ClaimHelper.GetCurrentUserClaim(Fabrikam.ClaimTypes.Organization).Value;
            var model = new ClaimMappingListViewModel
                            {
                                ClaimMappings = claimMappingsRepository.GetClaimMappingsByOrganization(organization), 
                                OutputRoles = claimMappingsRepository.GetAllRoles(), 
                                IncomingClaimType = new[] { ClaimTypes.Name, AllOrganizations.ClaimTypes.Group }
                            };

            return this.View("ClaimMappings", model);
        }

        private bool ValidateClaimMapping(string incomingClaimType, string incomingValue, Role role, string organization, ClaimMappingRepository repository)
        {
            bool valid = true;

            if (string.IsNullOrEmpty(incomingClaimType))
            {
                this.ViewData.ModelState.AddModelError("IncomingClaimType", @"The Incoming Claim Type field is required.");
                valid = false;
            }

            if (string.IsNullOrEmpty(incomingValue))
            {
                this.ViewData.ModelState.AddModelError("IncomingValue", @"The Incoming Value field is required.");
                this.ModelState.SetModelValue("IncomingValue", 
                                              new ValueProviderResult(this.ValueProvider.GetValue("IncomingValue") != null ? this.ValueProvider.GetValue("IncomingValue").AttemptedValue : string.Empty, string.Empty, CultureInfo.CurrentCulture));
                valid = false;
            }

            if (role == null)
            {
                this.ViewData.ModelState.AddModelError("RoleRequired", @"The OutputRole field is required.");
                valid = false;
            }
            else
            {
                var existingMapping = repository.GetClaimMapping(incomingClaimType, incomingValue, role, organization);
                if (existingMapping != null)
                {
                    this.ViewData.ModelState.AddModelError("DuplicatedMapping", @"A Claim Mapping already exists for this parameters.");
                    valid = false;
                }
            }

            return valid;
        }
    }
}