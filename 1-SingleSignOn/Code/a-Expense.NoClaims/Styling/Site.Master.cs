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
    using System.Web.UI;

    public partial class Site : MasterPage
    {
        protected void LoginStatusOnLoggedOut(object sender, EventArgs e)
        {
            this.Session.Abandon();
        }
    }
}