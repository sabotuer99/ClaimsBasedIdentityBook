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
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    public static class X509Helper
    {
        /// <summary>
        /// Private Utility method to get a certificate from a given store
        /// </summary>
        /// <param name="storeName">Name of certificate store (e.g. My, TrustedPeople)</param>
        /// <param name="storeLocation">Location of certificate store (e.g. LocalMachine, CurrentUser)</param>
        /// <param name="subjectDistinguishedName">The Subject Distinguished Name of the certificate</param>
        /// <returns>The specified X509 certificate</returns>
        static X509Certificate2 LookupCertificate( StoreName storeName, StoreLocation storeLocation, string subjectDistinguishedName )
        {
            X509Store store = null;
            X509Certificate2Collection certs = null;
            X509Certificate2 certificate = null;
            try
            {
                store = new X509Store( storeName, storeLocation );
                store.Open( OpenFlags.ReadOnly );
                certs = store.Certificates.Find( X509FindType.FindBySubjectDistinguishedName,
                                                                           subjectDistinguishedName, false );
                if ( certs.Count != 1 )
                {
                    throw new X509HelperException( String.Format( "FedUtil: Certificate {0} not found or more than one certificate found", subjectDistinguishedName ) );
                }
                certificate = new X509Certificate2( certs[0] );
                return certificate;
            }
            finally
            {
                if ( certs != null )
                {
                    for ( int i = 0; i < certs.Count; ++i )
                    {
                        certs[i].Reset();
                    }
                }
                if ( store != null ) store.Close();
            }
        }

        /// <summary>
        /// Public Utility method to get a X509 Certificate from a given store
        /// </summary>
        /// <param name="storeName">Name of certificate store (e.g. My, TrustedPeople)</param>
        /// <param name="storeLocation">Location of certificate store (e.g. LocalMachine, CurrentUser)</param>
        /// <param name="subjectDistinguishedName">The Subject Distinguished Name of the certificate</param>
        /// <returns>The specified X509 certificate</returns>
        public static X509Certificate2 GetX509Certificate2(StoreName storeName, StoreLocation storeLocation, string subjectDistinguishedName)
        {
            return LookupCertificate(storeName, storeLocation, subjectDistinguishedName);
        }


        #region GetX509TokenFromCert()
        /// <summary>
        /// Utility method to get a X509 Token from a given certificate
        /// </summary>
        /// <param name="storeName">Name of certificate store (e.g. My, TrustedPeople)</param>
        /// <param name="storeLocation">Location of certificate store (e.g. LocalMachine, CurrentUser)</param>
        /// <param name="subjectDistinguishedName">The Subject Distinguished Name of the certificate</param>
        /// <returns>The corresponding X509 Token</returns>
        public static X509SecurityToken GetX509TokenFromCert( StoreName storeName,
                                                              StoreLocation storeLocation,
                                                              string subjectDistinguishedName )
        {
            X509Certificate2 certificate = LookupCertificate( storeName, storeLocation, subjectDistinguishedName );
            X509SecurityToken t = new X509SecurityToken( certificate );
            return t;
        }
        #endregion

        #region GetCertificateThumbprint
        /// <summary>
        /// Utility method to get an X509 Certificate thumbprint
        /// </summary>
        /// <param name="storeName">Name of certificate store (e.g. My, TrustedPeople)</param>
        /// <param name="storeLocation">Location of certificate store (e.g. LocalMachine, CurrentUser)</param>
        /// <param name="subjectDistinguishedName">The Subject Distinguished Name of the certificate</param>
        /// <returns>The corresponding X509 Certificate thumbprint</returns>
        public static byte[] GetCertificateThumbprint( StoreName storeName, StoreLocation storeLocation, string subjectDistinguishedName )
        {
            X509Certificate2 certificate = LookupCertificate( storeName, storeLocation, subjectDistinguishedName );
            return certificate.GetCertHash();
        }

        #endregion

    }

}


