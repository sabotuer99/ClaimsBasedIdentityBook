//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Fabrikam.FederationProvider
{
    using System.Collections.Generic;
    using System.Configuration;

    public class FederationIssuers
    {
        private readonly Dictionary<string, IssuerInfo> issuers;

        static FederationIssuers()
        {
            Instance = new FederationIssuers();
        }

        private FederationIssuers()
        {
            this.issuers = new Dictionary<string, IssuerInfo>();

            // Litware
            string homeRealmIdentifier = ConfigurationManager.AppSettings["LitwareIssuerIdentifier"];
            string issuerLocation = ConfigurationManager.AppSettings["LitwareIssuerLocation"];
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation));

            // Adatum
            homeRealmIdentifier = ConfigurationManager.AppSettings["AdatumIssuerIdentifier"];
            issuerLocation = ConfigurationManager.AppSettings["AdatumIssuerLocation"];
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation));

            // Fabrikam
            homeRealmIdentifier = ConfigurationManager.AppSettings["FabrikamSimpleIssuerIdentifier"];
            issuerLocation = ConfigurationManager.AppSettings["FabrikamSimpleIssuerLocation"];
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation));
        }

        public static FederationIssuers Instance { get; private set; }

        public IssuerInfo this[string realm]
        {
            get { return this.issuers[realm]; }
        }

        public bool IsValidRealm(string realm)
        {
            return this.issuers.ContainsKey(realm);
        }
    }
}