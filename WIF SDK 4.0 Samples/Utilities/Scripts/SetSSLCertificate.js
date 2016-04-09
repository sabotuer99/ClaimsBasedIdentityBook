var existingCertHash;
var hashString;
var metabasePath;
var site;
var existingBinding;
var site1;
var objShell;
var isVista;

if ( WScript.Arguments.length < 2 ) 
{
    WScript.Echo( "Usage: cscript.exe SetSSLCertificate.js <SiteMetabasePath> <CertificateHash>" );
    WScript.Echo( "     SiteMetabasePath - e.g. /W3SVC/1 for default website" );
    WScript.Echo( "     CertificateHash  - Hex string of SHA-1 hash, e.g." );
    WScript.Echo( "         97249e1a5fa6bee5e515b82111ef524a4c91583f" );
    WScript.Quit( 1 );
}

metabasePath = WScript.Arguments( 0 );
hashString   = WScript.Arguments( 1 );

if ( !/^IIS:/i.test( metabasePath ) )
{
    if ( "/" != metabasePath.charAt( 0 ) )
    {
        metabasePath = "/" + metabasePath;
    }

    metabasePath = "IIS://localhost" + metabasePath;
}

site = GetObject( metabasePath );

WScript.Echo( "Examining the SSL certificate settings for " + metabasePath );
WScript.Echo();

existingCertHash = site['SSLCertHash'];
if ( null !== existingCertHash )
{
    existingCertHash = adsiBinaryToHexString( existingCertHash );
    
    
    if ( existingCertHash !== "" ) {
       WScript.Echo( "WARNING: Site already has a certificate registered" );
       WScript.Echo( "Certificate hash           = " + existingCertHash );
    }
    else {
       WScript.Echo( "WARNING: Site does not have any SSL certificate registered" );
    }

    WScript.Echo( "Requested certificate hash = " + hashString );
    WScript.Echo( "Press Ctrl-C to stop or Press ENTER to update the existing SSL configuration for both IIS and HTTP.SYS . . ." );
    WScript.StdIn.ReadLine();
}

// here we will clear out the http.sys ssl cert registration for port 443
objShell = WScript.CreateObject("WScript.Shell");
isVista = objShell.ExpandEnvironmentStrings("%IsVista%");
if ( isVista !== "true" ) 
{
   objShell.Run("httpcfg.exe delete ssl -i 0.0.0.0:443", 0, 1);
}

// now let us update the IIS setting
existingBinding = site['SecureBindings'];
if ( existingBinding !== null )
{
    site.PutEx( 1, "SecureBindings", null );
    site.SetInfo();
}

// retrieve the updated site object which does not have secureBinding any more
site1 = GetObject( metabasePath );

site1['SSLCertHash']    = bytesToAdsiBinary( hexStringToBytes( hashString ) );
site1['SSLStoreName']   = "MY";
site1['SecureBindings'] = ":443:";
site1.SetInfo();

WScript.Echo( "The SSL configuration has been successfully updated" );

WScript.Quit( 0 );

//
// This takes the SAFEARRAY returned by ADSI and converts it into a
// hex string.
//
function adsiBinaryToHexString( adsiArray )
{
    var i, realArray, hexString, hexChar;

    realArray = new VBArray( adsiArray ).toArray();

    hexString = "";
    for ( i = 0; i < realArray.length; i++ )
    {
        hexChar = realArray[i].toString( 16 );
        if ( 1 === hexChar.length )
        {
            hexString += "0";
        }
        hexString += hexChar;
    }
    return hexString;
}

//
// This takes a byte array like [ 0xab,0xcd,0xef,0x01 ] and will produce a 
// string where each character has a value which is the byte-order 
// corrected value of two of the characters from the byte array. So given
// the example array, the output of this fucntion is equivalent to
// String.fromCharCode( 0xcdab, 0x01ef ). 
//
function bytesToAdsiBinary( bytes )
{
    var i, binaryString;
    
    if ( 0 != bytes.length % 2 )
    {
        throw "Byte array must have an even number of bytes";
    }

    binaryString = "";
    for ( i = 0; i < bytes.length / 2; i++ )
    {
        binaryString += String.fromCharCode( ( bytes[2*i] | (bytes[2*i + 1] << 8) ) );
    }

    return binaryString;
}

//
// This simply chops up a hex string like "abcdef01" into an array of the byte
// values like [ 0xab,0xcd,0xef,0x01 ].
//
function hexStringToBytes( hexString )
{
    var i, values;

    if ( 0 != hexString.length % 2 )
    {
        throw "Hex string must have an even number of characters";
    }

    values = new Array();
    for ( i = 0; i < hexString.length / 2; i++ )
    {
        values.push( parseInt( hexString.substring( 2*i, 2*i + 2 ), 16 ) );
    }
    return values;
}
