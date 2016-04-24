//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.Data
{
    using System;

    public class AuditTrail
    {
        public string Action { get; set; }
        public Guid Id { get; set; }

        public Order Order { get; set; }

        public string Principal { get; set; }

        public DateTime Timestamp { get; set; }
    }
}