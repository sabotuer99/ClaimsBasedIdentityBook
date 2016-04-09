using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.SecurityTokenService;

/// <summary>
/// A custom SecurityTokenServiceConfiguration implementation.
/// </summary>
public class MySecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
{
    static readonly object syncRoot = new object();
    static string MySecurityTokenServiceConfigurationKey = "MySecurityTokenServiceConfigurationKey";

    const string issuerName = "PassiveSigninSTS";
    const string SigningCertificateName = "CN=localhost";

    public static MySecurityTokenServiceConfiguration Current
    {
        get
        {
            HttpApplicationState httpAppState = HttpContext.Current.Application;

            MySecurityTokenServiceConfiguration myConfiguration = httpAppState.Get( MySecurityTokenServiceConfigurationKey ) as MySecurityTokenServiceConfiguration;

            if ( myConfiguration != null )
            {
                return myConfiguration;
            }
            
            lock ( syncRoot )
            {
                myConfiguration = httpAppState.Get( MySecurityTokenServiceConfigurationKey ) as MySecurityTokenServiceConfiguration;

                if ( myConfiguration == null )
                {
                    myConfiguration = new MySecurityTokenServiceConfiguration();
                    httpAppState.Add( MySecurityTokenServiceConfigurationKey, myConfiguration );
                }

                return myConfiguration;
            }
        }
    }

    /// <summary>
    /// MySecurityTokenServiceConfiguration constructor.
    /// </summary>
    public MySecurityTokenServiceConfiguration()
        : base( issuerName, new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, SigningCertificateName ) ) )
    {
        this.SecurityTokenService = typeof( MySecurityTokenService );
    }
}
