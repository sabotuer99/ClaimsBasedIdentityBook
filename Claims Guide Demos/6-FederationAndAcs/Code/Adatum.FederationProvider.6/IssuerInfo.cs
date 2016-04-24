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
            this.IsSocialIssuer = false;
        }

        public IssuerInfo(string homeRealm, string issuerLocation, string whr)
            : this(homeRealm, issuerLocation)
        {
            this.IsSocialIssuer = true;
            this.Whr = whr;
        }

        public string HomeRealmIdentifier { get; private set; }
        public bool IsSocialIssuer { get; private set; }
        public string IssuerLocation { get; private set; }
        public string Whr { get; private set; }
    }
}