<%@ Application Language="C#" %>
<%@ Import Namespace="System.Threading" %>
<%@ Import Namespace="Microsoft.IdentityModel.Claims" %>
<%@ Import Namespace="Microsoft.IdentityModel.Web" %>

<script RunAt="server">

    //
    // This function handles the case where the user is authenticated,
    // but there is an authorization failure.
    //        
    void WSFederationAuthenticationModule_AuthorizationFailed(object sender, AuthorizationFailedEventArgs e)
    {
        if ( Thread.CurrentPrincipal.Identity.IsAuthenticated && 
             StringComparer.OrdinalIgnoreCase.Equals( HttpContext.Current.Request.Url.ToString(), CustomClaimsAuthorizationManager.HighValueResourceUrl ) )
        {
            //
            // The user is authenticated, but not authorized to access high value resources.
            // Redirect to the identity provider for stronger authentication.
            // Delete the current session token cookies as they are not useful for this transaction.
            //
            e.RedirectToIdentityProvider = true;
            FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
        }
    }

    void WSFederationAuthenticationModule_RedirectingToIdentityProvider( object sender, RedirectingToIdentityProviderEventArgs e )
    {
        if ( StringComparer.OrdinalIgnoreCase.Equals( HttpContext.Current.Request.Url.ToString(), CustomClaimsAuthorizationManager.HighValueResourceUrl ) )
        {
            //
            // If the request is for high value resources, request the 
            // identity provider for an Authentication type of X509.
            //
            e.SignInRequestMessage.AuthenticationType = AuthenticationMethods.X509;
        }
    }

</script>

