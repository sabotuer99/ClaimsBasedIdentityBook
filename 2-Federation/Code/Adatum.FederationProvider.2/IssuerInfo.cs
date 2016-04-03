//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Adatum.FederationProvider
{
    public class IssuerInfo
    {
        public IssuerInfo(string homeRealmIdentifier, string issuerLocation)
        {
            this.HomeRealmIdentifier = homeRealmIdentifier;
            this.IssuerLocation = issuerLocation;
        }

        public string HomeRealmIdentifier { get; private set; }
        public string IssuerLocation { get; private set; }
    }
}