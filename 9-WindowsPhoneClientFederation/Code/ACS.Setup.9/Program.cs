//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace ACS.Setup
{
    using System;
    using System.IO;
    using System.Linq;
    using ACS.ServiceManagementWrapper;
    using Microsoft.IdentityModel.Claims;
    using Samples.Web.ClaimsUtilities;

    internal class Program
    {
        private const string AcsPassword = "Your ACS Password";
        private const string AcsServiceNamespace = "Your ACS Namespace";
        private const string AcsUsername = "Your ACS Username";
        private const string LitwareIdentityProvider = "Litware.9";
        private const string LitwareFederationMetaDataLocation = @"..\..\..\Litware.SimulatedIssuer.9\FederationMetadata\2007-06\FederationMetadata.xml";
        private const string AOrderRelyingParty = "aOrderService.9";
        private const string AOrderRealmAddress = "https://localhost/a-Order.OrderTracking.Services.9";
        private const string TokenKey = "TsvBo+Rt3MRtW1gPyhLtnjkFy7jcBJupydJ9hvw40KE=";
        
        private static void CreateIdentityProvider(string issuerName, string fedMetadataFile, ServiceManagementWrapper acsWrapper)
        {
            Console.Write(string.Format("Creating {0} identity provider....", issuerName));
            byte[] fedMetadata = File.ReadAllBytes(fedMetadataFile);
            acsWrapper.AddIdentityProvider(issuerName, fedMetadata);
            Console.WriteLine("done");
        }

        private static void CreateIdentityProviders(ServiceManagementWrapper acsWrapper)
        {
            CreateIdentityProvider(LitwareIdentityProvider, LitwareFederationMetaDataLocation, acsWrapper);
        }

        private static void CreateAOrderRelyingPartyWithRules(ServiceManagementWrapper acsWrapper)
        {
            var ips = new[] { LitwareIdentityProvider };

            Console.Write(string.Format("Creating {0} relying party....", AOrderRelyingParty));
            var ruleGroup = string.Format("Default role group for {0}", AOrderRelyingParty);
            byte[] key = Convert.FromBase64String(TokenKey);
            acsWrapper.AddRelyingPartyWithSymmetricKey(AOrderRelyingParty, AOrderRealmAddress, "https://break_here", key, TokenType.SWT, 30, ruleGroup, ips);
            Console.WriteLine("done");
            CreateAOrderRules(acsWrapper);
        }

        private static void CleanupIdentityProviders(ServiceManagementWrapper acsWrapper)
        {
            Console.Write("Cleaning up identity providers...");
            var ips = acsWrapper.RetrieveIdentityProviders();
            foreach (var ip in ips)
            {
                if (ip.DisplayName != SocialIdentityProviders.WindowsLiveId.DisplayName)
                {
                    acsWrapper.RemoveIssuer(ip.DisplayName);
                }
            }
            Console.WriteLine("done");
        }

        private static void CleanupRelyingParties(ServiceManagementWrapper acsWrapper)
        {
            Console.Write("Cleaning up relying parties...");
            var rps = acsWrapper.RetrieveRelyingParties();
            foreach (var rp in rps)
            {
                if (rp.Name != "AccessControlManagement")
                {
                    acsWrapper.RemoveRelyingParty(rp.Name);
                }
            }
            Console.WriteLine("done");
        }
       
        private static void CreateAOrderRules(ServiceManagementWrapper acsWrapper)
        {
            Console.Write(string.Format("Creating {0} mapping rules....", AOrderRelyingParty));

            var relyingParty = acsWrapper.RetrieveRelyingParties().Single(rp => rp.Name == AOrderRelyingParty);
            var defaultRuleGroup = relyingParty.RelyingPartyRuleGroups.FirstOrDefault();

            // remove rules
            acsWrapper.RemoveAllRulesInGroup(defaultRuleGroup.RuleGroup.Name);

            // add cost center
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                LitwareIdentityProvider,
                Litware.ClaimTypes.CostCenter,
                Litware.ClaimTypes.CostCenter);

            // country
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                LitwareIdentityProvider,
                ClaimTypes.Country,
                ClaimTypes.Country);

            // given name
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                LitwareIdentityProvider,
                ClaimTypes.GivenName,
                ClaimTypes.GivenName);

            // pass name
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, LitwareIdentityProvider, ClaimTypes.Name);

            // add organization
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name,
                LitwareIdentityProvider,
                Adatum.ClaimTypes.Organization,
                "Litware");

            // add role
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                LitwareIdentityProvider,
                AllOrganizations.ClaimTypes.Group,
                ClaimTypes.Role);

            // state or province
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                LitwareIdentityProvider,
                ClaimTypes.StateOrProvince,
                ClaimTypes.StateOrProvince);

            // street address
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                LitwareIdentityProvider,
                ClaimTypes.StreetAddress,
                ClaimTypes.StreetAddress);

            // surname
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                LitwareIdentityProvider,
                ClaimTypes.Surname,
                ClaimTypes.Surname);

            Console.WriteLine("done.");
        }

        private static void Main()
        {
            var acs = new ServiceManagementWrapper(AcsServiceNamespace, AcsUsername, AcsPassword);

            Console.WriteLine("Setting up ACS namespace:" + AcsServiceNamespace);
            CleanupIdentityProviders(acs);
            CleanupRelyingParties(acs);
            CreateIdentityProviders(acs);
            CreateAOrderRelyingPartyWithRules(acs);

            Console.WriteLine("Setup complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}