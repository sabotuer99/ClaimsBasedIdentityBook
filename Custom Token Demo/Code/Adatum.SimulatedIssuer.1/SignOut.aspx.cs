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

    public partial class SignOut : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            string action = this.Request.QueryString[WSFederationConstants.Parameters.Action];

            if (action == WSFederationConstants.Actions.SignOut || action == WSFederationConstants.Actions.SignOutCleanup)
            {
                // Process signout request.
                SimulatedWindowsAuthenticationOperations.LogOutUser(this.Request, this.Response);
                WSFederationMessage requestMessage = WSFederationMessage.CreateFromUri(this.Request.Url);
                FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(requestMessage, this.User, null, this.Response);
                this.ActionExplanationLabel.Text = @"Sign out from the issuer has been requested.";

                this.SignOutRelyingParties();

                SingleSignOnManager.Clear();
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

        private void SignOutRelyingParties()
        {
            var signedInUrls = SingleSignOnManager.SignOutRelyingParties();
            if (signedInUrls.Length > 0)
            {
                this.RelyingPartyLabel.Visible = true;
                foreach (string url in signedInUrls)
                {
                    this.RelyingPartySignOutLinks.Controls.Add(
                        new LiteralControl(string.Format("<p><a href='{0}'>{0}</a>&nbsp;<img src='{0}?wa=wsignoutcleanup1.0' title='Signout request: {0}?wa=wsignoutcleanup1.0'/></p>", url)));
                }
            }
        }
    }
}