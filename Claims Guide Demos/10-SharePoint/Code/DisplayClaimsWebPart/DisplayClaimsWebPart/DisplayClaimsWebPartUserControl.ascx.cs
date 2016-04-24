//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DisplayClaimsWebPart.DisplayClaimsWebPart
{
    using System;
    using System.Web.UI;

    using Microsoft.IdentityModel.Claims;

    public partial class DisplayClaimsWebPartUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var claimsPrincipal = Page.User as IClaimsPrincipal;
            if (claimsPrincipal != null)
            {
                IClaimsIdentity claimsIdentity = (IClaimsIdentity)claimsPrincipal.Identity;
                ClaimsGridView.DataSource = claimsIdentity.Claims;
                Page.DataBind();
            }
        }
    }
}
