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
    using System.Security.Principal;
    using System.Web;
    using Samples.Web.ClaimsUtilities;

    public class Global : HttpApplication
    {
        private static IPrincipal MaryMay
        {
            get
            {
                IIdentity identity = new GenericIdentity("mary");
                string[] roles = { Adatum.Roles.Employee, Adatum.Roles.OrderApprover };
                return new GenericPrincipal(identity, roles);
            }
        }

        // In a production scenario, the user would be authenticated using Windows 
        // Integrated Authentication. This is becasue we don't want to force you to 
        // create new users on your local users and groups to experiment different logins.
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // Possible values for this.Context.User that will be logged in are:
            // - JohnDoe
            // * 'Employee' role
            // - MaryMay
            // * 'Employee' and 'Order approver' roles
            this.Context.User = MaryMay;
        }
    }
}