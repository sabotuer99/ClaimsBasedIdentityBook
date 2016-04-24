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
    using System;
    using System.Web.UI;
    using Samples.Web.ClaimsUtilities;

    public partial class Default : Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.OrdersGrid.Visible = !this.User.IsInRole(Adatum.Roles.OrderApprover);
            this.OrdersGridForApprovers.Visible = this.User.IsInRole(Adatum.Roles.OrderApprover);
        }
    }
}