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
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

public class CustomIssuerTokenResolver : SecurityTokenResolver
{
    List<X509Certificate2> trustedIssuerCertificates = new List<X509Certificate2>();

    public CustomIssuerTokenResolver()
    {
        //
        // Populate the list of trusted issuer certs for the issuer token resolver.
        // For illustrative purposes, this sample reads the Live STS Federation metadata
        // on AppDomain startup. In a production system, you may prefer to store the
        // Live STS signing certificates in configuration in order to improve startup time.
        //
        IEnumerable<X509Certificate2> liveStsSigningCertificates = LiveStsFederationMetadata.Instance.SigningCertificates;
        foreach ( X509Certificate2 liveStsCertificate in liveStsSigningCertificates )
        {
            trustedIssuerCertificates.Add( liveStsCertificate );
        }
    }

    /// <summary>
    /// Resolves the given SecurityKeyIdentifierClause to a SecurityKey.
    /// </summary>
    /// <param name="keyIdentifierClause">SecurityKeyIdentifierClause to resolve</param>
    /// <param name="key">The resolved SecurityKey.</param>
    /// <returns>True if successfully resolved.</returns>
    /// <exception cref="ArgumentNullException">The input argument 'keyIdentifierClause' is null.</exception>
    protected override bool TryResolveSecurityKeyCore( SecurityKeyIdentifierClause keyIdentifierClause, out SecurityKey key )
    {
        if ( keyIdentifierClause == null )
        {
            throw new ArgumentNullException( "keyIdentifierClause" );
        }

        key = null;
        EncryptedKeyIdentifierClause encryptedKeyIdentifierClause = keyIdentifierClause as EncryptedKeyIdentifierClause;
        if ( encryptedKeyIdentifierClause != null )
        {
            SecurityKeyIdentifier keyIdentifier = encryptedKeyIdentifierClause.EncryptingKeyIdentifier;
            if ( keyIdentifier != null && keyIdentifier.Count > 0 )
            {
                for ( int i = 0; i < keyIdentifier.Count; i++ )
                {
                    SecurityKey unwrappingSecurityKey = null;
                    if ( TryResolveSecurityKey( keyIdentifier[i], out unwrappingSecurityKey ) )
                    {
                        key = new InMemorySymmetricSecurityKey( unwrappingSecurityKey.DecryptKey( encryptedKeyIdentifierClause.EncryptionMethod, encryptedKeyIdentifierClause.GetEncryptedKey() ), false );
                        return true;
                    }
                }
            }
        }
        else
        {
            SecurityToken token = null;
            if ( TryResolveToken( keyIdentifierClause, out token ) )
            {
                if ( token.SecurityKeys.Count > 0 )
                {
                    key = token.SecurityKeys[0];
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Resolves the given SecurityKeyIdentifier to a SecurityToken.
    /// </summary>
    /// <param name="keyIdentifier">SecurityKeyIdentifier to resolve.</param>
    /// <param name="token">The resolved SecurityToken.</param>
    /// <returns>True if successfully resolved.</returns>
    /// <exception cref="ArgumentNullException">The input argument 'keyIdentifier' is null.</exception>
    protected override bool TryResolveTokenCore( SecurityKeyIdentifier keyIdentifier, out SecurityToken token )
    {
        if ( keyIdentifier == null )
        {
            throw new ArgumentNullException( "keyIdentifier" );
        }

        token = null;
        foreach ( SecurityKeyIdentifierClause clause in keyIdentifier )
        {
            if ( TryResolveToken( clause, out token ) )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Resolves the given SecurityKeyIdentifierClause to a SecurityToken.
    /// </summary>
    /// <param name="keyIdentifierClause">SecurityKeyIdentifierClause to resolve.</param>
    /// <param name="token">The resolved SecurityToken.</param>
    /// <returns>True if successfully resolved.</returns>
    /// <exception cref="ArgumentNullException">The input argument 'keyIdentifierClause' is null.</exception>
    protected override bool TryResolveTokenCore( SecurityKeyIdentifierClause keyIdentifierClause, out SecurityToken token )
    {
        if ( keyIdentifierClause == null )
        {
            throw new ArgumentNullException( "keyIdentifierClause" );
        }

        token = null;

        foreach ( X509Certificate2 cert in trustedIssuerCertificates )
        {
            X509ThumbprintKeyIdentifierClause thumbprintKeyIdentifierClause = keyIdentifierClause as X509ThumbprintKeyIdentifierClause;
            if ( ( thumbprintKeyIdentifierClause != null ) && ( thumbprintKeyIdentifierClause.Matches( cert ) ) )
            {
                token = new X509SecurityToken( cert );
                return true;
            }

            X509IssuerSerialKeyIdentifierClause issuerSerialKeyIdentifierClause = keyIdentifierClause as X509IssuerSerialKeyIdentifierClause;
            if ( ( issuerSerialKeyIdentifierClause != null ) && ( issuerSerialKeyIdentifierClause.Matches( cert ) ) )
            {
                token = new X509SecurityToken( cert );
                return true;
            }

            X509SubjectKeyIdentifierClause subjectKeyIdentifierClause = keyIdentifierClause as X509SubjectKeyIdentifierClause;
            if ( ( subjectKeyIdentifierClause != null ) && ( subjectKeyIdentifierClause.Matches( cert ) ) )
            {
                token = new X509SecurityToken( cert );
                return true;
            }

            X509RawDataKeyIdentifierClause rawDataKeyIdentifierClause = keyIdentifierClause as X509RawDataKeyIdentifierClause;
            if ( ( rawDataKeyIdentifierClause != null ) && ( rawDataKeyIdentifierClause.Matches( cert ) ) )
            {
                token = new X509SecurityToken( cert );
                return true;
            }
        }

        return false;
    }
}
