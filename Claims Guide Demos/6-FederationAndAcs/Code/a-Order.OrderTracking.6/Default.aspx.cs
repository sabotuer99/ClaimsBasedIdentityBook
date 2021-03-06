//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder
{
    using System.Web.UI;
    using Samples.Web.ClaimsUtilities;

    public partial class Default : Page
    {
        protected override void OnLoad(System.EventArgs e)
        {
            if (!this.User.IsInRole(Adatum.Roles.OrderTracker))
            {
                this.Response.Redirect("AccessDenied.aspx", false);
            }

            base.OnLoad(e);
        }
    }
}