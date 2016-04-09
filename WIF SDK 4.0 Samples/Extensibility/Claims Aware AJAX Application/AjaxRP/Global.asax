<%@ Application Language="C#" %>
<%@ Import Namespace="System.Security.Cryptography.X509Certificates" %>
<%@ Import Namespace="System.IdentityModel.Claims" %>
<%@ Import Namespace="System.IdentityModel.Policy" %>
<%@ Import Namespace="Microsoft.IdentityModel" %>
<%@ Import Namespace="Microsoft.IdentityModel.Tokens" %>
<%@ Import Namespace="Microsoft.IdentityModel.Web" %>

<script RunAt="server">

    void Application_Start( object sender, EventArgs e )
    {
        // Code that runs on application startup

    }

    void Application_End( object sender, EventArgs e )
    {
        //  Code that runs on application shutdown

    }

    void Application_Error( object sender, EventArgs e )
    {
        // Code that runs when an unhandled error occurs

    }

    void Session_Start( object sender, EventArgs e )
    {
        // Code that runs when a new session is started

    }

    void Session_End( object sender, EventArgs e )
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }

    void WSFederationAuthenticationModule_ConfigurationLoaded( object sender, EventArgs e )
    {
        FederatedAuthentication.ServiceConfiguration.SecurityTokenHandlers.AddOrReplace(
            new SessionSecurityTokenHandler( SessionSecurityTokenHandler.DefaultTransforms, new MruSecurityTokenCache(), new TimeSpan( 0, 0, 10 ) )
            );
    }

    void WSFederationAuthenticationModule_RedirectingToIdentityProvider( object sender, RedirectingToIdentityProviderEventArgs e )
    {
        if ( StringComparer.OrdinalIgnoreCase.Equals( HttpContext.Current.Request.Url.AbsolutePath, "https://localhost/AjaxSample_RP/callback.aspx" ) )
        {
            e.Cancel = true;
            Response.Flush();
            Response.End();
        }
    }
    
    void SessionAuthenticationModule_SessionSecurityTokenReceived( object sender, SessionSecurityTokenReceivedEventArgs e )
    {
        // To show what happens when a cookie expires in an AJAX app, the code below sets up the cookie to expire after 10 seconds from the creation time of the cookie.
        DateTime now = DateTime.UtcNow;
        DateTime expirationTime = e.SessionToken.ValidFrom + new TimeSpan( 0, 0, 10 );
        if ( now > expirationTime )
        {
            // If the cookie has expired, the status code is set to a custom code: 440. 
            // The status description is set to something relevant.
            // The cookie associated with this request is deleted. 
            // When the AJAX client side script code receives this, it can take action 
            // based on the session StatusCode
            Response.StatusCode = 440;
            Response.StatusDescription = "Timed out";
            FederatedAuthentication.SessionAuthenticationModule.CookieHandler.Delete();
            Response.Write( "Timed out" );
            e.Cancel = true;
        }
    }
    
       
</script>

