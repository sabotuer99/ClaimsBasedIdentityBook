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

using Microsoft.Web.Administration;
using System;

class IIS7Util
{
    static string usageString = "Usage: IIS7Util.exe SetSslCert|DeleteSslCert <CertificateHash>";

    static void Main( string[] args )
    {
        if ( args.Length == 2 && args[0].Equals( "SetSslCert", StringComparison.OrdinalIgnoreCase ) )
        {
            SetSslCert( args[1] );
        }
        else if ( args.Length == 2 && args[0].Equals( "DeleteSslCert", StringComparison.OrdinalIgnoreCase ) )
        {
            DeleteSslCert( args[1] );
        }
        else
        {
            Console.WriteLine( usageString );
            Environment.Exit( 1 );
        }
    }

    static void SetSslCert( string requestedCertHashString )
    {
        if ( String.IsNullOrEmpty( requestedCertHashString ) )
        {
            throw new ArgumentException( "requestedCertHashString" );
        }
        try
        {
            ServerManager serverManager = new ServerManager();
            Site defaultWebSite = serverManager.Sites["Default Web Site"];
            if ( defaultWebSite == null )
            {
                throw new Exception( "Unable to obtain a reference to the Default Web Site." );
            }
            if ( defaultWebSite.Bindings == null )
            {
                throw new Exception( "The binding collection on the Default Web Site was null." );
            }
            Binding sslBinding = null;
            foreach ( Binding binding in defaultWebSite.Bindings )
            {
                if ( binding.Protocol.Equals( "https", StringComparison.OrdinalIgnoreCase ) )
                {
                    sslBinding = binding;
                    break;
                }
            }
            if ( sslBinding == null )
            {
                Console.WriteLine( "No SSL binding found on the Default Web Site. Creating one..." );
                defaultWebSite.Bindings.Add( "*:443:", "https" );
                foreach ( Binding binding in defaultWebSite.Bindings )
                {
                    if ( binding.Protocol.Equals( "https", StringComparison.OrdinalIgnoreCase ) )
                    {
                        sslBinding = binding;
                        break;
                    }
                }
            }
            if ( sslBinding == null )
            {
                throw new Exception( "Unable to get sslBinding" );
            }

            Console.WriteLine( "Setting up the IIS 7 SSL Certificate configuration..." );
            sslBinding.CertificateHash = HexStringToByteArray( requestedCertHashString );
            sslBinding.CertificateStoreName = "MY";
            serverManager.CommitChanges();
        }
        catch ( Exception e )
        {
            Console.WriteLine( "Encountered an error when attempting to set up the IIS 7 SSL Certificate configuration." );
            Console.WriteLine( "Error Detail:" );
            Console.WriteLine( e.Message );
            Environment.Exit( 1 );
        }

    }

    private static byte[] HexStringToByteArray( string requestedCertHashString )
    {
        if ( String.IsNullOrEmpty( requestedCertHashString ) )
        {
            throw new ArgumentException( "requestedCertHashString" );
        }
        int hexLength = requestedCertHashString.Length;
        if ( hexLength % 2 != 0 )
        {
            throw new Exception( requestedCertHashString + " does not have an even number of characters." );
        }
        byte[] certBytes = new byte[hexLength / 2];
        for ( int i = 0; i < hexLength / 2; i++ )
        {
            certBytes[i] = byte.Parse( requestedCertHashString.Substring( 2 * i, 2 ), System.Globalization.NumberStyles.HexNumber );
        }
        return certBytes;
    }

    static void DeleteSslCert( string requestedCertHashString )
    {
        if ( String.IsNullOrEmpty( requestedCertHashString ) )
        {
            throw new ArgumentException( "requestedCertHashString" );
        }
        try
        {
            ServerManager serverManager = new ServerManager();
            Site defaultWebSite = serverManager.Sites["Default Web Site"];
            if ( defaultWebSite == null )
            {
                throw new Exception( "Unable to obtain a reference to the Default Web Site." );
            }
            if ( defaultWebSite.Bindings == null )
            {
                throw new Exception( "The binding collection on the Default Web Site was null." );
            }
            Binding sslBinding = null;
            foreach ( Binding binding in defaultWebSite.Bindings )
            {
                if ( binding.Protocol.Equals( "https", StringComparison.OrdinalIgnoreCase ) )
                {
                    sslBinding = binding;
                    break;
                }
            }
            if ( sslBinding == null )
            {
                throw new Exception( "No SSL binding found on the Default Web Site." );
            }
            byte[] existingCertHash = sslBinding.CertificateHash;

            // if no existing ssl cert is found, exit
            if ( existingCertHash == null )
            {
                Console.WriteLine( "WARNING: Site does not have any certificate registered." );
                Console.WriteLine( "Nothing has changed for the SSL configuration." );
                Environment.Exit( 1 );
            }

            string existingCertHashString = BitConverter.ToString( existingCertHash ).Replace( "-", "" );

            // if existing ssl cert does not match requested cert, exit
            if ( !requestedCertHashString.Equals( existingCertHashString, StringComparison.OrdinalIgnoreCase ) )
            {
                Console.WriteLine( "WARNING: Site already has a different certificate registered" );
                Console.WriteLine( "Existing Certificate hash: " + existingCertHashString );
                Console.WriteLine( "Requested Certificate hash: " + requestedCertHashString );
                Console.WriteLine( "Nothing has changed for the SSL configuration." );
                Environment.Exit( 1 );
            }

            // we have a match. clear the ssl cert configuration
            Console.WriteLine( "Deleting the IIS 7 SSL Certificate configuration..." );
            sslBinding.CertificateHash = null;
            sslBinding.CertificateStoreName = String.Empty;
            serverManager.CommitChanges();
        }
        catch ( Exception e )
        {
            Console.WriteLine( "Encountered an error when attempting to delete the IIS 7 SSL Certificate configuration." );
            Console.WriteLine( "Error Detail:" );
            Console.WriteLine( e.Message );
            Environment.Exit( 1 );
        }
    }
}
