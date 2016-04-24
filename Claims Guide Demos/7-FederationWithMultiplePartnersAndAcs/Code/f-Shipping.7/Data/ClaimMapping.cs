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
    public class ClaimMapping
    {
        public string IncomingClaimType { get; set; }

        public string IncomingValue { get; set; }

        public string Organization { get; set; }
        public Role OutputRole { get; set; }
    }
}