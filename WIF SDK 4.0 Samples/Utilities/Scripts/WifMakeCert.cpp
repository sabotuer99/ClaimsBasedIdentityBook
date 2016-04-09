//------------------------------------------------------------
// Copyright ( c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

#pragma once
#include "stdio.h"
#include "stdlib.h"
#include "windows.h"
#include "wincrypt.h"
#include "strsafe.h"
#include "assert.h"
#include "Rpc.h"
#include "Objbase.h"

const size_t MAX_CERT_SUBJECT_NAME_LENGTH = 200;

//
// Length of {xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} + NULL
//
const size_t MAX_CLSID_STRING_LENGTH      = 39;

//
// Length of xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx + NULL
//
const size_t MAX_GUID_STRING_LENGTH       = MAX_CLSID_STRING_LENGTH - 2;

const DWORD  CERT_ENCODING_TYPE           = X509_ASN_ENCODING | PKCS_7_ASN_ENCODING;

const DWORD RSA2048BIT_KEY                = 0x08000000;

HRESULT DuplicateGuidString( __in PCWSTR pwszGuidIn,
                             __deref_out PWSTR *ppwszGuidOut )
{
    HRESULT hr = E_FAIL;
    size_t cchIn = 0;
    PWSTR pwszGuid = NULL;

    if ( ppwszGuidOut == NULL )
    {
        hr = E_INVALIDARG;
        fwprintf( stderr, L"DuplicateGuidString: ppwszGuidOut is null.\n" );
        goto Cleanup;
    }

    hr = StringCchLengthW( pwszGuidIn, MAX_GUID_STRING_LENGTH, &cchIn );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"DuplicateGuidString: StringCchLengthW pwszGuidIn Error 0x%x\n", hr );
        goto Cleanup;
    }

    pwszGuid = (WCHAR *) LocalAlloc( LMEM_FIXED, ( cchIn + 1 ) * sizeof( WCHAR ) );

    if ( pwszGuid == NULL )
    {
        hr = E_OUTOFMEMORY;
        fwprintf( stderr, L"DuplicateGuidString: LocalAlloc pwszGuid failed.\n" );
        goto Cleanup;
    }

    hr = StringCchCopyNW( pwszGuid, cchIn + 1, pwszGuidIn, cchIn );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"DuplicateGuidString: StringCchLengthW pwszGuidIn Error 0x%x\n", hr );
        goto Cleanup;
    }

    //
    // Assign the out param
    //
    *ppwszGuidOut = pwszGuid;
    pwszGuid = NULL;

    hr = S_OK;

Cleanup:
    if ( pwszGuid != NULL )
    {
        LocalFree( pwszGuid );
        pwszGuid = NULL;
    }

    return hr;
}

HRESULT ConvertClsidToWString( __in CLSID const *pClsid,
                               __deref_out PWSTR *ppwszClsid )
{
    HRESULT hr = E_FAIL;
    PWSTR pwszClsid = NULL;
    WCHAR *pwszTemp = NULL;
    WCHAR *pwszClsidLastChar = NULL;

    if ( ppwszClsid == NULL )
    {
        hr = E_INVALIDARG;
        fwprintf( stderr, L"ConvertClsidToWString: ppwszClsid is null.\n" );
        goto Cleanup;
    }

    *ppwszClsid = NULL;

    //
    // First, get the CLSID string
    //
    hr = StringFromCLSID( *pClsid, &pwszClsid );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"ConvertClsidToWString: StringFromCLSID Error 0x%x\n", hr );
        goto Cleanup;
    }

    //
    // Next, convert the CLSID hex to lower case.
    // pwszTemp moves through pwszClsid to in-place convert each character to lower case.
    //
    for ( pwszTemp = pwszClsid; L'\0' != *pwszTemp; pwszTemp++ )
    {
        if (L'A' <= *pwszTemp && L'F' >= *pwszTemp)
        {
            *pwszTemp = towlower( *pwszTemp );
        }
    }

    //
    // Get the length of the lower case CLSID string
    //
    size_t cchClsid = 0;
    hr = StringCchLengthW( pwszClsid, MAX_CLSID_STRING_LENGTH, &cchClsid );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"ConvertClsidToWString: StringCchLengthW Error 0x%x\n", hr );
        goto Cleanup;
    }

    //
    // Point pwszTemp at the start of the lower case CLSID string
    //
    pwszTemp = pwszClsid;

    //
    // Point pwszClsidLastChar at the last character
    //
    pwszClsidLastChar = &pwszClsid[cchClsid - 1];

    //
    // If present, trim out the '{' at the beginning and the '}' at the end
    //
    if (L'{' == *pwszTemp && L'}' == *pwszClsidLastChar)
    {
        //
        // Skipping the '{'
        //
        pwszTemp++;

        //
        // Overwriting the '}'
        //
        *pwszClsidLastChar = L'\0';
    }

    //
    // Copy the lower-case GUID with braces removed into the output string
    //
    hr = DuplicateGuidString( pwszTemp, ppwszClsid );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"ConvertClsidToWString: DuplicateGuidString Error 0x%x\n", hr );
        goto Cleanup;
    }

    hr = S_OK;

Cleanup:
    if ( pwszClsid != NULL )
    {
        CoTaskMemFree( pwszClsid );
        pwszClsid = NULL;
    }
    return hr;
}

HRESULT WifUuidCreate( __out UUID *pUuid )
{
    HRESULT hr = E_FAIL;
    BYTE *pb = NULL;

    if ( pUuid == NULL )
    {
        hr = E_INVALIDARG;
        fwprintf( stderr, L"WifUuidCreate: pUuid is null.\n" );
        return hr;
    }

    hr = UuidCreate( pUuid );
    if ( S_OK != hr )
    {
        BYTE *pbEnd;

        assert( (HRESULT) RPC_S_UUID_LOCAL_ONLY == hr );

        // No net card?  Fake up a GUID:

        pb = (BYTE *) pUuid;
        pbEnd = (BYTE *) pb + sizeof( *pUuid );

        GetSystemTimeAsFileTime( (FILETIME *) pb );
        pb += sizeof( FILETIME );

        while ( pb < pbEnd )
        {
            *(DWORD *) pb = GetTickCount();
            pb += sizeof( DWORD );
        }
        assert( pb == pbEnd );
    }

    return hr;
}

//
// Summary:
// Creates a new GUID string. It allocates a lower case GUID using LocalAlloc().
//
// Contract: *ppwszGUID must be freed by the caller using LocalFree().
//
HRESULT GenerateGuidWString( __deref_out PWSTR *ppwszGUID )
{
    HRESULT hr = E_FAIL;
    GUID guid = {};

    if ( ppwszGUID == NULL )
    {
        hr = E_INVALIDARG;
        fwprintf( stderr, L"GenerateGuidWString: ppwszGUID is null.\n" );
        goto Cleanup;
    }
    *ppwszGUID = NULL;

    hr = WifUuidCreate( &guid );

    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"GenerateGuidWString: WifUuidCreate Error 0x%x\n", hr );
        goto Cleanup;
    }

    hr = ConvertClsidToWString( &guid, ppwszGUID );

    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"GenerateGuidWString: ConvertClsidToWString Error 0x%x\n", hr );
        goto Cleanup;
    }

    hr = S_OK;

Cleanup:
    return hr;
}

//
// Summary:
// This function adds the certificate specified by the pCertContext to the LocalMachine or CurrentUser Personal store.
//
BOOL AddCertificateToMyStore( __in PCCERT_CONTEXT pCertContext,
                              __in BOOL fMachineCertificate )
{
    HCERTSTORE hStore = NULL;
    BOOL result = FALSE;

    DWORD dwStoreFlags = fMachineCertificate ? CERT_SYSTEM_STORE_LOCAL_MACHINE : CERT_SYSTEM_STORE_CURRENT_USER;

    //
    // Open My cert store in machine/user profile
    //
    hStore = CertOpenStore( CERT_STORE_PROV_SYSTEM, 
                            0,
                            0,
                            dwStoreFlags,
                            L"My" );
    if ( hStore == NULL )
    {
        fwprintf( stderr, L"AddCertificateToLocalMachineMyStore: CertOpenStore Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    //
    // Add self-signed cert to the store
    //
    if ( !CertAddCertificateContextToStore( hStore,
                                            pCertContext,
                                            CERT_STORE_ADD_REPLACE_EXISTING,
                                            0 ) )
    {
        fwprintf( stderr, L"AddCertificateToLocalMachineMyStore: CertAddCertificateContextToStore Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    result = TRUE;

Cleanup:
    if ( hStore != NULL )
    {
        CertCloseStore( hStore,
                        0 );
        hStore = NULL;
    }

    return result;
}

//
// Summary:
// This function creates a self-signed certificate given the Key provider info.
//
// Contract: *ppCertContext must be freed by the caller using CertFreeCertificateContext();
//
BOOL CreateCertificate( __in PCRYPT_KEY_PROV_INFO pKeyProvInfo,
                        __in PCERT_NAME_BLOB pSubjectIssuerBlob,
                        __in PCERT_PUBLIC_KEY_INFO pPublicKeyInfo,
                        __deref_out PCCERT_CONTEXT *ppCertContext )
{
    BOOL result = FALSE;
    
    BYTE *pbEncodedKeyUsage = NULL;
    DWORD cbEncodedKeyUsage = 0;

    BYTE *pbEncodedHash = NULL;
    DWORD cbEncodedHash = 0;

    CERT_EXTENSIONS Exts = {};
    CERT_EXTENSION rgExt[2];
    Exts.cExtension = 2;
    Exts.rgExtension = rgExt;

    CRYPT_BIT_BLOB KeyUsage = {};
    CRYPT_DATA_BLOB Hash = {};
    
    if ( ppCertContext == NULL )
    {
        fwprintf( stderr, L"CreateCertificate: ppCertContext is null.\n" );
        goto Cleanup;
    }

    //
    // Create the Key Usage extension
    //
    BYTE byteKeyUsageData = CERT_DIGITAL_SIGNATURE_KEY_USAGE 
                            | CERT_NON_REPUDIATION_KEY_USAGE 
                            | CERT_KEY_ENCIPHERMENT_KEY_USAGE 
                            | CERT_DATA_ENCIPHERMENT_KEY_USAGE;

    KeyUsage.cbData = sizeof( byteKeyUsageData );
    KeyUsage.pbData = &byteKeyUsageData;

    if ( !CryptEncodeObject( CERT_ENCODING_TYPE,
                             X509_KEY_USAGE,
                             (LPVOID)&KeyUsage,
                             NULL,
                             &cbEncodedKeyUsage ) )
    {
        fwprintf( stderr, L"CreateCertificate: CryptEncodeObject KeyUsage 1 Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    pbEncodedKeyUsage = (BYTE *)LocalAlloc( LMEM_FIXED, cbEncodedKeyUsage );
    if ( pbEncodedKeyUsage == NULL )
    {
        fwprintf( stderr, L"CreateCertificate: LocalAlloc pbEncodedKeyUsage Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    if ( !CryptEncodeObject( CERT_ENCODING_TYPE,
                             X509_KEY_USAGE,
                             (LPVOID)&KeyUsage,
                             pbEncodedKeyUsage,
                             &cbEncodedKeyUsage ) )
    {
        fwprintf( stderr, L"CreateCertificate: CryptEncodeObject KeyUsage 2 Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    rgExt[0].pszObjId     = szOID_KEY_USAGE;
    rgExt[0].Value.cbData = cbEncodedKeyUsage;
    rgExt[0].Value.pbData = pbEncodedKeyUsage;
    rgExt[0].fCritical    = FALSE;

    //
    // Create the SKI extension
    //
    BYTE abKeyId[20] = {};
    BYTE *pbHashData = &abKeyId[0];
    DWORD cbHashData = sizeof(abKeyId);

    if ( !CryptHashCertificate( NULL,
                                CALG_SHA1,
                                0,
                                pPublicKeyInfo->PublicKey.pbData,
                                pPublicKeyInfo->PublicKey.cbData,
                                pbHashData,
                                &cbHashData ) )
    {
        fwprintf( stderr, L"CreateCertificate: CryptHashCertificate Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    Hash.cbData = cbHashData;
    Hash.pbData = pbHashData;

    if ( !CryptEncodeObject( CERT_ENCODING_TYPE,
                             szOID_SUBJECT_KEY_IDENTIFIER,
                             (LPVOID)&Hash,
                             NULL,
                             &cbEncodedHash ) )
    {
        fwprintf( stderr, L"CreateCertificate: CryptEncodeObject Hash 1 Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    pbEncodedHash = (BYTE *)LocalAlloc( LMEM_FIXED, cbEncodedHash );
    if ( pbEncodedHash == NULL )
    {
        fwprintf( stderr, L"CreateCertificate: LocalAlloc pbEncodedHash Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    if ( !CryptEncodeObject( CERT_ENCODING_TYPE,
                             szOID_SUBJECT_KEY_IDENTIFIER,
                             (LPVOID)&Hash,
                             pbEncodedHash,
                             &cbEncodedHash ) )
    {
        fwprintf( stderr, L"CreateCertificate: CryptEncodeObject Hash 2 Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    rgExt[1].pszObjId     = szOID_SUBJECT_KEY_IDENTIFIER;
    rgExt[1].Value.cbData = cbEncodedHash;
    rgExt[1].Value.pbData = pbEncodedHash;
    rgExt[1].fCritical    = FALSE;

    //
    // Create self-signed certificate
    //
    *ppCertContext = CertCreateSelfSignCertificate( NULL, 
                                                    pSubjectIssuerBlob,
                                                    0,
                                                    pKeyProvInfo, 
                                                    NULL,  // pSignatureAlgorithm - If NULL, the default algorithm, SHA1RSA, is used.
                                                    NULL,  // pStartTime - If NULL, the system current time is used by default.
                                                    NULL,  // pEndTime -  If NULL, the pStartTime value plus one year will be used by default.
                                                    &Exts );
    if ( *ppCertContext == NULL )
    {
        fwprintf( stderr, L"CreateCertificate: CertCreateSelfSignCertificate Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    result = TRUE;

Cleanup:
    if ( pbEncodedKeyUsage != NULL )
    {
        LocalFree( pbEncodedKeyUsage );
        pbEncodedKeyUsage = NULL;
    }

    if ( pbEncodedHash != NULL )
    {
        LocalFree( pbEncodedHash );
        pbEncodedHash = NULL;
    }

    return result;
}

//
// Summary:
// Creates a new key container name by appending a newly created GUID string to the certificate subject name.
//
// Contract: *ppwszKeyContainerName must be freed by the caller using LocalFree().
//
BOOL CreateKeyContainerName( __in LPWSTR pwszCertSubjectName,
                             __out LPWSTR *ppwszKeyContainerName )
{
    LPWSTR pwszGuidString = NULL;
    BOOL result = FALSE;
    LPWSTR pwszKeyContainerName = NULL;
    
    if ( ppwszKeyContainerName == NULL )
    {
        fwprintf( stderr, L"CreateKeyContainerName: ppwszKeyContainerName is null.\n" );
        goto Cleanup;
    }

    *ppwszKeyContainerName = NULL;

    //
    // Generate a new GUID string
    //
    HRESULT hr = GenerateGuidWString( &pwszGuidString );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"CreateKeyContainerName: GenerateGuidWString Error 0x%x\n", hr );
        goto Cleanup;
    }

    //
    // Compute the length of pwszCertSubjectName
    //
    size_t cchCertSubjectNameLength = 0;

    hr = StringCchLengthW( pwszCertSubjectName, MAX_CERT_SUBJECT_NAME_LENGTH, &cchCertSubjectNameLength );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"CreateKeyContainerName: StringCchLengthW pwszCertSubjectName Error 0x%x\n", hr );
        goto Cleanup;
    }

    //
    // Compute the length of pwszGuidString
    //
    size_t cchGuidStringLength = 0;

    hr = StringCchLengthW( pwszGuidString, MAX_GUID_STRING_LENGTH, &cchGuidStringLength );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"CreateKeyContainerName: StringCchLengthW pwszGuidString Error 0x%x\n", hr );
        goto Cleanup;
    }

    //
    // Allocate and print the key container name
    //
    DWORD dwLength = (DWORD)( cchCertSubjectNameLength + cchGuidStringLength + 1 );
    pwszKeyContainerName = ( LPWSTR )LocalAlloc( LMEM_FIXED, dwLength * sizeof( WCHAR ) );

    if ( pwszKeyContainerName == NULL )
    {
        fwprintf( stderr, L"CreateKeyContainerName: LocalAlloc pwszKeyContainerName Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    hr = StringCchPrintfW( pwszKeyContainerName, dwLength, L"%s%s", pwszCertSubjectName, pwszGuidString );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"CreateKeyContainerName: StringCchPrintfW Error 0x%x\n", hr );
        goto Cleanup;
    }

    //
    // Assign the out param
    //
    *ppwszKeyContainerName = pwszKeyContainerName;
    pwszKeyContainerName = NULL;

    result = TRUE;

Cleanup:
    if ( pwszGuidString != NULL )
    {
        LocalFree( pwszGuidString );
        pwszGuidString = NULL;
    }

    if ( pwszKeyContainerName != NULL )
    {
        LocalFree( pwszKeyContainerName );
        pwszKeyContainerName = NULL;
    }

    return result;
}

//
// Summary:
// Creates a key container for self-signed certificate in the machine profile.
// A new key pair is generated.
//
// Contract:
// 1. The pwszContainerName field of the pKeyProvInfo must be freed by the caller using LocalFree().
// 2. *ppPublicKeyInfo must be freed by the caller using LocalFree().
//
BOOL CreateKeyContainer( __in LPWSTR pwszCertSubjectName, 
                         __in BOOL fMachineCertificate,
                         __out PCRYPT_KEY_PROV_INFO pKeyProvInfo,
                         __deref_out PCERT_PUBLIC_KEY_INFO *ppPublicKeyInfo )
{
    HCRYPTPROV hCryptProv = NULL;
    HCRYPTKEY hKey = NULL;
    LPWSTR pwszKeyContainerName = NULL;
    PCERT_PUBLIC_KEY_INFO pPublicKeyInfo = NULL;
    BOOL result = FALSE;
    
    if ( pKeyProvInfo == NULL )
    {
        fwprintf( stderr, L"CreateKeyContainer: pKeyProvInfo is null.\n" );
        goto Cleanup;
    }
    
    if ( ppPublicKeyInfo == NULL )
    {
        fwprintf( stderr, L"CreateKeyContainer: ppPublicKeyInfo is null.\n" );
        goto Cleanup;
    }

    if ( !CreateKeyContainerName( pwszCertSubjectName, &pwszKeyContainerName ) )
    {
        fwprintf( stderr, L"CreateKeyContainer: CreateKeyContainerName Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }
    
    //
    // Prepare key provider structure for creating self-signed certificate
    //
    pKeyProvInfo->pwszContainerName = pwszKeyContainerName;
    pKeyProvInfo->pwszProvName      = NULL;
    pKeyProvInfo->cProvParam        = 0;
    pKeyProvInfo->rgProvParam       = NULL;
    pKeyProvInfo->dwKeySpec         = AT_KEYEXCHANGE;

    if ( fMachineCertificate )
    {
        pKeyProvInfo->dwProvType    = PROV_RSA_SCHANNEL;
        pKeyProvInfo->dwFlags       = CRYPT_MACHINE_KEYSET;
    }
    else
    {
        pKeyProvInfo->dwProvType    = PROV_RSA_AES;
    }

    //
    // Try to acquire an existing container
    //
    if ( !CryptAcquireContextW( &hCryptProv,
                                pKeyProvInfo->pwszContainerName, 
                                pKeyProvInfo->pwszProvName,
                                pKeyProvInfo->dwProvType,
                                pKeyProvInfo->dwFlags ) ) 
    {
        //
        // Try to create a new container
        //
        if ( !CryptAcquireContextW( &hCryptProv, 
                                    pKeyProvInfo->pwszContainerName, 
                                    pKeyProvInfo->pwszProvName,
                                    pKeyProvInfo->dwProvType,
                                    pKeyProvInfo->dwFlags | CRYPT_NEWKEYSET ) ) 
        {
            fwprintf( stderr, L"CreateKeyContainer: CryptAcquireContextW Error 0x%x\n", GetLastError() );
            goto Cleanup;
        }
    }

    //
    // Generate new key pair
    //
    if ( !CryptGenKey( hCryptProv,
                       pKeyProvInfo->dwKeySpec,
                       RSA2048BIT_KEY | CRYPT_EXPORTABLE,
                       &hKey ) )
    {
        fwprintf( stderr, L"CreateKeyContainer: CryptGenKey Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    //
    // Get the public key info
    //
    DWORD cbPublicKeyInfo;
    if ( !CryptExportPublicKeyInfo( hCryptProv,
                                    pKeyProvInfo->dwKeySpec,
                                    CERT_ENCODING_TYPE,
                                    NULL,
                                    &cbPublicKeyInfo ) )
    {
        fwprintf( stderr, L"CreateKeyContainer: CryptExportPublicKeyInfo 1 Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    pPublicKeyInfo = (PCERT_PUBLIC_KEY_INFO)LocalAlloc( LMEM_FIXED, cbPublicKeyInfo );
    if ( pPublicKeyInfo == NULL )
    {
        fwprintf( stderr, L"CreateKeyContainer: LocalAlloc pPublicKeyInfo Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    if ( !CryptExportPublicKeyInfo( hCryptProv,
                                    pKeyProvInfo->dwKeySpec,
                                    CERT_ENCODING_TYPE,
                                    pPublicKeyInfo,
                                    &cbPublicKeyInfo ) )
    {
        fwprintf( stderr, L"CreateKeyContainer: CryptExportPublicKeyInfo 2 Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    //
    // Assign the out param
    //
    *ppPublicKeyInfo = pPublicKeyInfo;

    pPublicKeyInfo = NULL;
    pwszKeyContainerName = NULL;

    result = TRUE;

Cleanup:
    if ( hKey != NULL )
    {
        //
        // Release the key handle. The key pair is not destroyed by this function.
        //
        CryptDestroyKey( hKey );
        hKey = NULL;
    } 

    if ( hCryptProv != NULL ) 
    {
        //
        // Release the handle to the key container.
        //
        CryptReleaseContext( hCryptProv, 
                             0 );          // reserved; must be 0
        hCryptProv = NULL;
    }

    if ( pPublicKeyInfo != NULL )
    {
        LocalFree( pPublicKeyInfo );
        pPublicKeyInfo = NULL;
    }

    if ( pwszKeyContainerName != NULL )
    {
        LocalFree( pwszKeyContainerName );
        pwszKeyContainerName = NULL;
    }

    return result;
}

//
// Summary:
// This function creates an X.500 subject name given the string subject name.
//
// Contract: The pbData field of the pSubjectIssuerBlob must be freed by the caller using LocalFree().
//
BOOL EncodeCertificateSubject( __in LPWSTR pwszCertSubjectName,
                               __out PCERT_NAME_BLOB pSubjectIssuerBlob )
{
    DWORD cbEncoded = 0;
    BYTE *pbEncoded = NULL;
    LPWSTR pwszX500 = NULL;
    BOOL result = FALSE;
    
    if ( pSubjectIssuerBlob == NULL )
    {
        fwprintf( stderr, L"EncodeCertificateSubject: pSubjectIssuerBlob is null.\n" );
        goto Cleanup;
    }

    //
    // Allocate the X500 name
    //
    LPCWSTR pwszCNPrefix = L"CN=";
    const size_t CN_PREFIX_LENGTH = 3;  // length of pwszCNPrefix

    size_t cchCertSubjectNameLength = 0;

    HRESULT hr = StringCchLengthW( pwszCertSubjectName, MAX_CERT_SUBJECT_NAME_LENGTH, &cchCertSubjectNameLength );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"EncodeCertificateSubject: StringCchLengthW Error 0x%x\n", hr );
        goto Cleanup;
    }

    DWORD dwLength = (DWORD)( CN_PREFIX_LENGTH + cchCertSubjectNameLength + 1 );

    pwszX500 = ( LPWSTR )LocalAlloc( LMEM_FIXED, dwLength * sizeof( WCHAR ) );

    if ( pwszX500 == NULL )
    {
        fwprintf( stderr, L"EncodeCertificateSubject: LocalAlloc Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    hr = StringCchPrintfW( pwszX500, dwLength, L"%s%s", pwszCNPrefix, pwszCertSubjectName );
    if ( FAILED( hr ) )
    {
        fwprintf( stderr, L"EncodeCertificateSubject: StringCchPrintfW Error 0x%x\n", hr );
        goto Cleanup;
    }

    if ( !CertStrToNameW( X509_ASN_ENCODING,
                          pwszX500,
                          CERT_X500_NAME_STR,
                          NULL,
                          NULL,
                          &cbEncoded,
                          NULL) )
    {
        fwprintf( stderr, L"EncodeCertificateSubject: CertStrToNameW 1 Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    pbEncoded = ( BYTE *)LocalAlloc( LMEM_FIXED, cbEncoded );
    if ( pbEncoded == NULL )
    {
        fwprintf( stderr, L"EncodeCertificateSubject: LocalAlloc Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    if ( !CertStrToNameW( X509_ASN_ENCODING,
                          pwszX500,
                          CERT_X500_NAME_STR,
                          NULL,
                          pbEncoded,
                          &cbEncoded,
                          NULL) )
    {
        fwprintf( stderr, L"EncodeCertificateSubject: CertStrToNameW 2 Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    //
    // Prepare the blob.
    //
    pSubjectIssuerBlob->cbData = cbEncoded;
    pSubjectIssuerBlob->pbData = pbEncoded;

    pbEncoded = NULL;

    result = TRUE;

Cleanup:
    if ( pwszX500 != NULL )
    {
        LocalFree( pwszX500 );
        pwszX500 = NULL;
    }

    if ( pbEncoded != NULL )
    {
        LocalFree( pbEncoded );
        pbEncoded = NULL;
    }

    return result;
}

//
// Summary:
// Prints the command line usage of this tool.
//
void PrintUsage()
{
    fwprintf( stderr, L"Invalid Arguments.\n\n" );
    fwprintf( stderr, L"Usage: WifMakeCert.exe Machine|User CERTSUBJECTNAME CERTFILEPATH\n" );
}

//
// Summary:
// Saves a certificate context to the specified path.
//
BOOL SaveCertificateToFile( __in PCCERT_CONTEXT pCertContext,
                            __in LPWSTR pwszFilePath )
{
    BOOL result = FALSE;
    HANDLE hFile = INVALID_HANDLE_VALUE;

    hFile = CreateFile( pwszFilePath,
                        GENERIC_WRITE,
                        0,
                        NULL,
                        CREATE_ALWAYS,
                        FILE_ATTRIBUTE_NORMAL,
                        NULL );
    if ( hFile == INVALID_HANDLE_VALUE )
    {
        fwprintf( stderr, L"SaveCertificateToFile: CreateFile Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }

    DWORD dwNumberOfBytesWritten = 0;

    if ( !WriteFile( hFile,
                     pCertContext->pbCertEncoded,
                     pCertContext->cbCertEncoded,
                     &dwNumberOfBytesWritten,
                     NULL ) )
    {
        fwprintf( stderr, L"SaveCertificateToFile: WriteFile Error 0x%x\n", GetLastError() );
        goto Cleanup;
    }
    
    if ( dwNumberOfBytesWritten != pCertContext->cbCertEncoded )
    {
        fwprintf( stderr, L"SaveCertificateToFile: WriteFile did not write all bytes.\n" );
        goto Cleanup;
    }

    result = TRUE;

Cleanup:
    if ( hFile != NULL )
    {
        CloseHandle( hFile );
        hFile = INVALID_HANDLE_VALUE;
    }

    return result;
}

//
// Summary:
// Main entry point to this application.
// Usage:
// WifMakeCert.exe Machine|User CERTSUBJECTNAME CERTFILEPATH
// Example:
// WifMakeCert.exe Machine localhost localhost.cer
//
int __cdecl wmain( int argc,
                   __in_ecount( argc ) WCHAR *argv[] )
{
    int result = 1;

    CRYPT_KEY_PROV_INFO KeyProvInfo = {};

    CERT_NAME_BLOB SubjectIssuerBlob = {};

    PCCERT_CONTEXT pCertContext = NULL;
    PCERT_PUBLIC_KEY_INFO pPublicKeyInfo = NULL;

    //
    // Process input
    //

    if( argc < 4 )
    {
        PrintUsage();
        goto Cleanup;
    }

    BOOL fMachineCertificate = FALSE;

    if ( _wcsicmp( argv[1], L"Machine" ) == 0 )
    {
        fMachineCertificate = TRUE;
    }
    else if ( _wcsicmp( argv[1], L"User" ) != 0 )
    {
        PrintUsage();
        goto Cleanup;
    }

    LPWSTR pwszCertSubjectName = argv[2];

    LPWSTR pwszCertFilePath = argv[3];

    //
    // Create the self-signed certificate as per the input parameters specified.
    //

    if ( !CreateKeyContainer( pwszCertSubjectName, fMachineCertificate, &KeyProvInfo, &pPublicKeyInfo ) )
    {
        goto Cleanup;
    }

    if ( !EncodeCertificateSubject( pwszCertSubjectName, &SubjectIssuerBlob ) )
    {
        goto Cleanup;
    }

    if ( !CreateCertificate( &KeyProvInfo, &SubjectIssuerBlob, pPublicKeyInfo, &pCertContext ) )
    {
        goto Cleanup;
    }

    if ( !AddCertificateToMyStore( pCertContext, fMachineCertificate ) )
    {
        goto Cleanup;
    }

    if ( !SaveCertificateToFile( pCertContext, pwszCertFilePath ) )
    {
        goto Cleanup;
    }

    result = 0;

Cleanup:
    if ( KeyProvInfo.pwszContainerName != NULL )
    {
        LocalFree( KeyProvInfo.pwszContainerName );
        KeyProvInfo.pwszContainerName = NULL;
    }

    if ( SubjectIssuerBlob.pbData != NULL )
    {
        LocalFree( SubjectIssuerBlob.pbData );
        SubjectIssuerBlob.pbData = NULL;
    }

    if ( pPublicKeyInfo != NULL )
    {
        LocalFree( pPublicKeyInfo );
        pPublicKeyInfo = NULL;
    }

    if ( pCertContext != NULL )
    {
        CertFreeCertificateContext( pCertContext );
        pCertContext = NULL;
    }

    return result;
}
