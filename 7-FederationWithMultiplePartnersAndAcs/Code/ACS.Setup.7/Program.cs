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
    using ACS.ServiceManagementWrapper.ACS.Management;
    using Microsoft.IdentityModel.Claims;
    using Samples.Web.ClaimsUtilities;

    internal class Program
    {
        private const string AcsPassword = "Your ACS Password";
        private const string AcsServiceNamespace = "Your ACS Namespace";
        private const string AcsUsername = "Your ACS Username";
        private const string AdatumIdentityProvider = "Adatum.7";
        private const string LitwareIdentityProvider = "Litware.7";

        public static void CreateEnrollmentRelyingParty(string[] identityProviders, ServiceManagementWrapper acsWrapper)
        {
            Console.Write("Creating f-shipping.Enrollment relying party....");

            var realmAddress = "https://localhost/f-shipping.Enrollment.7";
            var replyAddress = "https://localhost/f-shipping.Enrollment.7/FederationResult";
            var ruleGroup = "Default rule group for f-Shipping.Enrollment.7";
            acsWrapper.AddRelyingParty("f-Shipping.Enrollment.7", realmAddress, replyAddress, null, null, null, ruleGroup, identityProviders);

            Console.WriteLine("done");
        }

        public static void CreateIdentityProvider(string issuerName, string fedMetadataFile, ServiceManagementWrapper acsWrapper)
        {
            Console.Write(string.Format("Creating {0} identity provider....", issuerName));
            byte[] fedMetadata = File.ReadAllBytes(fedMetadataFile);
            acsWrapper.AddIdentityProvider(issuerName, fedMetadata);
            Console.WriteLine("done");
        }

        public static void CreateIdentityProviders(ServiceManagementWrapper acsWrapper)
        {
            acsWrapper.AddGoogleIdentityProvider();
            
            acsWrapper.AddYahooIdentityProvider();

            CreateIdentityProvider(LitwareIdentityProvider, @"..\..\..\Litware.SimulatedIssuer.7\FederationMetadata\2007-06\FederationMetadata.xml", acsWrapper);

            CreateIdentityProvider(AdatumIdentityProvider, @"..\..\..\Adatum.SimulatedIssuer.7\FederationMetadata\2007-06\FederationMetadata.xml", acsWrapper);
        }

        public static void CreateRelyingParty(string relyingPartyName, string[] identityProviders, ServiceManagementWrapper acsWrapper)
        {
            Console.Write(string.Format("Creating {0} relying party....", relyingPartyName));
            var realmAddress = string.Format("https://localhost/f-shipping.7/{0}", relyingPartyName);
            var replyAddress = string.Format("https://localhost/f-shipping.7/{0}/FederationResult", relyingPartyName);
            var ruleGroup = string.Format("Default role group for {0}", relyingPartyName);
            acsWrapper.AddRelyingParty(relyingPartyName, realmAddress, replyAddress, null, null, null, ruleGroup, identityProviders);
            Console.WriteLine("done");
        }

        public static void CreateRelyingPartysWithRules(ServiceManagementWrapper acsWrapper)
        {
            var ips = new[] { AdatumIdentityProvider };

            CreateRelyingParty("Adatum", ips, acsWrapper);
            CreateAdatumRules(acsWrapper);

            ips = new[] { LitwareIdentityProvider };
            CreateRelyingParty("Litware", ips, acsWrapper);
            CreateLitwareRules(acsWrapper);

            ips = new[] { SocialIdentityProviders.WindowsLiveId.DisplayName };
            CreateRelyingParty("Contoso", ips, acsWrapper);
            CreateContosoRules(acsWrapper);

            ips = new[] { SocialIdentityProviders.WindowsLiveId.DisplayName, SocialIdentityProviders.Google.DisplayName, SocialIdentityProviders.Yahoo.DisplayName };
            CreateEnrollmentRelyingParty(ips, acsWrapper);
            CreateEnrollmentRules(acsWrapper);
        }


        private static void CleanupIdenityProviders(ServiceManagementWrapper acsWrapper)
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

        private static void CreateAdatumRules(ServiceManagementWrapper acsWrapper)
        {
            Console.Write("Creating Adatum.7 mapping rules....");

            var relyingParty = acsWrapper.RetrieveRelyingParties().Single(rp => rp.Name == "Adatum");
            var defaultRuleGroup = relyingParty.RelyingPartyRuleGroups.FirstOrDefault();
            var identityProviderName = AdatumIdentityProvider;

            // remove rules
            acsWrapper.RemoveAllRulesInGroup(defaultRuleGroup.RuleGroup.Name);

            // pass name
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, identityProviderName, ClaimTypes.Name);

            // transform organization
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                Adatum.ClaimTypes.Organization, 
                Fabrikam.ClaimTypes.Organization);

            // add cost center
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                Adatum.ClaimTypes.CostCenter, 
                Fabrikam.ClaimTypes.CostCenter);

            // add role
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                ClaimTypes.Role, 
                Fabrikam.Roles.ShipmentCreator);


            // given name
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.GivenName,
                ClaimTypes.GivenName);

            // surname
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.Surname,
                ClaimTypes.Surname);

            // street address
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.StreetAddress,
                ClaimTypes.StreetAddress);


            // state or province
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.StateOrProvince,
                ClaimTypes.StateOrProvince);


            // country
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.Country,
                ClaimTypes.Country);

            Console.WriteLine("done.");
        }

        private static void CreateContosoRules(ServiceManagementWrapper acsWrapper)
        {
            Console.Write("Creating Contoso mapping rules....");

            var identityProviderName = "Windows Live ID";
            var relyingParty = acsWrapper.RetrieveRelyingParties().Single(rp => rp.Name == "Contoso");
            var defaultRuleGroup = relyingParty.RelyingPartyRuleGroups.FirstOrDefault();

            // remove rules
            acsWrapper.RemoveAllRulesInGroup(defaultRuleGroup.RuleGroup.Name);

            // add name
            acsWrapper.AddSimpleRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                ClaimTypes.NameIdentifier, 
                null, 
                ClaimTypes.Name, 
                "rick");

            // add organization
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                Fabrikam.ClaimTypes.Organization, 
                "Contoso");

            // add cost center
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                Fabrikam.ClaimTypes.CostCenter, 
                Contoso.CostCenters.SingleCostCenter);

            // add role
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                ClaimTypes.Role, 
                Fabrikam.Roles.ShipmentCreator);

            // given name
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                ClaimTypes.GivenName, 
                "Rick");

            // surname
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(defaultRuleGroup.RuleGroup.Name, 
                                                                        identityProviderName, 
                                                                        ClaimTypes.Surname, 
                                                                        "Rico");
            Console.WriteLine("done.");
        }

        private static void CreateEnrollmentRules(ServiceManagementWrapper acsWrapper)
        {
            var name = "f-Shipping.Enrollment.7";

            var rp = acsWrapper.RetrieveRelyingParties().Single(r => r.Name == name);
            var defaultRuleGroup = rp.RelyingPartyRuleGroups.FirstOrDefault();

            acsWrapper.RemoveAllRulesInGroup(defaultRuleGroup.RuleGroup.Name);

            // Social IP
            CreateGoogleRules(acsWrapper, defaultRuleGroup);
            CreateYahooRules(acsWrapper, defaultRuleGroup);
            CreateWindowsLiveRules(acsWrapper, defaultRuleGroup);
        }

        private static void CreateGoogleRules(ServiceManagementWrapper acsWrapper, RelyingPartyRuleGroup defaultRuleGroup)
        {
            Console.Write("Creating Google mapping rules....");

            var identityProviderName = SocialIdentityProviders.Google.HomeRealm;

            // pass name
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, identityProviderName, ClaimTypes.Name);

            // pass nameidentifier
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, identityProviderName, ClaimTypes.NameIdentifier);

            Console.WriteLine("done.");
        }

        private static void CreateLitwareRules(ServiceManagementWrapper acsWrapper)
        {
            Console.Write("Creating Litware.7 mapping rules....");

            var relyingParty = acsWrapper.RetrieveRelyingParties().Single(rp => rp.Name == "Litware");
            var defaultRuleGroup = relyingParty.RelyingPartyRuleGroups.FirstOrDefault();
            var identityProviderName = LitwareIdentityProvider;

            // remove rules
            acsWrapper.RemoveAllRulesInGroup(defaultRuleGroup.RuleGroup.Name);

            // pass name
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, identityProviderName, ClaimTypes.Name);

            // add organization
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                Fabrikam.ClaimTypes.Organization, 
                "litware");

            // add cost center
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                Litware.ClaimTypes.CostCenter, 
                Fabrikam.ClaimTypes.CostCenter);

            // add role
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                ClaimTypes.Role, 
                Fabrikam.Roles.ShipmentCreator);

            // add role
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                ClaimTypes.Role, 
                Fabrikam.Roles.Administrator);

            // given name
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name, 
                identityProviderName, 
                ClaimTypes.GivenName,
                ClaimTypes.GivenName);

            // surname
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.Surname,
                ClaimTypes.Surname);

            // street address
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.StreetAddress,
                ClaimTypes.StreetAddress);


            // state or province
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.StateOrProvince,
                ClaimTypes.StateOrProvince);


            // country
            acsWrapper.AddPassThroughRuleToRuleGroup(
                defaultRuleGroup.RuleGroup.Name,
                identityProviderName,
                ClaimTypes.Country,
                ClaimTypes.Country);

            Console.WriteLine("done.");
        }

        private static void CreateWindowsLiveRules(ServiceManagementWrapper acsWrapper, RelyingPartyRuleGroup defaultRuleGroup)
        {
            Console.Write("Creating Windows Live ID mapping rules....");

            var name = SocialIdentityProviders.WindowsLiveId.DisplayName;

            // pass nameidentifier
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, name, ClaimTypes.NameIdentifier);

            // pass name
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, name, ClaimTypes.NameIdentifier, ClaimTypes.Name);

            Console.WriteLine("done.");
        }

        private static void CreateYahooRules(ServiceManagementWrapper acsWrapper, RelyingPartyRuleGroup defaultRuleGroup)
        {
            Console.Write("Creating Yahoo! mapping rules....");
            var name = SocialIdentityProviders.Yahoo.HomeRealm;

            // pass name
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, name, ClaimTypes.Name);

            // pass nameidentifier
            acsWrapper.AddPassThroughRuleToRuleGroup(defaultRuleGroup.RuleGroup.Name, name, ClaimTypes.NameIdentifier);

            Console.WriteLine("done.");
        }

        private static void Main()
        {
            var acs = new ServiceManagementWrapper(AcsServiceNamespace, AcsUsername, AcsPassword);

            Console.WriteLine("Setting up ACS namespace:" + AcsServiceNamespace);
            CleanupIdenityProviders(acs);
            CleanupRelyingParties(acs);
            CreateIdentityProviders(acs);
            CreateRelyingPartysWithRules(acs);

            Console.WriteLine("Setup complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}