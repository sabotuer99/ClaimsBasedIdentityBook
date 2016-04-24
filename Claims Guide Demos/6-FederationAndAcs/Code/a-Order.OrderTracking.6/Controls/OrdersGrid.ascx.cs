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
    using AOrder.Data;
    using Samples.Web.ClaimsUtilities;

    public partial class OrdersGrid : UserControl
    {
        protected override void OnLoad(EventArgs e)
        {
            var company = ClaimHelper.GetCurrentUserClaim(Adatum.ClaimTypes.Organization).Value;
            var orderRepository = new OrderRepository();
            this.OrdersGridView.DataSource = orderRepository.GetOrdersByCompanyName(company);
            this.OrdersGridView.DataBind();
        }
    }
}