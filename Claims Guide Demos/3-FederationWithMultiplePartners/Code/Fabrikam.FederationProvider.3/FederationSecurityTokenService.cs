//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Fabrikam.FederationProvider
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.Configuration;
    using Microsoft.IdentityModel.Claims;
    using Microsoft.IdentityModel.Configuration;
    using Microsoft.IdentityModel.Protocols.WSIdentity;
    using Microsoft.IdentityModel.Protocols.WSTrust;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Samples.Web.ClaimsUtilities;

    public class FederationSecurityTokenService : SecurityTokenService
    {
        public FederationSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        protected override IClaimsIdentity GetOutputClaimsIdentity(IClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            var output = new ClaimsIdentity();

            if (null == principal)
            {
                throw new InvalidRequestException("The caller's principal is null.");
            }

            SingleSignOnManager.RegisterRelyingParty(scope.ReplyToAddress);

            var input = (ClaimsIdentity)principal.Identity;
            var issuer = input.Claims.First().Issuer;

            switch (issuer.ToUpperInvariant())
            {
                case "ADATUM":
                    var adatumClaimTypesToCopy = new[]
                                                     {
                                                         WSIdentityConstants.ClaimTypes.Name, 
                                                         ClaimTypes.GivenName, 
                                                         ClaimTypes.Surname, 
                                                         ClaimTypes.StreetAddress, 
                                                         ClaimTypes.StateOrProvince, 
                                                         ClaimTypes.Country
                                                     };
                    CopyClaims(input, adatumClaimTypesToCopy, output);

                    TransformClaims(input, AllOrganizations.ClaimTypes.Group, Adatum.Groups.CustomerService, ClaimTypes.Role, Fabrikam.Roles.ShipmentCreator, output);
                    TransformClaims(input, AllOrganizations.ClaimTypes.Group, Adatum.Groups.OrderFulfillments, ClaimTypes.Role, Fabrikam.Roles.ShipmentCreator, output);
                    TransformClaims(input, AllOrganizations.ClaimTypes.Group, Adatum.Groups.OrderFulfillments, ClaimTypes.Role, Fabrikam.Roles.ShipmentManager, output);
                    TransformClaims(input, AllOrganizations.ClaimTypes.Group, Adatum.Groups.ItAdmins, ClaimTypes.Role, Fabrikam.Roles.Administrator, output);
                    TransformClaims(input, Adatum.ClaimTypes.CostCenter, "*", Fabrikam.ClaimTypes.CostCenter, "*", output);

                    output.Claims.Add(new Claim(Fabrikam.ClaimTypes.Organization, Adatum.OrganizationName));

                    SingleSignOnManager.RegisterIssuer("https://localhost/Adatum.SimulatedIssuer.3/");

                    break;
                case "LITWARE":
                    var litwareClaimTypesToCopy = new[]
                                                      {
                                                          WSIdentityConstants.ClaimTypes.Name, 
                                                          ClaimTypes.GivenName, 
                                                          ClaimTypes.Surname, 
                                                          ClaimTypes.StreetAddress, 
                                                          ClaimTypes.StateOrProvince, 
                                                          ClaimTypes.Country
                                                      };
                    CopyClaims(input, litwareClaimTypesToCopy, output);

                    TransformClaims(input, AllOrganizations.ClaimTypes.Group, Litware.Groups.Sales, ClaimTypes.Role, Fabrikam.Roles.ShipmentCreator, output);
                    TransformClaims(input, Litware.ClaimTypes.CostCenter, "*", Fabrikam.ClaimTypes.CostCenter, "*", output);

                    output.Claims.Add(new Claim(Fabrikam.ClaimTypes.Organization, Litware.OrganizationName));

                    SingleSignOnManager.RegisterIssuer("https://localhost/Litware.SimulatedIssuer.3/");

                    break;
                case "FABRIKAM-SIMPLE":
                    var fabrikamSimpleClaimTypesToCopy = new[]
                                                             {
                                                                 WSIdentityConstants.ClaimTypes.Name, 
                                                                 ClaimTypes.GivenName, 
                                                                 ClaimTypes.Surname, 
                                                                 ClaimTypes.StreetAddress, 
                                                                 ClaimTypes.StateOrProvince, 
                                                                 ClaimTypes.Country
                                                             };
                    CopyClaims(input, fabrikamSimpleClaimTypesToCopy, output);

                    switch (input.Name.ToUpperInvariant())
                    {
                            // In a production environment, all the claims for the users are taken from claim 
                            // mappings where the user name is the input claim and all the claims added here 
                            // are output claims.
                        case "BILL@CONTOSO.COM":
                            output.Claims.AddRange(new List<Claim>
                                                       {
                                                           new Claim(ClaimTypes.Role, Fabrikam.Roles.Administrator), 
                                                           new Claim(ClaimTypes.Role, Fabrikam.Roles.ShipmentManager), 
                                                           new Claim(ClaimTypes.Role, Fabrikam.Roles.ShipmentCreator), 
                                                           new Claim(Fabrikam.ClaimTypes.CostCenter, Contoso.CostCenters.SingleCostCenter), 
                                                           new Claim(Fabrikam.ClaimTypes.Organization, Contoso.OrganizationName)
                                                       });

                            break;
                    }

                    SingleSignOnManager.RegisterIssuer("https://localhost/Fabrikam.SimulatedIssuer.3/");
                    break;

                default:
                    throw new InvalidOperationException("Issuer not trusted.");
            }

            return output;
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
                scope.SymmetricKeyEncryptionRequired = false;
            }

            scope.ReplyToAddress = scope.AppliesToAddress;

            return scope;
        }

        private static void CopyClaims(IClaimsIdentity input, IEnumerable<string> claimTypes, IClaimsIdentity output)
        {
            output.Claims.CopyRange(input.Claims.Where(c => claimTypes.Contains(c.ClaimType)));
        }

        private static void TransformClaims(IClaimsIdentity input, string inputClaimType, string inputClaimValue, string outputClaimType, string outputClaimValue, IClaimsIdentity output)
        {
            var inputClaims = input.Claims.Where(c => c.ClaimType == inputClaimType);
            if ((inputClaimValue == "*") && (outputClaimValue == "*"))
            {
                var claimsToAdd = inputClaims.Select(c => new Claim(outputClaimType, c.Value));
                output.Claims.AddRange(claimsToAdd);
            }
            else
            {
                if (inputClaims.Count(c => c.Value == inputClaimValue) > 0)
                {
                    output.Claims.Add(new Claim(outputClaimType, outputClaimValue));
                }
            }
        }
    }
}