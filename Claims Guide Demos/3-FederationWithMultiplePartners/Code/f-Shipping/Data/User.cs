//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping.Data
{
    public class User
    {
        public string Address { get; set; }
        public string CostCenter { get; set; }
        public string FullName { get; set; }

        public string Organization { get; set; }

        public ShipmentServiceType PreferredShippingServiceType { get; set; }
        public string UserName { get; set; }
    }
}