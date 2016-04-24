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
    using System.Collections.Generic;
    using System.Linq;
    using Samples.Web.ClaimsUtilities;

    public class OrganizationRepository
    {
        private static readonly ICollection<Organization> OrganizationsStore = InitializeOrganizationStoreAndAddData();

        public Organization GetOrganization(string organizationName)
        {
            var upperOrganizationName = organizationName.ToUpperInvariant();

            return (from o in OrganizationsStore
                    where o.Name.ToUpperInvariant() == upperOrganizationName
                    select o)
                .FirstOrDefault();
        }

        private static ICollection<Organization> InitializeOrganizationStoreAndAddData()
        {
            var organizations = new List<Organization>
                                    {
                                        new Organization
                                            {
                                                Name = Adatum.OrganizationName, 
                                                LogoPath = "~/Content/images/adatum-logo.png"
                                            }, 
                                        new Organization
                                            {
                                                Name = Litware.OrganizationName, 
                                                LogoPath = "~/Content/images/litware-logo.png"
                                            }, 
                                        new Organization
                                            {
                                                Name = Contoso.OrganizationName, 
                                                LogoPath = "~/Content/images/contoso-logo.png"
                                            }
                                    };

            return organizations;
        }
    }
}