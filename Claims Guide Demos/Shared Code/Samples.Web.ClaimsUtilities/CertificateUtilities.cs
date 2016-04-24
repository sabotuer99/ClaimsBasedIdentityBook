//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Samples.Web.ClaimsUtilities
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Permissions;

    public static class CertificateUtilities
    {
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static X509Certificate2 GetCertificate(StoreName name, StoreLocation location, string subjectName)
        {
            var store = new X509Store(name, location);
            X509Certificate2Collection certificates = null;
            store.Open(OpenFlags.ReadOnly);

            try
            {
                X509Certificate2 result = null;

                // Every time we call store.Certificates property, a new collection will be returned.
                certificates = store.Certificates;

                foreach (X509Certificate2 cert in certificates)
                {
                    if (cert.SubjectName.Name != null)
                    {
                        if (cert.SubjectName.Name.Equals(subjectName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (result != null)
                            {
                                throw new CryptographicException(string.Format(CultureInfo.CurrentUICulture, "There are multiple certificates for subject Name {0}", subjectName));
                            }

                            result = new X509Certificate2(cert);
                        }
                    }
                }

                if (result == null)
                {
                    throw new CryptographicException(string.Format(CultureInfo.CurrentUICulture, "No certificate was found for subject Name {0}", subjectName));
                }

                return result;
            }
            finally
            {
                if (certificates != null)
                {
                    foreach (X509Certificate2 cert in certificates)
                    {
                        cert.Reset();
                    }
                }

                store.Close();
            }
        }
    }
}