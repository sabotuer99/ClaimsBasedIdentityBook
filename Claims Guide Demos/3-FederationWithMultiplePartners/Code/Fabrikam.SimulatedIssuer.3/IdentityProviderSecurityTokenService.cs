//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Fabrikam.SimulatedIssuer
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

            switch (principal.Identity.Name.ToUpperInvariant())
            {
                    // In a production environment, all the information that will be added
                    // as claims should be read from the users database.
                case "BILL@CONTOSO.COM":
                    outputIdentity.Claims.AddRange(new List<Claim>
                                                       {
                                                           new Claim(ClaimTypes.Name, "bill@contoso.com"), 
                                                           new Claim(ClaimTypes.GivenName, "Bill"), 
                                                           new Claim(ClaimTypes.Surname, "Rain"), 
                                                           new Claim(ClaimTypes.StreetAddress, "183 West Field Av."), 
                                                           new Claim(ClaimTypes.StateOrProvince, "WA"), 
                                                           new Claim(ClaimTypes.Country, "United States")
                                                       });
                    break;
            }

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
                scope.SymmetricKeyEncryptionRequired = false;
            }

            scope.ReplyToAddress = request.ReplyTo ?? scope.AppliesToAddress;

            return scope;
        }
    }
}