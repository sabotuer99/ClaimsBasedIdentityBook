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

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

/// <summary>
/// This class encrypts the session security token using the RSA key
/// of the relying party's service certificate.
/// </summary>
public class RsaEncryptedSessionSecurityTokenHandler : SessionSecurityTokenHandler
{
    static List<CookieTransform> _transforms;

    static RsaEncryptedSessionSecurityTokenHandler()
    {
        X509Certificate2 serviceCertificate = CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" );
        _transforms = new List<CookieTransform>() 
                        { 
                            new DeflateCookieTransform(), 
                            new RsaEncryptionCookieTransform( serviceCertificate ),
                            new RsaSignatureCookieTransform( serviceCertificate ),
                        };
    }

    public RsaEncryptedSessionSecurityTokenHandler()
        : base( _transforms.AsReadOnly() )
    {
    }
}
