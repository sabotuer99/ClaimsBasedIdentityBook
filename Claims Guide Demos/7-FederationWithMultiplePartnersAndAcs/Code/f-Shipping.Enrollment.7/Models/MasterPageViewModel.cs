//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping.Models
{
    public class MasterPageViewModel
    {
        public string ClaimsIssuer { get; set; }

        public string ClaimsOriginalIssuer { get; set; }
        public string TenantLogoPath { get; set; }
    }
}