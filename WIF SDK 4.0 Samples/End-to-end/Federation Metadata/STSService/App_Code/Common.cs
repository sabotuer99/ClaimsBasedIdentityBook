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
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Web.Configuration;
using System.Xml;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSFederation.Metadata;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.SecurityTokenService;

/// <summary>
/// Summary description for Common
/// </summary>
public static class Common
{
    public const string EncryptingCertificateName = "EncryptingCertificateName";
    public const string IssuerName = "IssuerName";
    public const string QuotationClassClaimType = "http://schemas.microsoft.com/WindowsIdentityFramework/Samples/ClaimTypes/QuotationClass";
    public const string SigningCertificateName = "SigningCertificateName";

    /// <summary>
    /// Generate a sample MetadataBase.
    /// </summary>
    /// <remarks>
    /// In a production system this would be generated from the STS configuration.
    /// </remarks>
    public static MetadataBase GetFederationMetadata()
    {
        string endpointId = "http://localhost/stsservice/service.svc";
        EntityDescriptor metadata = new EntityDescriptor();
        metadata.EntityId = new EntityId(endpointId);
        
        // Define the signing key
        string signingCertificateName = WebConfigurationManager.AppSettings["SigningCertificateName"];
        X509Certificate2 cert = CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, signingCertificateName );
        metadata.SigningCredentials = new X509SigningCredentials( cert );

        // Create role descriptor for security token service
        SecurityTokenServiceDescriptor stsRole = new SecurityTokenServiceDescriptor();
        stsRole.ProtocolsSupported.Add( new Uri( WSFederationMetadataConstants.Namespace ) );
        metadata.RoleDescriptors.Add( stsRole );

        // Add a contact name
        ContactPerson person = new ContactPerson( ContactType.Administrative );
        person.GivenName = "contactName";
        stsRole.Contacts.Add( person );

        // Include key identifier for signing key in metadata
        SecurityKeyIdentifierClause clause = new X509RawDataKeyIdentifierClause( cert );
        SecurityKeyIdentifier ski = new SecurityKeyIdentifier( clause );
        KeyDescriptor signingKey = new KeyDescriptor( ski );
        signingKey.Use = KeyType.Signing;
        stsRole.Keys.Add( signingKey );
        
        // Add endpoints
        string activeSTSUrl = "https://localhost/stsservice/service.svc";
        EndpointAddress endpointAddress = new EndpointAddress( new Uri( activeSTSUrl ),
                                                    null,
                                                    null, GetMetadataReader( activeSTSUrl ), null );
        stsRole.SecurityTokenServiceEndpoints.Add( endpointAddress );

        // Add a collection of offered claims
        // NOTE: In this sample, these claims must match the claims actually generated in CustomSecurityTokenService.GetOutputClaimsIdentity.
        //       In a production system, there would be some common data store that both use
        stsRole.ClaimTypesOffered.Add( new DisplayClaim( ClaimTypes.Name, "Name", "The name of the subject." ) );
        stsRole.ClaimTypesOffered.Add( new DisplayClaim( ClaimTypes.Role, "Role", "The role of the subject." ) );
        // Add a special claim for the QuoteService
        stsRole.ClaimTypesOffered.Add( new DisplayClaim( QuotationClassClaimType, "QuotationClass", "Class of quotation desired." ) );

        return metadata;
    }

    /// <summary>
    /// Create a reader to provide simulated Metadata endpoint configuration element
    /// </summary>
    /// <param name="activeSTSUrl">The active endpoint URL.</param>
    static XmlDictionaryReader GetMetadataReader( string activeSTSUrl )
    {
        MetadataSet metadata = new MetadataSet();
        MetadataReference mexReferece = new MetadataReference( new EndpointAddress( activeSTSUrl + "/mex" ), AddressingVersion.WSAddressing10 );
        MetadataSection refSection = new MetadataSection( MetadataSection.MetadataExchangeDialect, null, mexReferece );
        metadata.MetadataSections.Add( refSection );

        byte[] metadataSectionBytes;
        StringBuilder stringBuilder = new StringBuilder();
        using ( StringWriter stringWriter = new StringWriter( stringBuilder ) )
        {
            using ( XmlTextWriter textWriter = new XmlTextWriter( stringWriter ) )
            {
                metadata.WriteTo( textWriter );
                textWriter.Flush();
                stringWriter.Flush();
                metadataSectionBytes = stringWriter.Encoding.GetBytes( stringBuilder.ToString() );
            }
        }

        return XmlDictionaryReader.CreateTextReader( metadataSectionBytes, XmlDictionaryReaderQuotas.Max );
    }
}
