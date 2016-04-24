//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace SingleSignOutModule
{
    using System;
    using System.Text;
    using System.Web;
    using Microsoft.IdentityModel.Claims;
    using Microsoft.IdentityModel.Web;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration.Claims;

    public class SingleSignOutModule : IHttpModule
    {
        private const string SignOutCookieName = "SPSignOut";

        public void Init(HttpApplication context)
        {
            // Single Sign-Out
            FederatedAuthentication.WSFederationAuthenticationModule.SignedIn += WSFederationAuthenticationModule_SignedIn;
            FederatedAuthentication.SessionAuthenticationModule.SigningOut += SessionAuthenticationModule_SigningOut;
        }

        public void Dispose()
        {
        }

        private void WSFederationAuthenticationModule_SignedIn(object sender, EventArgs e)
        {
            IClaimsIdentity identity = HttpContext.Current.User.Identity as IClaimsIdentity;

            if (identity != null)
            {
                foreach (Claim claim in identity.Claims)
                {
                    if (claim.ClaimType == SPClaimTypes.IdentityProvider)
                    {
                        int index = claim.Value.IndexOf(':');
                        string loginProviderName = claim.Value.Substring(index + 1, claim.Value.Length - index - 1);
                        HttpCookie signOutCookie = new HttpCookie(SignOutCookieName, Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(loginProviderName)));
                        signOutCookie.Secure = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.RequireSsl;
                        signOutCookie.HttpOnly = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.HideFromClientScript;
                        signOutCookie.Domain = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Domain;
                        HttpContext.Current.Response.Cookies.Add(signOutCookie);
                        break;
                    }
                }
            }
        }

        private void SessionAuthenticationModule_SigningOut(object sender, EventArgs e)
        {
            DoFederatedSignOut();
        }

        private void DoFederatedSignOut()
        {
            string providerName = GetProviderNameFromCookie();
            SPTrustedLoginProvider loginProvider = null;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                loginProvider = GetLoginProvider(providerName);
            });

            if (loginProvider != null)
            {
                string returnUrl = string.Format(
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   "{0}://{1}/_layouts/SignOut.aspx",
                                   HttpContext.Current.Request.Url.Scheme,
                                   HttpContext.Current.Request.Url.Host);
                HttpCookie signOutExpiredCookie = new HttpCookie(SignOutCookieName, string.Empty);
                signOutExpiredCookie.Expires = new DateTime(1970, 1, 1);
                HttpContext.Current.Response.Cookies.Remove(SignOutCookieName);
                HttpContext.Current.Response.Cookies.Add(signOutExpiredCookie);
                WSFederationAuthenticationModule.FederatedSignOut(loginProvider.ProviderUri, new Uri(returnUrl));
            }
        }

        private static string GetProviderNameFromCookie()
        {
            var signOutCookie = HttpContext.Current.Request.Cookies[SignOutCookieName];

            if (signOutCookie != null)
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(signOutCookie.Value));
            }

            return string.Empty;
        }

        private SPTrustedLoginProvider GetLoginProvider(string providerName)
        {
            if (!string.IsNullOrEmpty(providerName))
            {
                try
                {
                    var stsManager = SPSecurityTokenServiceManager.Local;
                    var loginProviders = stsManager.TrustedLoginProviders;
                    return loginProviders.GetProviderByName(providerName);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine(ex.ToString());
                }
            }

            return null;
        }
    }
}
