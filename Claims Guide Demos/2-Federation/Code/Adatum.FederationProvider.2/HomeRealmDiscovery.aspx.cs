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
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using Microsoft.IdentityModel.Protocols.WSFederation;

    public partial class HomeRealmDiscovery : Page
    {
        public void ProcessSignIn()
        {
            var whrIdentifier = HttpUtility.UrlEncode(this.TrustedIssuersDropDownList.Text);
            this.Response.Redirect("~/Federation.aspx?" + this.Request.QueryString + "&whr=" + whrIdentifier, false);
        }

        protected override void OnLoad(EventArgs e)
        {
            string action = this.Request.QueryString[WSFederationConstants.Parameters.Action];

            if (action != WSFederationConstants.Actions.SignIn)
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture, 
                        "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}'.", 
                        String.IsNullOrEmpty(action) ? "<EMPTY>" : action, 
                        WSFederationConstants.Parameters.Action, 
                        WSFederationConstants.Actions.SignIn));
            }

            base.OnLoad(e);
        }

        protected void OnSignInButtonClicked(object sender, EventArgs e)
        {
            this.ProcessSignIn();
        }
    }
}