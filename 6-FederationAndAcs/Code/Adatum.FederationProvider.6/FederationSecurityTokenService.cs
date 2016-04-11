//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Adatum.FederationProvider
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
            var issuer = input.Claims.First().Issuer.ToUpperInvariant();

            switch (issuer)
            {
                case "LITWARE":
                    CopyClaims(input, new[] { WSIdentityConstants.ClaimTypes.Name }, output);

                    TransformClaims(input, AllOrganizations.ClaimTypes.Group, Litware.Groups.Sales, ClaimTypes.Role, Adatum.Roles.OrderTracker, output);

                    output.Claims.Add(new Claim(Adatum.ClaimTypes.Organization, Litware.OrganizationName));

                    SingleSignOnManager.RegisterIssuer("https://localhost/Litware.SimulatedIssuer.6/");

                    break;

                case "ADATUM":
                    output = input;
                    SingleSignOnManager.RegisterIssuer("https://localhost/Adatum.SimulatedIssuer.6");

                    break;

                default:
                    if (issuer == TrustedIssuers.Instance.AcsIssuerName.ToUpperInvariant())
                    {
                        CopySocialClaims(input, output);
                        output.Claims.Add(new Claim(ClaimTypes.Role, Adatum.Roles.OrderTracker));
                        output.Claims.Add(new Claim(Adatum.ClaimTypes.Organization, "MaryInc"));

                        SingleSignOnManager.RegisterIssuer(TrustedIssuers.Instance.AcsIssuerEndpoint);

                    }
                    else
                    {
                        throw new InvalidOperationException("Issuer not trusted.");
                    }
                    break;
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
            output.Claims.AddRange(input.Claims.Where(c => claimTypes.Contains(c.ClaimType)));
        }

        private static void CopySocialClaims(IClaimsIdentity input, IClaimsIdentity output)
        {
            string issuer = input.Claims.Where(c => c.ClaimType.Equals("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider")).FirstOrDefault().Value;
            string nameClaimValue;
            string originalIssuer = "acs\\";
            switch (issuer)
            {
                case "Google":
                    nameClaimValue = input.Claims.Where(c => c.ClaimType.Equals(ClaimTypes.Name)).FirstOrDefault().Value;
                    originalIssuer = string.Concat(originalIssuer, "Google");
                    break;
                case "uri:WindowsLiveID":
                    nameClaimValue = input.Claims.Where(c => c.ClaimType.Equals(ClaimTypes.NameIdentifier)).FirstOrDefault().Value;
                    originalIssuer = string.Concat(originalIssuer, "LiveID");
                    break;

                case "Facebook-596390297202303":
                    nameClaimValue = input.Claims.Where(c => c.ClaimType.Equals(ClaimTypes.Name)).FirstOrDefault().Value;
                    originalIssuer = string.Concat(originalIssuer, "Facebook");
                    break;

                default:
                    throw new InvalidOperationException("Issuer not trusted.");
            }
            var claim = new Claim(ClaimTypes.Name, nameClaimValue, ClaimValueTypes.String, "adatum", originalIssuer);
            output.Claims.Add(claim);
        }

        private static void TransformClaims(IClaimsIdentity input, string inputClaimType, string inputClaimValue, string outputClaimType, string outputClaimValue, IClaimsIdentity output)
        {
            var inputClaims = input.Claims.Where(c => c.ClaimType == inputClaimType);
            if (inputClaims.Count(c => c.Value == inputClaimValue) > 0)
            {
                output.Claims.Add(new Claim(outputClaimType, outputClaimValue));
            }
        }
    }
}