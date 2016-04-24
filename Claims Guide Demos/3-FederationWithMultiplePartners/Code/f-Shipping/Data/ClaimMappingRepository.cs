//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping.Data
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Claims;
    using System.Linq;
    using Samples.Web.ClaimsUtilities;

    public class ClaimMappingRepository
    {
        private static readonly ICollection<ClaimMapping> ClaimMappingsStore = InitializeClaimMappingStoreAndAddData();
        private static Role administratorRole;

        private static Role shipmentCreatorRole;

        private static Role shipmentManagerRole;

        public IEnumerable<Role> GetAllRoles()
        {
            return new[] { shipmentCreatorRole, shipmentManagerRole, administratorRole };
        }

        public ClaimMapping GetClaimMapping(string incomingClaimType, string incomingValue, Role role, string organization)
        {
            return (from cm in ClaimMappingsStore
                    where
                        cm.IncomingClaimType.Equals(incomingClaimType, StringComparison.OrdinalIgnoreCase) &&
                        cm.IncomingValue.Equals(incomingValue, StringComparison.OrdinalIgnoreCase) &&
                        cm.OutputRole.Name == role.Name &&
                        cm.Organization == organization
                    select cm)
                .FirstOrDefault();
        }

        public IEnumerable<ClaimMapping> GetClaimMappingsByOrganization(string organization)
        {
            return ClaimMappingsStore.Where(cm => cm.Organization == organization);
        }

        public Role GetRoleByName(string roleName)
        {
            return (from cm in ClaimMappingsStore where cm.OutputRole.Name == roleName select cm.OutputRole).FirstOrDefault();
        }

        public void SaveClaimMapping(ClaimMapping claimMapping)
        {
            ClaimMappingsStore.Add(claimMapping);
        }

        private static ICollection<ClaimMapping> InitializeClaimMappingStoreAndAddData()
        {
            shipmentCreatorRole = new Role
                                      {
                                          Name = Fabrikam.Roles.ShipmentCreator, 
                                          Actions = new[] { "Create new shipments" }
                                      };

            shipmentManagerRole = new Role
                                      {
                                          Name = Fabrikam.Roles.ShipmentManager, 
                                          Actions = new[] { "Create new shipments", "Cancel shipments" }
                                      };

            administratorRole = new Role
                                    {
                                        Name = Fabrikam.Roles.Administrator, 
                                        Actions = new[] { "Configure the system" }
                                    };

            var claimMappings = new List<ClaimMapping>
                                    {
                                        new ClaimMapping
                                            {
                                                IncomingClaimType = AllOrganizations.ClaimTypes.Group, 
                                                IncomingValue = Adatum.Groups.CustomerService, 
                                                OutputRole = shipmentCreatorRole, 
                                                Organization = Adatum.OrganizationName
                                            }, 
                                        new ClaimMapping
                                            {
                                                IncomingClaimType = AllOrganizations.ClaimTypes.Group, 
                                                IncomingValue = Adatum.Groups.OrderFulfillments, 
                                                OutputRole = shipmentManagerRole, 
                                                Organization = Adatum.OrganizationName
                                            }, 
                                        new ClaimMapping
                                            {
                                                IncomingClaimType = AllOrganizations.ClaimTypes.Group, 
                                                IncomingValue = Adatum.Groups.ItAdmins, 
                                                OutputRole = administratorRole, 
                                                Organization = Adatum.OrganizationName
                                            }, 
                                        new ClaimMapping
                                            {
                                                IncomingClaimType = AllOrganizations.ClaimTypes.Group, 
                                                IncomingValue = Litware.Groups.Sales, 
                                                OutputRole = shipmentManagerRole, 
                                                Organization = Litware.OrganizationName
                                            }, 
                                        new ClaimMapping
                                            {
                                                IncomingClaimType = ClaimTypes.Name, 
                                                IncomingValue = "bill@contoso.com", 
                                                OutputRole = administratorRole, 
                                                Organization = Contoso.OrganizationName
                                            }, 
                                        new ClaimMapping
                                            {
                                                IncomingClaimType = ClaimTypes.Name, 
                                                IncomingValue = "bill@contoso.com", 
                                                OutputRole = shipmentManagerRole, 
                                                Organization = Contoso.OrganizationName
                                            }
                                    };

            return claimMappings;
        }
    }
}