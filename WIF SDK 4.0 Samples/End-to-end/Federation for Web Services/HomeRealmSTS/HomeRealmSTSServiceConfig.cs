//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------


using System;
using System.Configuration;
using System.Security.Cryptography.X509Certificates;
using Microsoft.IdentityModel.Protocols.WSTrust;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    /// <summary>
    /// Application configuration for the HomeReal SecurityTokenService.
    /// </summary>
    public static class HomeRealmSTSServiceConfig
    {
        // Issuer name placed into issued tokens
        internal const string StsName = "Home Realm STS";

        // Statics for location of certs
        internal static StoreName CertStoreName = StoreName.My;
        internal static StoreLocation CertStoreLocation = StoreLocation.LocalMachine;

        // Statics initialized from app.config
        internal static string CertDistinguishedName;
        internal static string TargetDistinguishedName;
        internal static double PurchaseLimit;
        internal static string IssuerAddress;
        internal static string ExpectedAppliesToURI;

        #region Helper functions to load app settings from config
        /// <summary>
        /// Helper function to load Application Settings from config
        /// </summary>
        public static void LoadAppSettings()
        {
            CertDistinguishedName = ConfigurationManager.AppSettings["certDistinguishedName"];
            CheckIfLoaded( CertDistinguishedName );

            TargetDistinguishedName = ConfigurationManager.AppSettings["targetDistinguishedName"];
            CheckIfLoaded( TargetDistinguishedName );

            IssuerAddress = ConfigurationManager.AppSettings["issuerAddress"];
            CheckIfLoaded( IssuerAddress );

            ExpectedAppliesToURI = ConfigurationManager.AppSettings["expectedAppliestoURI"];
            CheckIfLoaded( ExpectedAppliesToURI );

            string purchaseLimitString = ConfigurationManager.AppSettings["purchaseLimit"];
            CheckIfLoaded( purchaseLimitString );
            PurchaseLimit = Double.Parse( purchaseLimitString );
        }

        /// <summary>
        /// Helper function to check if a required Application Setting has been specified in config.
        /// Throw if some Application Setting has not been specified.
        /// </summary>
        private static void CheckIfLoaded( string s )
        {
            if ( String.IsNullOrEmpty( s ) )
            {
                throw new ConfigurationErrorsException( "Required Configuration Element(s) missing at HomeRealmSTS. Please check the STS configuration file." );
            }
        }

        #endregion

        #region constructors

        static HomeRealmSTSServiceConfig()
        {
            LoadAppSettings();
        }

        #endregion
    }
}
