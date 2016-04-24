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
    using System.Collections.Generic;
    using System.Configuration;

    public class TrustedIssuers
    {
        private readonly string acsIssuerEndpoint;
        private readonly string acsIssuerName;
        private readonly Dictionary<string, IssuerInfo> issuers;

        static TrustedIssuers()
        {
            Instance = new TrustedIssuers();
        }

        private TrustedIssuers()
        {
            this.issuers = new Dictionary<string, IssuerInfo>();
            string acsServiceNamespace = ConfigurationManager.AppSettings["AcsServiceNamespace"];
            this.acsIssuerEndpoint = string.Format("https://{0}.accesscontrol.windows.net/v2/wsfederation", acsServiceNamespace);
            this.acsIssuerName = string.Format("https://{0}.accesscontrol.windows.net/", acsServiceNamespace);

            // Litware
            string homeRealmIdentifier = ConfigurationManager.AppSettings["LitwareIssuerIdentifier"];
            string issuerLocation = ConfigurationManager.AppSettings["LitwareIssuerLocation"];
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation));


            // Adatum
            homeRealmIdentifier = ConfigurationManager.AppSettings["AdatumIssuerIdentifier"];
            issuerLocation = ConfigurationManager.AppSettings["AdatumIssuerLocation"];
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation));

            // Hotmail
            homeRealmIdentifier = "hotmail.com";
            issuerLocation = this.AcsIssuerEndpoint;
            string whr = "uri:WindowsLiveID";
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation, whr));

            // Live
            homeRealmIdentifier = "live.com";
            issuerLocation = this.AcsIssuerEndpoint;
            whr = "uri:WindowsLiveID";
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation, whr));

            // Google
            homeRealmIdentifier = "gmail.com";
            issuerLocation = this.AcsIssuerEndpoint;
            whr = "Google";
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation, whr));

            // Facebook
            homeRealmIdentifier = "facebook.com";
            issuerLocation = this.AcsIssuerEndpoint;
            whr = "Facebook-596390297202303";
            this.issuers.Add(homeRealmIdentifier, new IssuerInfo(homeRealmIdentifier, issuerLocation, whr));
        }

        public static TrustedIssuers Instance { get; private set; }

        public string AcsIssuerEndpoint
        {
            get { return this.acsIssuerEndpoint; }
        }

        public string AcsIssuerName
        {
            get { return this.acsIssuerName; }
        }

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