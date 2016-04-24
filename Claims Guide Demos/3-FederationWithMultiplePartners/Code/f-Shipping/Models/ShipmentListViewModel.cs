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
    using System.Collections.Generic;
    using FShipping.Data;

    public class ShipmentListViewModel : MasterPageViewModel
    {
        public IEnumerable<Shipment> Shipments { get; set; }
    }
}