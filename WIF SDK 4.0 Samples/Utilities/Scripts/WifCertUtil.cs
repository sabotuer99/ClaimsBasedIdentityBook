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
using System.IO;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

class WifCertUtil
{
    static string usageString = "Usage: WifCertUtil.exe [GetCertThumbprint <storeName> [<*.cer> | <subjectName>]] | [GetCertKeyContainer <storeName> <hash> [-user]] ";

    static void Main( string[] args )
    {
        if ( args.Length == 3 && args[0].Equals( "GetCertThumbprint", StringComparison.OrdinalIgnoreCase ) )
        {
            GetCertThumbprint( args[1], args[2] );
        }
        else if ( args.Length == 3 && args[0].Equals( "GetCertKeyContainer", StringComparison.OrdinalIgnoreCase ) )
        {
            GetCertKeyContainer( args[1], args[2] );
        }
        else if ( args.Length == 4 && args[0].Equals( "GetCertKeyContainer", StringComparison.OrdinalIgnoreCase ) && args[3].Equals( "-user", StringComparison.OrdinalIgnoreCase) )
        {
            GetCertKeyContainer( args[1], args[2], StoreLocation.CurrentUser );
        }
        else
        {
            Console.WriteLine( usageString );
            Environment.Exit( 1 );
        }
    }

    static void GetCertThumbprint( string storeName, string certFileOrSubject )
    {
        if ( String.IsNullOrEmpty( storeName ) )
        {
            throw new ArgumentException( "The storeName parameter is null or empty.", "storeName" );
        }
        if ( String.IsNullOrEmpty( certFileOrSubject ) )
        {
            throw new ArgumentException( "The certFileOrSubject parameter is null or empty.", "certFileOrSubject" );
        }

        try
        {
            X509Certificate2 certificate = GetCertificateFromSubject( storeName, certFileOrSubject );
            if ( certificate == null )
            {
                certificate = GetCertificateFromFileName( certFileOrSubject );                
            }

            if ( certificate != null )
            {
                Console.WriteLine( certificate.Thumbprint );
            }
            else
            {
                Environment.Exit( 1 );
            }
        }
        catch ( Exception e )
        {
            Console.WriteLine( "Encountered an error when attempting to locate certificate thumbprint." );
            Console.WriteLine( "Error Detail:" );
            Console.WriteLine( e.Message );
            Environment.Exit( 1 );
        }
    }

    static void GetCertKeyContainer( string storeName, string thumbprint )
    {
        GetCertKeyContainer( storeName, thumbprint, StoreLocation.LocalMachine );
    }

    static void GetCertKeyContainer( string storeName, string thumbprint, StoreLocation storeLocation )
    {
        if ( String.IsNullOrEmpty( thumbprint ) )
        {
            throw new ArgumentException( "The thumbprint parameter is null or empty.", "thumbprint" );
        }

        try
        {
            Console.WriteLine( GetCertificateKeyFile( storeName, storeLocation, thumbprint ) );
        }
        catch ( Exception e )
        {
            Console.WriteLine( "Encountered an error when attempting to locate certificate key container." );
            Console.WriteLine( "Error Detail:" );
            Console.WriteLine( e.Message );
            Environment.Exit( 1 );
        }
    }

    /// <summary>
    /// Obtains a certificate from a CER file.
    /// </summary>
    /// <param name="fileName">File to read.</param>
    static X509Certificate2 GetCertificateFromFileName( string fileName )
    {
        X509Certificate2 cert = null;
        try
        {
            cert = new X509Certificate2( fileName );
        }
        catch ( CryptographicException )
        {
        }

        return cert;
    }

     /// <summary>
    /// Obtains a certificate from a given store and subject.
    /// </summary>
    /// <param name="storeName">Store name to look in.</param>
    /// <param name="subject">Subject name.</param>
    /// <returns></returns>
    static X509Certificate2 GetCertificateFromSubject( string storeName, string subject )
    {
        X509Store store = null;
        X509Certificate2 cert = null;
        X509Certificate2Collection certificates = null;
        try
        {
            store = new X509Store( storeName, StoreLocation.LocalMachine );
            store.Open( OpenFlags.ReadOnly );
            certificates = store.Certificates.Find( X509FindType.FindBySubjectDistinguishedName, "CN=" + subject, false );
            if ( certificates.Count > 0 )
            {
                cert = new X509Certificate2( certificates[0] );
            }
        }
        finally
        {
            if ( certificates != null )
            {
                for ( int i = 0; i < certificates.Count; i++ )
                {
                    X509Certificate2 certificate = certificates[i];
                    certificate.Reset();
                }
            }
            if ( store != null )
            {
                store.Close();
            }
        }

        return cert;
    }

    /// <summary>
    /// Get the certificate's private key file name.
    /// </summary>
    /// <param name="name">Certificate store</param>
    /// <param name="location">Certificate location</param>
    /// <param name="subjectName">Certificate SubjectDistinguishedName</param>
    /// <returns></returns>
    static string GetCertificateKeyFile( string storeName, StoreLocation storeLocation, string thumbprint )
    {
        X509Certificate2 certificate = GetCertificate( storeName, storeLocation, thumbprint, false );
        if ( certificate != null )
        {
            return GetKeyFileName( certificate );
        }

        Console.WriteLine( "Cannot locate key container." );
        Environment.Exit( 1 );
        return null;
    }

    /// <summary>
    /// Get the ceritificate with the specified SubjectDistinguishedName from the store.
    /// </summary>
    /// <param name="name">Certificate store name.</param>
    /// <param name="location">Certificate store location.</param>
    /// <param name="subjectName">Certificate SubjectDistinguishedName.</param>
    /// <param name="validOnly">true to allow only valid certificates to be returned from the search; otherwise, false.</param>
    /// <returns>X509Certificate2 certificate object. null if the certificate could not be located in store.</returns>
    static X509Certificate2 GetCertificate( string storeName, StoreLocation storeLocation, string thumbprint, bool validOnly )
    {
        X509Store store = new X509Store( storeName, storeLocation );
        X509Certificate2Collection certificates = null;
        store.Open( OpenFlags.ReadOnly );

        try
        {
            X509Certificate2 result = null;

            certificates = store.Certificates.Find( X509FindType.FindByThumbprint, thumbprint, validOnly );

            if ( certificates.Count > 0 )
            {
                // always return the first cert found
                result = new X509Certificate2( certificates[0] );
            }

            return result;
        }
        finally
        {
            if ( certificates != null )
            {
                for ( int i = 0; i < certificates.Count; i++ )
                {
                    X509Certificate2 certificate = certificates[i];
                    certificate.Reset();
                }
            }
            if ( store != null )
            {
                store.Close();
            }
        }
    }

    /// <summary>
    /// Gets the certificate private key file name.
    /// </summary>
    /// <param name="cert">Certificate object</param>
    /// <returns>Private key file name</returns>
    static string GetKeyFileName( X509Certificate2 certificate )
    {
        string filename = null;

        if ( certificate.PrivateKey != null )
        {
            RSACryptoServiceProvider provider = certificate.PrivateKey as RSACryptoServiceProvider;
            filename = provider.CspKeyContainerInfo.UniqueKeyContainerName;
        }
        return filename;
    }

}
