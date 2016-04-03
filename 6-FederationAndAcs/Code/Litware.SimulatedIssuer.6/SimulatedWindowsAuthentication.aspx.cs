//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Litware.SimulatedIssuer
{
    using System;
    using System.Web.Security;
    using System.Web.UI;

    public partial class SimulatedWindowsAuthentication : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            this.LoginUserNameLabel.Text = SimulatedActiveDirectory.UserName;

            base.OnLoad(e);
        }

        protected void OnContinueButtonClicked(object sender, EventArgs e)
        {
            if (this.Request.QueryString["ReturnUrl"] != null)
            {
                FormsAuthentication.RedirectFromLoginPage(SimulatedActiveDirectory.UserName, false);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(SimulatedActiveDirectory.UserName, true);
                this.Response.Redirect("default.aspx", false);
            }
        }
    }
}