var existingCertHash;
var hashString;
var metabasePath;
var site;

if ( WScript.Arguments.length < 2 ) 
{
    WScript.Echo( "Usage: cscript.exe DeleteSSLCertificate.js <SiteMetabasePath> <CertificateHash>" );
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

    if ( hashString === existingCertHash )
    {
        WScript.Echo( "Site has the requested certificate registered" );
        WScript.Echo( "Certificate hash           = " + existingCertHash );
        WScript.Echo( "Requested certificate hash = " + hashString );
        WScript.Echo( "The SSL certificate will be updated" );
        WScript.Quit( 0 ); 
    }
    else if ( existingCertHash !== "" )
    {
        WScript.Echo( "WARNING: Site already has a different certificate registered" );
        WScript.Echo( "Certificate hash           = " + existingCertHash );
        WScript.Echo( "Requested certificate hash = " + hashString );
        WScript.Echo( "Nothing has changed for the SSL configuration." );
    }
    else
    {
	WScript.Echo( "WARNING: Site does not have any certificate registered" );
        WScript.Echo( "Nothing has changed for the SSL configuration." );	
    }
}

WScript.Quit( 1 );

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
