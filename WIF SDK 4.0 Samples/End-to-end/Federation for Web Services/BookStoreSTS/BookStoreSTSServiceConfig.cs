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

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    /// <summary>
    /// Custom application configuration for the BookStoreSTS.
    /// </summary>
    public static class BookStoreSTSServiceConfig
    {
        // Issuer name placed into issued tokens
        internal const string SecurityTokenServiceName = "BookStore STS";

        // Statics for location of certs
        internal static readonly StoreName CertStoreName = StoreName.My;
        internal static readonly StoreLocation CertStoreLocation = StoreLocation.LocalMachine;

        // Statics initialized from app.config
        internal static string CertDistinguishedName;
        internal static string TargetDistinguishedName;
        internal static string IssuerDistinguishedName;
        internal static string BookDB;
        internal static string IssuerAddress;
        internal static string ExpectedAppliesToURI;

        #region Helper functions to load app settings from config

        /// <summary>
        /// Helper function to load Application Settings from config
        /// </summary>
        public static void LoadAppSettings()
        {
            BookDB = ConfigurationManager.AppSettings["bookDB"];
            CheckIfLoaded(BookDB);
            BookDB = String.Format("{0}\\{1}", System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, BookDB);

            CertDistinguishedName = ConfigurationManager.AppSettings["certDistinguishedName"];
            CheckIfLoaded(CertDistinguishedName);

            TargetDistinguishedName = ConfigurationManager.AppSettings["targetDistinguishedName"];
            CheckIfLoaded(TargetDistinguishedName);

            IssuerDistinguishedName = ConfigurationManager.AppSettings["issuerDistinguishedName"];
            CheckIfLoaded(IssuerDistinguishedName);

            IssuerAddress = ConfigurationManager.AppSettings["issuerAddress"];
            CheckIfLoaded(IssuerAddress);

            ExpectedAppliesToURI = ConfigurationManager.AppSettings["expectedAppliestoURI"];
            CheckIfLoaded(ExpectedAppliesToURI);
        }

        /// <summary>
        /// Helper function to check if a required Application Setting has been specified in config.
        /// Throw if some Application Setting has not been specified.
        /// </summary>
        private static void CheckIfLoaded(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new ConfigurationErrorsException("Required Configuration Element(s) missing at BookStoreSTS. Please check the STS configuration file.");
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Creates an instance of BookStoreSTSServiceConfig.
        /// </summary>
        static BookStoreSTSServiceConfig()
        {
            LoadAppSettings();
        }

        #endregion
    }
}
