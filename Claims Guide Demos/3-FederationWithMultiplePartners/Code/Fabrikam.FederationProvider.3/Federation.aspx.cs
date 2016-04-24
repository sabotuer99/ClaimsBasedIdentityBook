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
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Microsoft.IdentityModel.Web;

    public partial class Federation : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            string action = this.Request.QueryString[WSFederationConstants.Parameters.Action] ?? this.Request.Form[WSFederationConstants.Parameters.Action];

            if (action == WSFederationConstants.Actions.SignIn)
            {
                if (this.User != null && this.User.Identity.IsAuthenticated)
                {
                    this.HandleSignInResponse();
                }
                else
                {
                    this.HandleSignInRequest();
                }
            }
            else
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture, 
                        "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}' or '{3}'.", 
                        String.IsNullOrEmpty(action) ? "<EMPTY>" : action, 
                        WSFederationConstants.Parameters.Action, 
                        WSFederationConstants.Actions.SignIn, 
                        WSFederationConstants.Actions.SignOut));
            }

            base.OnLoad(e);
        }

        private void CreateContextCookie(string contextId, string context)
        {
            var contextCookie = new HttpCookie(contextId, context)
                                    {
                                        Secure = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.RequireSsl, 
                                        Path = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Path, 
                                        Domain = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Domain, 
                                        HttpOnly = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.HideFromClientScript
                                    };

            if (FederatedAuthentication.SessionAuthenticationModule.CookieHandler.PersistentSessionLifetime.HasValue &&
                (FederatedAuthentication.SessionAuthenticationModule.CookieHandler.PersistentSessionLifetime != TimeSpan.Zero))
            {
                contextCookie.Expires =
                    DateTime.UtcNow.Add(
                        FederatedAuthentication.SessionAuthenticationModule.CookieHandler.PersistentSessionLifetime.Value);
            }

            this.Response.Cookies.Add(contextCookie);
        }

        private void DeleteContextCookie(string contextId)
        {
            var contextCookie = new HttpCookie(contextId)
                                    {
                                        Secure = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.RequireSsl, 
                                        Path = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Path, 
                                        Domain = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Domain, 
                                        HttpOnly = FederatedAuthentication.SessionAuthenticationModule.CookieHandler.HideFromClientScript, 
                                        Expires = DateTime.UtcNow.AddDays(-1)
                                    };

            this.Response.Cookies.Add(contextCookie);
        }

        private void HandleSignInRequest()
        {
            var homeRealm = this.Request["whr"];
            string issuerLocation;
            string realm;
            string reply = string.Empty;
            string whr = string.Empty;

            if (FederationIssuers.Instance.IsValidRealm(homeRealm))
            {
                var federatedIssuer = FederationIssuers.Instance[homeRealm];
                issuerLocation = federatedIssuer.IssuerLocation;
                realm = FederatedAuthentication.WSFederationAuthenticationModule.Realm;
                if (federatedIssuer.IsSocialIssuer)
                {
                    whr = federatedIssuer.Whr;
                }
                else
                {
                    reply = this.Request.Url.AbsoluteUri.Remove(this.Request.Url.AbsoluteUri.IndexOf(this.Request.Url.Query, StringComparison.OrdinalIgnoreCase));
                }
            }
            else
            {
                throw new InvalidOperationException("The home realm is not trusted for federation.");
            }

            var contextId = Guid.NewGuid().ToString();
            this.CreateContextCookie(contextId, this.Request.Url.AbsoluteUri);

            var message = new SignInRequestMessage(new Uri(issuerLocation), realm)
                              {
                                  CurrentTime = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture) + "Z", 
                                  Reply = reply, 
                                  Context = contextId, 
                                  HomeRealm = whr
                              };

            this.Response.Redirect(message.RequestUrl, false);
        }

        private void HandleSignInResponse()
        {
            var responseMessage = WSFederationMessage.CreateFromFormPost(this.Page.Request);
            this.HandleSignInResponse(responseMessage);
        }

        private void HandleSignInResponse(WSFederationMessage responseMessageFromIssuer)
        {
            var contextId = responseMessageFromIssuer.Context;

            var ctxCookie = this.Request.Cookies[contextId];
            if (ctxCookie == null)
            {
                throw new InvalidOperationException("Context cookie not found");
            }

            var originalRequestUri = new Uri(ctxCookie.Value);
            this.DeleteContextCookie(contextId);

            var requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(originalRequestUri);

            SecurityTokenService sts =
                new FederationSecurityTokenService(FederationSecurityTokenServiceConfiguration.Current);
            SignInResponseMessage responseMessage =
                FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, this.User, sts);
            FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, this.Response);
        }
    }
}