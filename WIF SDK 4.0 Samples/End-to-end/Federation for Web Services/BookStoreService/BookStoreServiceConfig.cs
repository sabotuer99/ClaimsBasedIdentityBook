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
    public static class BookStoreServiceConfig
    {
        #region BookStore Service-Wide Constants

        // The following two Action strings are the defaults created by the OperationContract attribute
        internal const string BrowseBooksAction = "http://tempuri.org/IBrowseBooks/BrowseBooks";
        internal const string BuyBookAction = "http://tempuri.org/IBuyBook/BuyBook";

        // Statics for location of certs
        internal static StoreName CertStoreName = StoreName.My;
        internal static StoreLocation CertStoreLocation = StoreLocation.LocalMachine;

        // Statics initialized from web.config
        internal static string BookDB;
        internal static string IssuerCertDistinguishedName;

        #endregion

        #region Helper functions to load app settings from config

        /// <summary>
        /// Helper function to load Application Settings from config
        /// </summary>
        public static void LoadAppSettings()
        {
            BookDB = ConfigurationManager.AppSettings["bookDB"];
            CheckIfLoaded(BookDB);
            BookDB = String.Format("{0}\\{1}", System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath, BookDB);

            IssuerCertDistinguishedName = ConfigurationManager.AppSettings["issuerCertDistinguishedName"];
            CheckIfLoaded(IssuerCertDistinguishedName);
        }

        /// <summary>
        /// Helper function to check if a required Application Setting has been specified in config.
        /// Throw if some Application Setting has not been specified.
        /// </summary>
        private static void CheckIfLoaded(string s)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new ConfigurationErrorsException("Required Configuration Element(s) missing at BookStoreService. Please check the service configuration file.");
            }
        }

        #endregion

    }
}
