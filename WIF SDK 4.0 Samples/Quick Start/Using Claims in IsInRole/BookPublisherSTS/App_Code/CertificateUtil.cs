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
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// A utility class which helps to retrieve an x509 certificate
/// </summary>
public class CertificateUtil
{
    /// <summary>
    /// Gets a X.509 Certificate from the store.
    /// </summary>
    /// <param name="name">StoreName</param>
    /// <param name="location">StoreLocation</param>
    /// <param name="subjectName">X.509 Distinguished Name of the Certificate.</param>
    /// <returns>Instance of X509Certificate2.</returns>
    /// <exception cref="ApplicationException">No Certificate was found that matches the given
    /// criteria.</exception>
    public static X509Certificate2 GetCertificate( StoreName name, StoreLocation location, string subjectName )
    {
        X509Store store = new X509Store( name, location );
        X509Certificate2Collection certificates = null;
        store.Open( OpenFlags.ReadOnly );

        try
        {
            X509Certificate2 result = null;

            //
            // Every time we call store.Certificates property, a new collection will be returned.
            //
            certificates = store.Certificates;

            for ( int i = 0; i < certificates.Count; i++ )
            {
                X509Certificate2 cert = certificates[i];

                if ( cert.SubjectName.Name.ToLower() == subjectName.ToLower() )
                {
                    if ( result != null )
                    {
                        throw new ApplicationException( string.Format( "There are more than one certificate was found for subject Name {0}", subjectName ) );
                    }

                    result = new X509Certificate2( cert );
                }
            }

            if ( result == null )
            {
                throw new ApplicationException( string.Format( "No certificate was found for subject Name {0}", subjectName ) );
            }

            return result;
        }
        finally
        {
            if ( certificates != null )
            {
                for ( int i = 0; i < certificates.Count; i++ )
                {
                    X509Certificate2 cert = certificates[i];
                    cert.Reset();
                }
            }

            store.Close();
        }
    }
}
