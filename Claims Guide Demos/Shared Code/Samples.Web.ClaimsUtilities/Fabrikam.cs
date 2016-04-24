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
    public static class Fabrikam
    {
        public static string OrganizationName
        {
            get { return "Fabrikam"; }
        }

        public static class ClaimTypes
        {
            public static readonly string CostCenter = "http://schemas.fabrikam.com/claims/2009/08/costcenter";
            public static readonly string Organization = "http://schemas.fabrikam.com/claims/2009/08/organization";
            public static readonly string IdentityProvider = "http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider";
        }

        public static class ClaimValues
        {
            public static readonly string SingleCostCenter = "Single Cost center";
        }

        public static class Roles
        {
            public static readonly string Administrator = "Administrator";
            public static readonly string ShipmentCreator = "Shipment Creator";
            public static readonly string ShipmentManager = "Shipment Manager";
        }
    }
}