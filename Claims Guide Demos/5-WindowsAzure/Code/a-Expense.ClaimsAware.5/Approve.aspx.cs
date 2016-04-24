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
    using System.Collections.Generic;
    using Samples.Web.ClaimsUtilities;

    public partial class Approve : BasePage
    {
        protected override IEnumerable<string> AuthorizedRoles
        {
            get { return new List<string> { Adatum.Roles.Accountant }; }
        }
    }
}