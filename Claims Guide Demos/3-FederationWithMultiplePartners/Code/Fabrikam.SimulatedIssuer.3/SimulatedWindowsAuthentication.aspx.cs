//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Fabrikam.SimulatedIssuer
{
    using System;
    using System.Web.Security;
    using System.Web.UI;

    public partial class SimulatedWindowsAuthentication : Page
    {
        protected void OnContinueButtonClicked(object sender, EventArgs e)
        {
            if (this.Request.QueryString["ReturnUrl"] != null)
            {
                FormsAuthentication.RedirectFromLoginPage(this.UserName.Text, false);
            }
            else
            {
                FormsAuthentication.SetAuthCookie(this.UserName.Text, true);
                this.Response.Redirect("default.aspx", false);
            }
        }
    }
}