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
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using ACS.ServiceManagementWrapper;
    using FShipping.Data;
    using FShipping.Models;
    using FShipping.Security;
    using Microsoft.IdentityModel.Claims;
    using Samples.Web.ClaimsUtilities;

    public class EnrollmentController : Controller
    {
        private readonly string acsServiceNamespace;
        private readonly string acsUsername;
        private readonly string acsPassword;

        public EnrollmentController()
        {
            this.acsServiceNamespace = ConfigurationManager.AppSettings["acs_servicenamespace"];
            this.acsUsername = ConfigurationManager.AppSettings["acs_username"];
            this.acsPassword = ConfigurationManager.AppSettings["acs_password"];
        }

        // GET
        public ActionResult EnrollWithFedMetadataFile()
        {
            return this.View(new EnrollmentViewModel());
        }

        // POST
        public ActionResult CreateTenantFromFedMetadaFile(string organizationName, HttpPostedFileBase fedMetadataFile, HttpPostedFileBase logoFile, string adminClaimType, string adminClaimValue, string costCenterClaimType)
        {
            string organizationInternalName = this.SanitizeString(organizationName);

            if (this.IsOrganizationNameValid(organizationInternalName))
            {
                Organization organization = new Organization { Name = organizationInternalName, DisplayName = organizationName, LogoPath = "~/Content/images/generic-logo.png" };

                if (logoFile != null && logoFile.ContentLength > 0)
                {
                    var imageFolderRelativePath = "~/Content/images/";
                    var imageFolderAbsolutePath = Server.MapPath("~/");
                    imageFolderAbsolutePath = string.Concat(imageFolderAbsolutePath, "..\\f-shipping.7\\Content\\images\\");
                    var fileName = string.Concat(organizationInternalName, "-logo.png");
                    var fileFullPath = string.Concat(imageFolderAbsolutePath, fileName);
                    logoFile.SaveAs(fileFullPath);
                    organization.LogoPath = string.Concat(imageFolderRelativePath, fileName);
                }

                OrganizationRepository organizationRepository = new OrganizationRepository();
                organizationRepository.AddOrganization(organization);
                ServiceManagementWrapper acsWrapper = new ServiceManagementWrapper(acsServiceNamespace, acsUsername, acsPassword);

                // add the new IP
                var identityProviderName = organizationInternalName;
                StreamReader sr = new StreamReader(fedMetadataFile.InputStream);
                byte[] fedMetadataBytes = new byte[fedMetadataFile.InputStream.Length];
                fedMetadataFile.InputStream.Read(fedMetadataBytes, 0, (int)fedMetadataFile.InputStream.Length);
                acsWrapper.AddIdentityProvider(identityProviderName, fedMetadataBytes);

                var ruleGroup = string.Format("Default role group for {0}", organizationInternalName);

                this.CreateRelyingParty(organizationInternalName, identityProviderName, ruleGroup, acsWrapper);
                this.CreateRulesForTenantWithOwnIP(organizationInternalName, identityProviderName, acsWrapper, ruleGroup, adminClaimType, adminClaimValue, costCenterClaimType);

                return View("CompleteEnrollment");
            }
            return View("EnrollWithFedMetadataFile", new EnrollmentViewModel { ErrorMessage = "Organization name not valid", OrganizationName = organizationName });
        }


        // GET
        public ActionResult EnrollManually()
        {
            return this.View(new EnrollmentViewModel());
        }

        // POST
        public ActionResult CreateTenantManually(string organizationName, string issuerName, string iPStsAddress, string adminClaimType, string adminClaimValue, HttpPostedFileBase certificateFile, HttpPostedFileBase logoFile, string costCenterClaimType)
        {
            string organizationInternalName = this.SanitizeString(organizationName);

            if (this.IsOrganizationNameValid(organizationInternalName))
            {
                Organization organization = new Organization { Name = organizationInternalName, DisplayName = organizationName, LogoPath = "~/Content/images/generic-logo.png" };

                if (logoFile != null && logoFile.ContentLength > 0)
                {
                    var imageFolderRelativePath = "~/Content/images/";
                    var imageFolderAbsolutePath = Server.MapPath("~/");
                    imageFolderAbsolutePath = string.Concat(imageFolderAbsolutePath, "..\\f-shipping.7\\Content\\images\\");
                    var fileName = string.Concat(organizationInternalName, "-logo.png");
                    var fileFullPath = string.Concat(imageFolderAbsolutePath, fileName);
                    logoFile.SaveAs(fileFullPath);
                    organization.LogoPath = string.Concat(imageFolderRelativePath, fileName);
                }

                OrganizationRepository organizationRepository = new OrganizationRepository();
                organizationRepository.AddOrganization(organization);
                ServiceManagementWrapper acsWrapper = new ServiceManagementWrapper(acsServiceNamespace, acsUsername, acsPassword);

                // add the new IP
                var identityProviderName = issuerName;
                byte[] signingCertBytes = new byte[certificateFile.InputStream.Length];
                certificateFile.InputStream.Read(signingCertBytes, 0, (int)certificateFile.InputStream.Length);
                acsWrapper.AddIdentityProviderManually(identityProviderName, iPStsAddress, WebSSOProtocolType.WsFederation, signingCertBytes, null);

                var ruleGroup = string.Format("Default role group for {0}", organizationInternalName);

                this.CreateRelyingParty(organizationInternalName, identityProviderName, ruleGroup, acsWrapper);
                this.CreateRulesForTenantWithOwnIP(organizationInternalName, identityProviderName, acsWrapper, ruleGroup, adminClaimType, adminClaimValue, costCenterClaimType);


                return View("CompleteEnrollment");
            }
            return View("EnrollManually", new EnrollmentViewModel { ErrorMessage = "Organization name not valid", OrganizationName = organizationName });
        }

        // GET
        [AuthenticateAndAuthorize()]
        public ActionResult EnrollWithSocialProvider(string socialIP)
        {
            return View();
        }

        // POST
        [AuthenticateAndAuthorize()]
        public ActionResult CreateTenantWithSocialProvider(string OrganizationName, HttpPostedFileBase logoFile)
        {
            string organizationInternalName = SanitizeString(OrganizationName);

            if (this.IsOrganizationNameValid(organizationInternalName))
            {
                var ipName = ClaimHelper.GetCurrentUserClaim(Fabrikam.ClaimTypes.IdentityProvider).Value;
                if (ipName == SocialIdentityProviders.WindowsLiveId.HomeRealm)
                {
                    ipName = SocialIdentityProviders.WindowsLiveId.DisplayName;
                }
                Organization organization = new Organization { Name = organizationInternalName, DisplayName = OrganizationName, HomeRealm = ipName, LogoPath = "~/Content/images/generic-logo.png" };

                if (logoFile != null && logoFile.ContentLength > 0)
                {
                    var imageFolderRelativePath = "~/Content/images/";
                    var imageFolderAbsolutePath = Server.MapPath("~/");
                    imageFolderAbsolutePath = string.Concat(imageFolderAbsolutePath, "..\\f-shipping.7\\Content\\images\\");
                    var fileName = string.Concat(organizationInternalName, "-logo.png");
                    var fileFullPath = string.Concat(imageFolderAbsolutePath, fileName);
                    logoFile.SaveAs(fileFullPath);
                    organization.LogoPath = string.Concat(imageFolderRelativePath, fileName);
                }

                OrganizationRepository organizationRepository = new OrganizationRepository();
                organizationRepository.AddOrganization(organization);
                ServiceManagementWrapper acsWrapper = new ServiceManagementWrapper(acsServiceNamespace, acsUsername, acsPassword);

                var relayingPartyName = organizationInternalName;
                var realmAddress = string.Format("https://localhost/f-shipping.7/{0}", organizationInternalName);
                var replyAddress = string.Format("https://localhost/f-shipping.7/{0}/FederationResult", organizationInternalName);
                var ruleGroup = string.Format("Default role group for {0}", organizationInternalName);
                var socialProviders = new string[] {ipName};

                acsWrapper.AddRelyingParty(organizationInternalName, realmAddress, replyAddress, null, null, null, ruleGroup, socialProviders);
                
                var nameIdentifierValue = ClaimHelper.GetCurrentUserClaim(ClaimTypes.NameIdentifier).Value;

                CreateRulesForTenantWithSocialIP(organizationInternalName, ipName, acsWrapper, ruleGroup, nameIdentifierValue);

                return View("CompleteEnrollment");
            }
            return View("EnrollWithSocialProvider",new EnrollmentViewModel { ErrorMessage = "Organization name not valid", OrganizationName = OrganizationName });
        }

        private void CreateRelyingParty(string relyingPartyName, string identityProviderName, string ruleGroup, ServiceManagementWrapper acsWrapper)
        {
            // add the relaying party
            var realmAddress = string.Format("https://localhost/f-shipping.7/{0}", relyingPartyName);
            var replyAddress = string.Format("https://localhost/f-shipping.7/{0}/FederationResult", relyingPartyName);
            var identityProviders = new string[] { identityProviderName };
            acsWrapper.AddRelyingParty(relyingPartyName, realmAddress, replyAddress, null, null, null, ruleGroup, identityProviders);
        }

        private void CreateRulesForTenantWithSocialIP(string organizationInternalName, string identityProviderName, ServiceManagementWrapper acsWrapper, string ruleGroup, string nameIdentifierValue)
        {
            // pass nameidentifier
            acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                    identityProviderName,
                                    ClaimTypes.NameIdentifier,
                                    nameIdentifierValue,
                                    ClaimTypes.NameIdentifier,
                                    nameIdentifierValue);


            // pass name
            if (identityProviderName.Equals(SocialIdentityProviders.WindowsLiveId))
            {
                acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                        identityProviderName,
                                        ClaimTypes.NameIdentifier,
                                        nameIdentifierValue,
                                        ClaimTypes.Name,
                                        nameIdentifierValue);
            }
            else
            {
                var userName = ClaimHelper.GetCurrentUserClaim(ClaimTypes.Name).Value;
                acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                        identityProviderName,
                                        ClaimTypes.NameIdentifier,
                                        nameIdentifierValue,
                                        ClaimTypes.Name,
                                        userName);
            }

            // add organization
            acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                                identityProviderName,
                                                ClaimTypes.NameIdentifier,
                                                nameIdentifierValue,
                                                Fabrikam.ClaimTypes.Organization,
                                                organizationInternalName);

            // add costcenter
            acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                                identityProviderName,
                                                ClaimTypes.NameIdentifier,
                                                nameIdentifierValue,
                                                Fabrikam.ClaimTypes.CostCenter,
                                                Fabrikam.ClaimValues.SingleCostCenter);

            // add role
            acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                    identityProviderName,
                                    ClaimTypes.NameIdentifier,
                                    nameIdentifierValue,
                                    Microsoft.IdentityModel.Claims.ClaimTypes.Role,
                                    Fabrikam.Roles.ShipmentCreator);

            // add role
            acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                    identityProviderName,
                                    ClaimTypes.NameIdentifier,
                                    nameIdentifierValue,
                                    Microsoft.IdentityModel.Claims.ClaimTypes.Role,
                                    Fabrikam.Roles.Administrator);
        }

        private void CreateRulesForTenantWithOwnIP(string organizationInternalName, string identityProviderName, ServiceManagementWrapper acsWrapper, string ruleGroup, string adminClaimType, string adminClaimValue, string costCenterClaimType) 
        {
            // name
            acsWrapper.AddPassThroughRuleToRuleGroup(ruleGroup,
                                        identityProviderName,
                                        ClaimTypes.Name);


            // add organization
            acsWrapper.AddSimpleRuleToRuleGroupWithoutSpecifyInputClaim(ruleGroup,
                                                identityProviderName,
                                                Fabrikam.ClaimTypes.Organization,
                                                organizationInternalName);

            if (!string.IsNullOrEmpty(costCenterClaimType))
            {
                // add costcenter
                acsWrapper.AddPassThroughRuleToRuleGroup(ruleGroup,
                                                    identityProviderName,
                                                    costCenterClaimType,
                                                    Fabrikam.ClaimTypes.CostCenter);
            }

            // add role
            acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                                identityProviderName,
                                                adminClaimType,
                                                adminClaimValue,
                                                Microsoft.IdentityModel.Claims.ClaimTypes.Role,
                                                Fabrikam.Roles.ShipmentCreator);

            // add role
            acsWrapper.AddSimpleRuleToRuleGroup(ruleGroup,
                                                identityProviderName,
                                                adminClaimType,
                                                adminClaimValue,
                                                Microsoft.IdentityModel.Claims.ClaimTypes.Role,
                                                Fabrikam.Roles.Administrator);
        }

        private bool IsOrganizationNameValid(string organizationName)
        {
            if (string.IsNullOrEmpty(organizationName))
                return false;

            if (organizationName.Length < 2)
                return false;

            OrganizationRepository organizationRepository = new OrganizationRepository();
            return organizationRepository.IsNameAvailable(organizationName);
        }

        private string SanitizeString(string aString)
        {
            var result = aString.Trim();
            result = result.Replace(" ", string.Empty);
            return result;
        }

    }
}
