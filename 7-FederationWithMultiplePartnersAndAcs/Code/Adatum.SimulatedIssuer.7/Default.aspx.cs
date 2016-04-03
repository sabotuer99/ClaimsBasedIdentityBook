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
    using System.Configuration;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.Web;
    using Microsoft.IdentityModel.SecurityTokenService;

    public partial class Default : Page
    {
        protected override void OnPreRender(EventArgs e)
        {
            string action = this.Request.QueryString[WSFederationConstants.Parameters.Action];


            if (action == WSFederationConstants.Actions.SignIn)
            {
                // Process signin request.
                var requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(this.Request.Url);
                if (this.User != null && this.User.Identity.IsAuthenticated)
                {
                    SecurityTokenService sts = new IdentityProviderSecurityTokenService(IdentityProviderSecurityTokenServiceConfiguration.Current);
                    SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, this.User, sts);
                    FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, this.Response);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            else if (action == WSFederationConstants.Actions.SignOut)
            {
                // Process signout request.
                var requestMessage = (SignOutRequestMessage)WSFederationMessage.CreateFromUri(this.Request.Url);
                FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(requestMessage, this.User, null, this.Response);
                this.ActionExplanationLabel.Text = @"Sign out from the issuer has been requested.";
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
        }

    }
}