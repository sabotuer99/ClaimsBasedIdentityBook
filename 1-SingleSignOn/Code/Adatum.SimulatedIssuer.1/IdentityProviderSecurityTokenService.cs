//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Adatum.SimulatedIssuer
{
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.Configuration;
    using Microsoft.IdentityModel.Claims;
    using Microsoft.IdentityModel.Configuration;
    using Microsoft.IdentityModel.Protocols.WSTrust;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Samples.Web.ClaimsUtilities;
    using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

    public class IdentityProviderSecurityTokenService : SecurityTokenService
    {
        public IdentityProviderSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            var outputIdentity = new ClaimsIdentity();

            if (null == principal)
            {
                throw new InvalidRequestException("The caller's principal is null.");
            }

            SingleSignOnManager.RegisterRelyingParty(scope.ReplyToAddress);

            // In a production environment, all the information that will be added
            // as claims should be read from the authenticated Windows Principal.
            // The following lines are hardcoded because windows integrated 
            // authentication is disabled.
            switch (principal.Identity.Name.ToUpperInvariant())
            {
                case "ADATUM\\JOHNDOE":
                    outputIdentity.Claims.AddRange(new List<Claim>
                                                       {
                                                           new Claim(ClaimTypes.Name, "johndoe"), 
                                                           new Claim(ClaimTypes.GivenName, "John"), 
                                                           new Claim(ClaimTypes.Surname, "Doe"), 
                                                           new Claim(ClaimTypes.StreetAddress, "12 Green park Ln."), 
                                                           new Claim(ClaimTypes.StateOrProvince, "WA"), 
                                                           new Claim(ClaimTypes.Country, "United States"), 
                                                           new Claim(Adatum.ClaimTypes.CostCenter, Adatum.CostCenters.CustomerService), 
                                                           new Claim(Microsoft.IdentityModel.Claims.ClaimTypes.Role, Adatum.Roles.OrderTracker), 
                                                           new Claim(AllOrganizations.ClaimTypes.Group, Adatum.Groups.CustomerService)
                                                       });
                    break;
                case "ADATUM\\MARY":
                    outputIdentity.Claims.AddRange(new List<Claim>
                                                       {
                                                           new Claim(ClaimTypes.Name, "mary"), 
                                                           new Claim(ClaimTypes.GivenName, "Mary"), 
                                                           new Claim(ClaimTypes.Surname, "May"), 
                                                           new Claim(ClaimTypes.StreetAddress, "164 Big Lake Av."), 
                                                           new Claim(ClaimTypes.StateOrProvince, "WA"), 
                                                           new Claim(ClaimTypes.Country, "United States"), 
                                                           new Claim(Adatum.ClaimTypes.CostCenter, Adatum.CostCenters.Accountancy), 
                                                           new Claim(Microsoft.IdentityModel.Claims.ClaimTypes.Role, Adatum.Roles.OrderTracker), 
                                                           new Claim(Microsoft.IdentityModel.Claims.ClaimTypes.Role, Adatum.Roles.OrderApprover), 
                                                           new Claim(AllOrganizations.ClaimTypes.Group, Adatum.Groups.CustomerService)
                                                       });
                    break;
                case "ADATUM\\PETER":
                    outputIdentity.Claims.AddRange(new List<Claim>
                                                       {
                                                           new Claim(ClaimTypes.Name, "peter"), 
                                                           new Claim(ClaimTypes.GivenName, "Peter"), 
                                                           new Claim(ClaimTypes.Surname, "Porter"), 
                                                           new Claim(ClaimTypes.StreetAddress, "45 Top hill Rd."), 
                                                           new Claim(ClaimTypes.StateOrProvince, "WA"), 
                                                           new Claim(ClaimTypes.Country, "United States"), 
                                                           new Claim(Adatum.ClaimTypes.CostCenter, Adatum.CostCenters.CustomerService), 
                                                           new Claim(Microsoft.IdentityModel.Claims.ClaimTypes.Role, Adatum.Roles.OrderTracker), 
                                                           new Claim(AllOrganizations.ClaimTypes.Group, Adatum.Groups.OrderFulfillments), 
                                                           new Claim(AllOrganizations.ClaimTypes.Group, Adatum.Groups.ItAdmins), 
                                                       });
                    break;
            }

            outputIdentity.Claims.Add(new Claim(Adatum.ClaimTypes.Organization, Adatum.OrganizationName));

            return outputIdentity;
        }

        protected override Scope GetScope(IClaimsPrincipal principal, RequestSecurityToken request)
        {
            var scope = new Scope(request.AppliesTo.Uri.AbsoluteUri, this.SecurityTokenServiceConfiguration.SigningCredentials);

            string encryptingCertificateName = WebConfigurationManager.AppSettings[ApplicationSettingsNames.EncryptingCertificateName];
            if (!string.IsNullOrEmpty(encryptingCertificateName))
            {
                scope.EncryptingCredentials = new X509EncryptingCredentials(CertificateUtilities.GetCertificate(StoreName.My, StoreLocation.LocalMachine, encryptingCertificateName));
            }
            else
            {
                scope.TokenEncryptionRequired = false;
            }

            scope.ReplyToAddress = scope.AppliesToAddress;

            return scope;
        }
    }
}