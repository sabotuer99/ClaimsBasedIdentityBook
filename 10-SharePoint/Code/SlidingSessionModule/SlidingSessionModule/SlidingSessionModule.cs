//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace SlidingSessionModule
{
    using System;
    using System.Text;
    using System.Web;
    using Microsoft.IdentityModel.Claims;
    using Microsoft.IdentityModel.Web;
    using Microsoft.SharePoint;
    using Microsoft.SharePoint.Administration.Claims;

    public class SlidingSessionModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            // Sliding session
            FederatedAuthentication.SessionAuthenticationModule.SessionSecurityTokenReceived += SessionAuthenticationModule_SessionSecurityTokenReceived;
        }

        public void Dispose()
        {
        }

        private void SessionAuthenticationModule_SessionSecurityTokenReceived(object sender, SessionSecurityTokenReceivedEventArgs e)
        {
            double sessionLifetimeInMinutes =
                (e.SessionToken.ValidTo - e.SessionToken.ValidFrom).TotalMinutes;
            var logonTokenCacheExpirationWindow = TimeSpan.FromSeconds(1);
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                logonTokenCacheExpirationWindow =
                    Microsoft.SharePoint.Administration.Claims.SPSecurityTokenServiceManager.Local.LogonTokenCacheExpirationWindow;
            });

            DateTime now = DateTime.UtcNow;
            DateTime validTo = e.SessionToken.ValidTo - logonTokenCacheExpirationWindow;
            DateTime validFrom = e.SessionToken.ValidFrom;

            if ((now < validTo) && (now > validFrom.AddMinutes((validTo - validFrom).TotalMinutes / 2)))
            {
                SessionAuthenticationModule sam = FederatedAuthentication.SessionAuthenticationModule;
                e.SessionToken = sam.CreateSessionSecurityToken(e.SessionToken.ClaimsPrincipal, e.SessionToken.Context,
                    now, now.AddMinutes(sessionLifetimeInMinutes), e.SessionToken.IsPersistent);

                e.ReissueCookie = true;
            }
        }
    }
}
