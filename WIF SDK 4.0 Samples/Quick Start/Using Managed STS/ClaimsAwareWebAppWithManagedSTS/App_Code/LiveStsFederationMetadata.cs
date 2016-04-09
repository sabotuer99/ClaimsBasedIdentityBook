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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Microsoft.IdentityModel.Protocols.WSFederation.Metadata;

/// <summary>
/// This class reads the Live STS Federation metadata and exposes the desired properties to callers.
/// </summary>
sealed class LiveStsFederationMetadata
{
    //
    // This is the property in AppSettings to which the Federation Utility tool
    // writes the location of the STS metadata.
    //
    const string FederationMetadataLocationAppSettingKey = "FederationMetadataLocation";

    //
    // Singleton instance. Load metadata once per AppDomain startup.
    //
    static readonly LiveStsFederationMetadata _instance = new LiveStsFederationMetadata();

    IEnumerable<X509Certificate2> _signingCertificates;

    //
    // Private constructor for Singleton.
    // 
    private LiveStsFederationMetadata()
    {
        MetadataBase metadataBase = LoadMetadata();
        SecurityTokenServiceDescriptor stsDescriptor = ReadStsDescriptor( metadataBase );
        _signingCertificates = ReadStsSigningCertificates( stsDescriptor );
    }

    private MetadataBase LoadMetadata()
    {
        string federationMetadataUrl = ConfigurationManager.AppSettings[FederationMetadataLocationAppSettingKey];

        if ( String.IsNullOrEmpty( federationMetadataUrl ) )
        {
            throw new InvalidOperationException( "Federation Utility must be used to configure this relying party before first use." );
        }

        using ( XmlDictionaryReader reader = XmlDictionaryReader.CreateDictionaryReader( new XmlTextReader( federationMetadataUrl ) ) )
        {
            MetadataSerializer serializer = new MetadataSerializer();
            return serializer.ReadMetadata( reader );
        }
    }

    private SecurityTokenServiceDescriptor ReadStsDescriptor( MetadataBase metadataBase )
    {
        EntitiesDescriptor entities = metadataBase as EntitiesDescriptor;
        if ( entities != null )
        {
            foreach ( EntityDescriptor en in entities.ChildEntities )
            {
                foreach ( RoleDescriptor role in en.RoleDescriptors )
                {
                    SecurityTokenServiceDescriptor stsDescriptor = role as SecurityTokenServiceDescriptor;
                    if ( stsDescriptor != null )
                    {
                        //
                        // If there are multiple STS descriptors, the first one listed will be used.
                        // This mirrors the behavior of the Federation Utility tool that creates automatic trust
                        // using the STS Federation metadata. This is provided for illustrative purposes only.
                        // In a production system, you may choose to trust one or more STS descriptors that are
                        // published in the STS Federation metadata.
                        //
                        return stsDescriptor;
                    }
                }
            }
        }

        EntityDescriptor entity = metadataBase as EntityDescriptor;
        if ( entity != null )
        {
            foreach ( RoleDescriptor role in entity.RoleDescriptors )
            {
                SecurityTokenServiceDescriptor stsDescriptor = role as SecurityTokenServiceDescriptor;
                if ( stsDescriptor != null )
                {
                    //
                    // If there are multiple STS descriptors, the first one listed will be used.
                    // This mirrors the behavior of the Federation Utility tool that creates automatic trust
                    // using the STS Federation metadata. This is provided for illustrative purposes only.
                    // In a production system, you may choose to trust one or more STS descriptors that are
                    // published in the STS Federation metadata.
                    //
                    return stsDescriptor;
                }
            }
        }

        throw new InvalidOperationException( "The metadata does not contain a valid SecurityTokenServiceDescriptor." );
    }

    /// <summary>
    /// Returns the collection of certificates that the STS uses to sign tokens.
    /// </summary>
    /// <returns>The collection of certificates.</returns>
    private IEnumerable<X509Certificate2> ReadStsSigningCertificates( SecurityTokenServiceDescriptor stsDescriptor )
    {
        List<X509Certificate2> stsCertificates = new List<X509Certificate2>();

        if ( stsDescriptor != null && stsDescriptor.Keys != null )
        {
            Collection<KeyDescriptor> keyDescriptors = (Collection<KeyDescriptor>)stsDescriptor.Keys;

            if ( keyDescriptors != null && keyDescriptors.Count > 0 )
            {
                foreach ( KeyDescriptor keyDescriptor in keyDescriptors )
                {
                    if ( keyDescriptor.KeyInfo != null && ( keyDescriptor.Use == KeyType.Signing || keyDescriptor.Use == KeyType.Unspecified ) )
                    {
                        SecurityKeyIdentifier kid = keyDescriptor.KeyInfo;
                        X509RawDataKeyIdentifierClause clause = null;

                        kid.TryFind<X509RawDataKeyIdentifierClause>( out clause );

                        if ( clause != null )
                        {
                            X509Certificate2 certificate = new X509Certificate2( clause.GetX509RawData() );
                            stsCertificates.Add( certificate );
                        }
                    }
                }
            }
        }

        return stsCertificates;
    }

    public static LiveStsFederationMetadata Instance
    {
        get { return _instance; }
    }

    public IEnumerable<X509Certificate2> SigningCertificates
    {
        get { return _signingCertificates; }
    }
}
