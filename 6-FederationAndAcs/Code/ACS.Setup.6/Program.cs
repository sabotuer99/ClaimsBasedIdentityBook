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
    using System.Linq;
    using ACS.ServiceManagementWrapper;
    using ACS.ServiceManagementWrapper.ACS.Management;
    using Microsoft.IdentityModel.Claims;

    internal class Program
    {
        private const string AcsPassword = "Your ACS Password";
        private const string AcsServiceNamespace = "Your ACS Namespace";
        private const string AcsUsername = "Your ACS Username";

        public static void CreateRelyingPartysWithRules(ServiceManagementWrapper acsWrapper)
        {
            var relyingPartyName = "Adatum.SimulatedIssuer.6";

            var ips = new[] { SocialIdentityProviders.WindowsLiveId.DisplayName, SocialIdentityProviders.Google.DisplayName, "Facebook" };

            Console.Write(string.Format("Creating {0} relying party....", relyingPartyName));
            var realmAddress = "https://localhost/Adatum.FederationProvider.6/";
            var replyAddress = "https://localhost/Adatum.FederationProvider.6/Federation.aspx";
            var ruleGroup = string.Format("Default role group for {0}", relyingPartyName);


            acsWrapper.AddRelyingParty(relyingPartyName, realmAddress, replyAddress, null, null, null, ruleGroup, ips);

            Console.WriteLine("done");


            var relyingParty = acsWrapper.RetrieveRelyingParties().Single(rp => rp.Name == relyingPartyName);
            var defaultRuleGroup = relyingParty.RelyingPartyRuleGroups.FirstOrDefault();
            acsWrapper.RemoveAllRulesInGroup(defaultRuleGroup.RuleGroup.Name);

            // Social IPs
            CreateGoogleRules(acsWrapper, defaultRuleGroup);
            CreateFacebookRules(acsWrapper, defaultRuleGroup);
            CreateWindowsLiveRules(acsWrapper, defaultRuleGroup);
           
        }
    
        private static void CleanupRelyingParty(ServiceManagementWrapper acsWrapper)
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

        private static void CreateFacebookRules(ServiceManagementWrapper acsWrapper, RelyingPartyRuleGroup defaultRuleGroup)
        {
            Console.Write("Creating Facebook mapping rules....");
            var name = "Facebook";

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

            CleanupRelyingParty(acs);

            acs.RemoveIdentityProvider("Google");
            acs.AddGoogleIdentityProvider();
            acs.RemoveIdentityProvider("Facebook");
            acs.AddFacebookIdentityProvider("Facebook", "Your Facebook Application Id", "Your Facebook Application Secret");

            CreateRelyingPartysWithRules(acs);

            Console.WriteLine("Setup complete. Press any key to exit.");
            Console.ReadKey();
        }
    }
}