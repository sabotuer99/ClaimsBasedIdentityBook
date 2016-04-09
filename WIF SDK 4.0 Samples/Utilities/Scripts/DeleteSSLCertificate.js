var metabasePath;
var site;

if ( WScript.Arguments.length < 1 ) 
{
    WScript.Echo( "Usage: cscript.exe DeleteSSLCertificate.js <SiteMetabasePath>" );
    WScript.Echo( "     SiteMetabasePath - e.g. /W3SVC/1 for default website" );
    WScript.Quit( 1 );
}

metabasePath = WScript.Arguments( 0 );

if ( !/^IIS:/i.test( metabasePath ) )
{
    if ( "/" != metabasePath.charAt( 0 ) )
    {
        metabasePath = "/" + metabasePath;
    }

    metabasePath = "IIS://localhost" + metabasePath;
}

site = GetObject( metabasePath );

site['SSLCertHash'] = "";
site.SetInfo();        
WScript.Echo( "The SSL certificate has be successfully removed" );

WScript.Quit( 0 );
