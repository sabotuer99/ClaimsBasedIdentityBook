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
    using System.Web;

    public static class SingleSignOnManager
    {
        private const string RelyingPartySiteCookieName = "AdatumClaimsRPStsSiteCookie";
        private const string RelyingPartySiteName = "AdatumClaimsRPStsSite";
        private const string IssuerSiteCookieName = "AdatumClaimsIssuerStsSiteCookie";
        private const string IssuerSiteName = "AdatumClaimsIssuerStsSite";


        public static void RegisterRelyingParty(string url)
        {
            if (HttpContext.Current != null)
            {
                HttpCookie siteCookie = HttpContext.Current.Request.Cookies[RelyingPartySiteCookieName] ?? new HttpCookie(RelyingPartySiteCookieName);
                
                siteCookie.Values.Add(RelyingPartySiteName, url);
                
                HttpContext.Current.Response.AppendCookie(siteCookie);
            }
        }


        public static void RegisterIssuer(string url)
        {
            if (HttpContext.Current != null)
            {
                HttpCookie siteCookie = HttpContext.Current.Request.Cookies[IssuerSiteCookieName] ?? new HttpCookie(IssuerSiteCookieName);

                siteCookie.Values.Add(IssuerSiteName, url);

                HttpContext.Current.Response.AppendCookie(siteCookie);
            }
        }

        public static string[] SignOutRelyingParties()
        {
            if (HttpContext.Current != null)
            {
                HttpCookie siteCookie = HttpContext.Current.Request.Cookies[RelyingPartySiteCookieName];

                if (siteCookie != null)
                {
                    var urls = siteCookie.Values.GetValues(RelyingPartySiteName);
                    return urls ?? new string[0];
                }
            }

            return new string[0];
        }

        public static string[] SignOutIssuers()
        {
            if (HttpContext.Current != null)
            {
                HttpCookie siteCookie = HttpContext.Current.Request.Cookies[IssuerSiteCookieName];

                if (siteCookie != null)
                {
                    var urls = siteCookie.Values.GetValues(IssuerSiteName);
                    return urls ?? new string[0];
                }
            }

            return new string[0];
        }

        public static void Clear()
        {
            if (HttpContext.Current != null)
            {
                HttpCookie relyingPartyCookie = HttpContext.Current.Request.Cookies[RelyingPartySiteCookieName];

                if (relyingPartyCookie != null)
                {
                    relyingPartyCookie.Values.Clear();
                    HttpContext.Current.Response.AppendCookie(relyingPartyCookie);
                }

                HttpCookie issuerCookie = HttpContext.Current.Request.Cookies[IssuerSiteCookieName];

                if (issuerCookie != null)
                {
                    issuerCookie.Values.Clear();
                    HttpContext.Current.Response.AppendCookie(issuerCookie);
                }
            }
        }
    }
}