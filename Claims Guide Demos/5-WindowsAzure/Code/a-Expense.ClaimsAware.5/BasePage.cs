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
    using System.Linq;
    using System.Web.UI;
    using AExpense.Data;

    public abstract class BasePage : Page
    {
        protected abstract IEnumerable<string> AuthorizedRoles { get; }

        protected override void OnLoad(EventArgs e)
        {
            this.Authorize();

            base.OnLoad(e);
        }

        // The following lines are part of the authorization logic.
        // In this legacy application, the authorization logic is spread
        // across the source code, in the aspx pages and the database.
        private void Authorize()
        {
            var user = (User)this.Session["LoggedUser"];

            if (user == null)
            {
                throw new InvalidOperationException("User session was not created");
            }

            if (user.Roles.Where(r => this.AuthorizedRoles.Contains(r.Name)).Count() == 0)
            {
                this.Response.Redirect("~/AccessDenied.aspx");
                return;
            }
        }
    }
}