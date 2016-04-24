//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================





namespace AExpense
{
    using System;
    using System.Collections.Generic;
    using Data;
    using Samples.Web.ClaimsUtilities;

    public partial class Default : BasePage
    {
        protected override IEnumerable<string> AuthorizedRoles
        {
            get
            {
                return new List<string> { Adatum.Roles.Employee, Adatum.Roles.Accountant };
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            var user = (User)Session["LoggedUser"];

            var repository = new ExpenseRepository();
            var expenses = repository.GetExpenses(user.Id);
            this.MyExpensesGridView.DataSource = expenses;
            this.DataBind();

            base.OnLoad(e);
        }
    }
}