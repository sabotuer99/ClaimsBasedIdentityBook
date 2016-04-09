//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//-----------------------------------------------------------------------------

using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Web.Hosting;

using Microsoft.IdentityModel.Web;

/// <summary>
/// A Windows Identity Foundation CookieTransform that uses key material provided
/// in the ASP.NET machineKeys configuration element to protect data.
/// </summary>
public class MachineKeyProtectionTransform : CookieTransform
{
    enum EncryptionMode
    {
        None,
        AES,
        DES,
        TripleDES,
    }

    byte[]         _encryptionKey;
    EncryptionMode _encryptionMode = EncryptionMode.None;

    public MachineKeyProtectionTransform()
    {
        MachineKeySection section;

        if ( HostingEnvironment.IsHosted )
        {
            if ( HostingEnvironment.ApplicationVirtualPath != null )
                section = (MachineKeySection) WebConfigurationManager.GetSection( "system.web/machineKey", HostingEnvironment.ApplicationVirtualPath );
            else
                section = (MachineKeySection) WebConfigurationManager.GetSection( "system.web/machineKey" );
        }
        else
        {
            section = (MachineKeySection) ConfigurationManager.GetSection( "system.web/machineKey" );
        }

        if ( section != null )
        {
            string decryption    = section.Decryption == null ? string.Empty : section.Decryption.ToLowerInvariant();
            string decryptionKey = section.DecryptionKey == null ? string.Empty : section.DecryptionKey;
            //
            // Encryption mode.
            // 
            switch ( decryption )
            {
                case @"aes":
                    _encryptionMode = EncryptionMode.AES;
                    break;

                case @"des":
                    _encryptionMode = EncryptionMode.DES;
                    break;

                case @"3des":
                    _encryptionMode = EncryptionMode.TripleDES;
                    break;

                default:
                    _encryptionMode = EncryptionMode.None;
                    break;
            }

            switch ( decryptionKey )
            {
                case @"":
                    if ( _encryptionMode != EncryptionMode.None )
                        throw new InvalidOperationException( "No MachineKey.DecryptionKey specified" );
                    break;

                default:
                    if ( _encryptionMode == EncryptionMode.DES )
                    {
                        if ( decryptionKey.Length < 16 )
                            throw new InvalidOperationException( "Insufficient MachineKey.DecryptionKey material supplied" );

                        _encryptionKey = DecodeHex( decryptionKey );
                    }
                    else if ( _encryptionMode == EncryptionMode.AES )
                    {
                        if ( decryptionKey.Length < 48 )
                            throw new InvalidOperationException( "Insufficient MachineKey.DecryptionKey material supplied" );

                        _encryptionKey = DecodeHex( decryptionKey );
                    }
                    else if ( _encryptionMode == EncryptionMode.TripleDES )
                    {
                        if ( decryptionKey.Length < 48 )
                            throw new InvalidOperationException( "Insufficient MachineKey.DecryptionKey material supplied" );

                        _encryptionKey = DecodeHex( decryptionKey );
                    }

                    break;
            }
        }
        else
        {
            _encryptionMode = EncryptionMode.None;
        }
    }

    /// <summary>
    /// Decode the array of bytes using the configured key and algorithm.
    /// </summary>
    /// <param name="value">The bytes to decode</param>
    /// <returns>The decoded bytes</returns>
    public override byte[] Decode( byte[] value )
    {
        if ( null == value || 0 == value.Length )
            throw new ArgumentNullException( "value" );

        SymmetricAlgorithm algorithm;

        switch ( _encryptionMode )
        {
            case EncryptionMode.None:
                return value;

            case EncryptionMode.AES:
                algorithm = Aes.Create();
                { algorithm.KeySize = 192; algorithm.Padding = PaddingMode.None; algorithm.Key = (byte[]) _encryptionKey.Clone(); }

                return new SymmetricEncryptionFormatter( algorithm ).Decrypt( value );

            case EncryptionMode.DES:
                algorithm = DES.Create();
                { algorithm.KeySize = 64; algorithm.Padding = PaddingMode.None; algorithm.Key = (byte[]) _encryptionKey.Clone(); }

                return new SymmetricEncryptionFormatter( algorithm ).Decrypt( value );

            case EncryptionMode.TripleDES:
                algorithm = TripleDES.Create();
                { algorithm.KeySize = 192; algorithm.Padding = PaddingMode.None; algorithm.Key = (byte[]) _encryptionKey.Clone(); }

                return new SymmetricEncryptionFormatter( algorithm ).Decrypt( value );
        }

        return value;
    }

    /// <summary>
    /// Parses a string of hex characters into a byte array.
    /// </summary>
    private byte[] DecodeHex( string hexString )
    {
        byte[] bytes = new byte[hexString.Length >> 1];

        for ( int i = 0; i < bytes.Length; i++ )
            bytes[i] = Convert.ToByte( hexString.Substring( i << 1, 2 ), 16 );

        return bytes;
    }

    /// <summary>
    /// Encodes an array of bytes using the configured key and algorithm.
    /// </summary>
    /// <param name="value">The bytes to encode</param>
    /// <returns>The encoded bytes</returns>
    public override byte[] Encode( byte[] value )
    {
        if ( null == value || 0 == value.Length )
            throw new ArgumentNullException( "value" );

        SymmetricAlgorithm algorithm;

        switch ( _encryptionMode )
        {
            case EncryptionMode.None:
                return value;

            case EncryptionMode.AES:
                algorithm = Aes.Create();
                { algorithm.KeySize = 192; algorithm.Padding = PaddingMode.None; algorithm.Key = (byte[]) _encryptionKey.Clone(); }

                return new SymmetricEncryptionFormatter( algorithm ).Encrypt( value );

            case EncryptionMode.DES:
                algorithm = DES.Create();
                { algorithm.KeySize = 64; algorithm.Padding = PaddingMode.None; algorithm.Key = (byte[]) _encryptionKey.Clone(); }

                return new SymmetricEncryptionFormatter( algorithm ).Encrypt( value );

            case EncryptionMode.TripleDES:
                algorithm = TripleDES.Create();
                { algorithm.KeySize = 192; algorithm.Padding = PaddingMode.None; algorithm.Key = (byte[]) _encryptionKey.Clone(); }

                return new SymmetricEncryptionFormatter( algorithm ).Encrypt( value );
        }

        return value;
    }
}
