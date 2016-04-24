//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Litware.SimulatedIssuer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
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
            var output = new ClaimsIdentity();

            if (null == principal)
            {
                throw new InvalidRequestException("The caller's principal is null.");
            }

            var input = (IClaimsIdentity)principal.Identity;

            switch (principal.Identity.Name.ToUpperInvariant())
            {
                    // In a production environment, all the information that will be added
                    // as claims should be read from the authenticated Windows Principal.
                    // The following lines are hardcoded because windows integrated 
                    // authentication is disabled.
                case "LITWARE\\RICK":
                    output.Claims.AddRange(new List<Claim>
                                               {
                                                   new Claim(ClaimTypes.Name, "rick"), 
                                                   new Claim(ClaimTypes.GivenName, "Rick"), 
                                                   new Claim(ClaimTypes.Surname, "Rico"), 
                                                   new Claim(ClaimTypes.StreetAddress, "51 Wide Rd."), 
                                                   new Claim(ClaimTypes.StateOrProvince, "WA"), 
                                                   new Claim(ClaimTypes.Country, "United States"), 
                                                   new Claim(Litware.ClaimTypes.CostCenter, Litware.CostCenters.Sales), 
                                                   new Claim(AllOrganizations.ClaimTypes.Group, Litware.Groups.Sales)
                                               });
                    break;
                default:
                    throw new UnauthorizedAccessException(string.Format(CultureInfo.CurrentUICulture, "User '{0}' is not authorized.", input.Name));
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

            scope.ReplyToAddress = request.ReplyTo ?? scope.AppliesToAddress;

            return scope;
        }
    }
}