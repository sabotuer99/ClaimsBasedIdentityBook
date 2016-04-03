//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration.Claims;
using Microsoft.SharePoint.WebControls;
using Microsoft.IdentityModel.Claims;

namespace SampleClaimsProvider
{
    public class SampleClaimsProvider : SPClaimProvider
    {
        internal static string ProviderDisplayName = "ADFS";
        internal static string ProviderInternalName = "ADFSClaimsProvider";
        private static string TrustedProviderName = "<<Your Trusted Issuer name>>";
        private const string OrganizationKey = "organization";
        private const string RoleKey = "role";
        private static Dictionary<string, string> filters = new Dictionary<string, string> { { RoleKey, "Role" }, { OrganizationKey, "Organization" } };
        private const string OrganizationClaimType = "http://schemas.adatum.com/claims/2009/08/organization";

        public SampleClaimsProvider(string providerDisplayName)
            : base(providerDisplayName)
        {}

        protected override void FillClaimTypes(List<string> claimTypes)
        {
            if (claimTypes == null)
            {
                throw new ArgumentNullException("claimTypes");
            }

            claimTypes.Add(OrganizationClaimType);
            claimTypes.Add(Microsoft.IdentityModel.Claims.ClaimTypes.Role);
        }

        protected override void FillClaimValueTypes(List<string> claimValueTypes)
        {
            if (claimValueTypes == null)
            {
                throw new ArgumentNullException("claimValueTypes");
            }

            claimValueTypes.Add(Microsoft.IdentityModel.Claims.ClaimValueTypes.String);

        }

        protected override void FillClaimsForEntity(Uri context, SPClaim entity, List<SPClaim> claims)
        {
            throw new NotImplementedException();
        }

        protected override void FillEntityTypes(List<string> entityTypes)
        {
            entityTypes.Add(SPClaimEntityTypes.Trusted);
        }

        protected override void FillHierarchy(Uri context, string[] entityTypes, string hierarchyNodeID, int numberOfLevels, Microsoft.SharePoint.WebControls.SPProviderHierarchyTree hierarchy)
        {
            if (!entityTypes.Contains(SPClaimEntityTypes.Trusted))
            {
                return;
            }

            if (hierarchyNodeID == null)
            {
                foreach (var filter in filters)
                {
                    hierarchy.AddChild(new Microsoft.SharePoint.WebControls.SPProviderHierarchyNode(
                        ProviderInternalName,
                        filter.Value,
                        filter.Key,
                        true));
                }
            }
        }

        protected override void FillResolve(Uri context, string[] entityTypes, SPClaim resolveInput, List<Microsoft.SharePoint.WebControls.PickerEntity> resolved)
        {
            this.FillResolve(context, entityTypes, resolveInput.Value, resolved);
        }

        protected override void FillResolve(Uri context, string[] entityTypes, string resolveInput, List<Microsoft.SharePoint.WebControls.PickerEntity> resolved)
        {
            if (!entityTypes.Contains(SPClaimEntityTypes.Trusted))
            {
                return;
            }

            var organizationMatches = SearchOrganizations(resolveInput);
            if (organizationMatches.Count() > 0)
            {
                foreach (var match in organizationMatches)
                {
                    PickerEntity pe = GetPickerEntity(match, OrganizationClaimType, filters[OrganizationKey]);
                    resolved.Add(pe);
                }
            }

            var roleMatches = SearchRoles(resolveInput);
            if (roleMatches.Count() > 0)
            {
                foreach (var match in roleMatches)
                {
                    PickerEntity pe = GetPickerEntity(match, Microsoft.IdentityModel.Claims.ClaimTypes.Role, filters[RoleKey]);
                    resolved.Add(pe);
                }
            }
        }

        protected override void FillSchema(Microsoft.SharePoint.WebControls.SPProviderSchema schema)
        {
            schema.AddSchemaElement(new
                  SPSchemaElement(PeopleEditorEntityDataKeys.DisplayName,
                  "Display Name", SPSchemaElementType.Both));
        }

        protected override void FillSearch(Uri context, string[] entityTypes, string searchPattern, string hierarchyNodeID, int maxCount, Microsoft.SharePoint.WebControls.SPProviderHierarchyTree searchTree)
        {
            if (!entityTypes.Contains(SPClaimEntityTypes.Trusted))
            {
                return;
            }

            var organizationMatches = SearchOrganizations(searchPattern);
            if (organizationMatches.Count() > 0)
            {
                var matchNode = new SPProviderHierarchyNode(
                                        ProviderInternalName,
                                        filters[OrganizationKey],
                                        OrganizationKey,
                                        true);
                searchTree.AddChild(matchNode);

                foreach (var match in organizationMatches)
                {
                    PickerEntity pe = GetPickerEntity(match, OrganizationClaimType, filters[OrganizationKey]);
                    matchNode.AddEntity(pe);
                }
            }

            var roleMatches = SearchRoles(searchPattern);
            if (roleMatches.Count() > 0)
            {
                var matchNode = new SPProviderHierarchyNode(
                                        ProviderInternalName,
                                        filters[RoleKey],
                                        RoleKey,
                                        true);
                searchTree.AddChild(matchNode);

                foreach (var match in roleMatches)
                {
                    PickerEntity pe = GetPickerEntity(match, Microsoft.IdentityModel.Claims.ClaimTypes.Role, filters[RoleKey]);
                    matchNode.AddEntity(pe);
                }
            }
        }

        private PickerEntity GetPickerEntity(string ClaimValue, string claimType, string GroupName)
        {
            PickerEntity pe = CreatePickerEntity();
            var issuer = SPOriginalIssuers.Format(SPOriginalIssuerType.TrustedProvider, TrustedProviderName);
            pe.Claim = new SPClaim(claimType, ClaimValue, Microsoft.IdentityModel.Claims.ClaimValueTypes.String, issuer);
            pe.Description = claimType + "/" + ClaimValue;
            pe.DisplayText = ClaimValue;
            pe.EntityData[PeopleEditorEntityDataKeys.DisplayName] = ClaimValue;
            pe.EntityType = SPClaimEntityTypes.Trusted;
            pe.IsResolved = true;
            pe.EntityGroupName = GroupName;

            return pe;
        }

        public override string Name
        {
            get { return ProviderInternalName; }
        }

        public override bool SupportsEntityInformation
        {
            get { return false; }
        }

        public override bool SupportsHierarchy
        {
            get { return true; }
        }

        public override bool SupportsResolve
        {
            get { return true; }
        }

        public override bool SupportsSearch
        {
            get { return true; }
        }

        private static string[] organizations = { "Adatum", "Litware" };
        private static IEnumerable<string> SearchOrganizations(string searchPattern)
        {
            return organizations.Where(o => o.StartsWith(searchPattern, StringComparison.InvariantCultureIgnoreCase));
        }

        private static string[] roles = { "Administrator", "Accountant", "Sales Manager" };
        private static IEnumerable<string> SearchRoles(string searchPattern)
        {
            return roles.Where(o => o.StartsWith(searchPattern, StringComparison.InvariantCultureIgnoreCase));
        }

    }
}
