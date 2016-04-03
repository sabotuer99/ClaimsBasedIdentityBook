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
    public static class Adatum
    {
        public static string OrganizationName
        {
            get { return "Adatum"; }
        }

        public static class ClaimTypes
        {
            public static readonly string CostCenter = "http://schemas.adatum.com/claims/2009/08/costcenter";
            public static readonly string Organization = "http://schemas.adatum.com/claims/2009/08/organization";
        }

        public static class CostCenters
        {
            public static readonly string Accountancy = "CC-4567-ACCOUNTANCY";
            public static readonly string CustomerService = "CC-1234-CUSTOMERSERVICE";
        }

        public static class Groups
        {
            public static readonly string CustomerService = "Customer Service";
            public static readonly string ItAdmins = "IT Admins";
            public static readonly string OrderFulfillments = "Order Fulfillments";
        }

        public static class Roles
        {
            public static readonly string Accountant = "Accountant";
            public static readonly string Employee = "Employee";
            public static readonly string OrderApprover = "Order Approver";
            public static readonly string OrderTracker = "Order Tracker";
        }
    }
}