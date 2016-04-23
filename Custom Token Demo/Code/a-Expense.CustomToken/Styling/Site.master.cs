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
    using Microsoft.IdentityModel.Claims;
    using Samples.Web.ClaimsUtilities;
    using ClaimTypes = System.IdentityModel.Claims.ClaimTypes;

    public partial class Site : MasterPage
    {
        protected Claim NameClaim
        {
            get { return ClaimHelper.GetCurrentUserClaim(ClaimTypes.Name); }
        }

        protected void OnFederatedPassiveSignInStatusSignedOut(object sender, EventArgs e)
        {
            this.Session.Abandon();
        }
    }
}