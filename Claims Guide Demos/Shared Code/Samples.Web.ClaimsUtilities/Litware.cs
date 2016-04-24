//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Samples.Web.ClaimsUtilities
{
    public static class Litware
    {
        public static string OrganizationName
        {
            get { return "Litware"; }
        }

        public static class ClaimTypes
        {
            public static readonly string CostCenter = "http://schemas.litware.com/claims/2009/08/costcenter";
        }

        public static class CostCenters
        {
            public static readonly string Sales = "Litware-Sales";
        }

        public static class Groups
        {
            public static readonly string Sales = "Sales";
            public static readonly string SalesManager = "Sales Manager";
        }
    }
}