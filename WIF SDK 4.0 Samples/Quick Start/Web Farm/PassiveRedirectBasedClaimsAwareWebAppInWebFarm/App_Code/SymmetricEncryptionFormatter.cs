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
using System.Diagnostics;
using System.Security.Cryptography;

/// <summary>
/// Provides symmetric-key encryption and decryption facilities.
/// </summary>
public class SymmetricEncryptionFormatter
{
    SymmetricAlgorithm _key;

    public SymmetricEncryptionFormatter()
    {
    }

    public SymmetricEncryptionFormatter( SymmetricAlgorithm key )
    {
        Key = key;
    }

    /// <summary>
    /// Decrypts the specified data
    /// </summary>
    public virtual byte[] Decrypt( byte[] data )
    {
        if ( data == null )
            throw new ArgumentNullException( "data" );

        if ( _key == null )
            throw new InvalidOperationException( "No cryptographic algorithm was available" );

        Debug.Assert( _key.Padding == PaddingMode.None );
        //
        // Force the padding mode to None - we handle padding here.
        //
        _key.Mode = CipherMode.CBC;
        _key.Padding = PaddingMode.None;
        //
        // Retrieve the IV value from the input data
        //
        int    blockSizeInBytes = _key.BlockSize >> 3;
        byte[] iv               = new byte[blockSizeInBytes];

        Array.Copy( data, 0, iv, 0, blockSizeInBytes );
        //
        // We manually remove the padding according to the last byte value
        // this is a fix for FX not supporting the ISO 10126 padding mode
        //
        ICryptoTransform transform            = _key.CreateDecryptor( _key.Key, iv );
        byte[]           plainTextWithPadding = new byte[(data.Length - blockSizeInBytes) / transform.InputBlockSize * transform.OutputBlockSize];
        //
        // Decrypt it block by block
        //
        int offset = blockSizeInBytes;

        for ( ; offset < data.Length - blockSizeInBytes; offset += blockSizeInBytes )
            transform.TransformBlock( data, offset, blockSizeInBytes, plainTextWithPadding, offset - blockSizeInBytes );

        byte[] lastBlock = transform.TransformFinalBlock( data, offset, blockSizeInBytes );

        Array.Copy( lastBlock, 0, plainTextWithPadding, offset - blockSizeInBytes, lastBlock.Length );
        //
        // Remove padding ourselves based on the last byte value
        //
        int lengthOfPadding = (int) plainTextWithPadding[plainTextWithPadding.Length - 1];

        if ( plainTextWithPadding.Length < lengthOfPadding )
            throw new CryptographicException( "Invalid Padding" );

        if ( lengthOfPadding > blockSizeInBytes )
            throw new CryptographicException( "Invalid Padding" );

        byte[] plainText = new byte[plainTextWithPadding.Length - lengthOfPadding];

        Array.Copy( plainTextWithPadding, 0, plainText, 0, plainTextWithPadding.Length - lengthOfPadding );
        //
        // Return the plain text
        //
        return plainText;
    }

    /// <summary>
    /// Encrypts the specified data
    /// </summary>
    public virtual byte[] Encrypt( byte[] data )
    {
        if ( data == null )
            throw new ArgumentNullException( "data" );

        if ( _key == null )
            throw new InvalidOperationException( "No cryptographic algorithm was available" );

        Debug.Assert( _key.Padding == PaddingMode.None );
        //
        // Force the padding mode to None
        //
        _key.Mode = CipherMode.CBC;
        _key.Padding = PaddingMode.None;
        //
        // Always generate a new IV for every encryption operation.
        //
        _key.GenerateIV();

        byte[] iv = _key.IV;
        //
        // We manually append padding using PKCS7 mode
        //
        int    blockSizeInBytes     = _key.BlockSize >> 3;
        int    paddingInBytes       = blockSizeInBytes - (data.Length % blockSizeInBytes);
        byte[] plainTextWithPadding = new byte[data.Length + paddingInBytes];

        Array.Copy( data, 0, plainTextWithPadding, 0, data.Length );

        for ( int index = data.Length; index < plainTextWithPadding.Length; index++ )
            plainTextWithPadding[index] = (byte) paddingInBytes;

        ICryptoTransform transform = _key.CreateEncryptor( _key.Key, iv );

        System.Diagnostics.Debug.Assert( transform.InputBlockSize == blockSizeInBytes );

        int    ivLength         = iv.Length;
        int    cipherTextLength = plainTextWithPadding.Length / transform.InputBlockSize * transform.OutputBlockSize;
        byte[] cipherText       = new byte[ivLength + cipherTextLength];
        //
        // Prepend the _iv to the cipher text
        //
        Array.Copy( iv, 0, cipherText, 0, ivLength );
        //
        // Add the encrypted data itself block by block
        //
        int offset = 0;

        for ( ; offset < plainTextWithPadding.Length - blockSizeInBytes; offset += blockSizeInBytes )
            transform.TransformBlock( plainTextWithPadding, offset, blockSizeInBytes, cipherText, ivLength + offset );

        byte[] lastBlock = transform.TransformFinalBlock( plainTextWithPadding, offset, blockSizeInBytes );

        Array.Copy( lastBlock, 0, cipherText, ivLength + offset, lastBlock.Length );
        //
        // Return the cipher text
        //
        return cipherText;
    }

    /// <summary>
    /// Gets or sets the key object.
    /// </summary>
    public virtual SymmetricAlgorithm Key
    {
        get
        {
            return _key;
        }
        set
        {
            _key = value;
        }
    }
}
