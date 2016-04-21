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
    using System;
    using System.Globalization;
    using System.Web.UI;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Microsoft.IdentityModel.Web;
    using System.IO;

    public partial class SimulatedWindowsAuthentication : Page
    {
        protected void ContinueButtonClick(object sender, EventArgs e)
        {
            SimulatedWindowsAuthenticationOperations.LogOnUser(this.UserList.SelectedValue, this.Context, this.Request, this.Response);
            this.HandleSignInRequest();
        }

        protected override void OnLoad(EventArgs e)
        {
            string action = this.Request.QueryString[WSFederationConstants.Parameters.Action];

            if (action == WSFederationConstants.Actions.SignIn)
            {
                // Process signin request.
                if (SimulatedWindowsAuthenticationOperations.TryToAuthenticateUser(this.Context, this.Request, this.Response))
                {
                    SecurityTokenService sts = new IdentityProviderSecurityTokenService(IdentityProviderSecurityTokenServiceConfiguration.Current);
                    var requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(this.Request.Url);
                    SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, this.User, sts);
                    FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, this.Response);
                    var response = this.Response;
                }
            }
            else if (action == WSFederationConstants.Actions.SignOut || action == WSFederationConstants.Actions.SignOutCleanup)
            {
                // Process signout request in the SignOut page. We do this because we have different styling for signout vs signin
                this.Response.Redirect("~/SignOut.aspx?" + this.Request.QueryString, false);
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

        private void HandleSignInRequest()
        {
            var requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(this.Request.Url);
            if (this.User != null && this.User.Identity.IsAuthenticated)
            {
                SecurityTokenService sts = new IdentityProviderSecurityTokenService(IdentityProviderSecurityTokenServiceConfiguration.Current);
                
                //This is the actual claims token
                //SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, this.User, sts);
                String realm = this.Request.Params["wtrealm"];
                String token = File.ReadAllText(@"C:\Users\troy\projects\FederatedDemos\Custom Token Demo\Code\Adatum.SimulatedIssuer.1\Token.xml");

                SignInResponseMessage responseMessage = new SignInResponseMessage(new Uri(realm), token);
                    



                FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, this.Response);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}