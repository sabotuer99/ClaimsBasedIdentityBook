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
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using AExpense.Data;

    public partial class Login : Page
    {
        protected void Login1OnAuthenticate(object sender, AuthenticateEventArgs e)
        {
            var repository = new UserRepository();

            if (!repository.ValidateUser(this.Login1.UserName, this.Login1.Password))
            {
                e.Authenticated = false;
                return;
            }

            var user = repository.GetUser(this.Login1.UserName);

            if (user != null)
            {
                this.Session["LoggedUser"] = user;
                e.Authenticated = true;
            }
        }
    }
}