//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AExpense
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IdentityModel.Claims;
    using System.Text;
    using System.Web;
    using System.Web.SessionState;
    using AExpense.Data;
    using Microsoft.IdentityModel.Protocols.WSIdentity;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Web;
    using Microsoft.IdentityModel.Web.Configuration;
    using Samples.Web.ClaimsUtilities;

    public class Global : HttpApplication
    {   
        protected void Session_Start(object sender, EventArgs e)
        {
            // Note that the application does not go to the database
            // to authenticate the user.
            // The authentication is done by the issuer. 
            // WSFederationAuthenticationModule automatically reads the 
            // user token sent by the IP and sets the user information 
            // in the thread current principal.
            if (this.Context.User.Identity.IsAuthenticated)
            {
                // At this point we can access the authenticated user information
                // by accessing the property Thread.CurrentPrincipal.
                string issuer = ClaimHelper.GetCurrentUserClaim(ClaimTypes.Name).OriginalIssuer;

                // Note, the GetClaim extension method is defined in the Samples.Web.Utillities project
                string givenName = ClaimHelper.GetCurrentUserClaim(WSIdentityConstants.ClaimTypes.GivenName).Value;
                string surname = ClaimHelper.GetCurrentUserClaim(WSIdentityConstants.ClaimTypes.Surname).Value;
                string costCenter = ClaimHelper.GetCurrentUserClaim(Adatum.ClaimTypes.CostCenter).Value;

                var repository = new UserRepository();
                string federatedUsername = GetFederatedUserName(issuer, this.User.Identity.Name);
                var user = repository.GetUser(federatedUsername);
                user.CostCenter = costCenter;
                user.FullName = givenName + " " + surname;

                // The user is stored in the session because the application 
                // authentication strategy requires it.
                this.Context.Session["LoggedUser"] = user;
            }
        }

        private static string GetFederatedUserName(string issuer, string username)
        {
            return string.Format(CultureInfo.InvariantCulture, @"{0}\{1}", issuer, username);
        }

        private static void OnServiceConfigurationCreated(object sender, ServiceConfigurationCreatedEventArgs e)
        {
            // Use the <serviceCertificate> to protect the cookies that are
            // sent to the client.
            var sessionTransforms =
                new List<CookieTransform>(
                    new CookieTransform[]
                        {
                            new DeflateCookieTransform(), 
                            new RsaEncryptionCookieTransform(e.ServiceConfiguration.ServiceCertificate), 
                            new RsaSignatureCookieTransform(e.ServiceConfiguration.ServiceCertificate)
                        });
            var sessionHandler = new SessionSecurityTokenHandler(sessionTransforms.AsReadOnly());

            e.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(sessionHandler);
        }

        private void Application_Start(object sender, EventArgs e)
        {
            FederatedAuthentication.ServiceConfigurationCreated += OnServiceConfigurationCreated;
        }

        private void WSFederationAuthenticationModule_SignedOut(object sender, EventArgs e)
        {
            Response.Redirect("~/CleanUp.aspx", false);
        }

        private void WSFederationAuthenticationModule_RedirectingToIdentityProvider(object sender, RedirectingToIdentityProviderEventArgs e)
        {
            // In the Windows Azure environment, build a wreply parameter for the SignIn request
            // that reflects the real address of the application.
            HttpRequest request = HttpContext.Current.Request;
            Uri requestUrl = request.Url;
            var wreply = new StringBuilder();

            wreply.Append(requestUrl.Scheme); // e.g. "http" or "https"
            wreply.Append("://");
            wreply.Append(request.Headers["Host"] ?? requestUrl.Authority);
            wreply.Append(request.ApplicationPath);

            if (!request.ApplicationPath.EndsWith("/"))
            {
                wreply.Append("/");
            }

            e.SignInRequestMessage.Reply = wreply.ToString();
        }
    }
}