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
using System.IdentityModel.Selectors;
using System.IO;
using System.Threading;
using System.Web.UI;
using System.Xml;

using Microsoft.IdentityModel;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Web;

public partial class _Default : System.Web.UI.Page
{
    ServiceConfiguration _serviceConfiguration = FederatedAuthentication.ServiceConfiguration;

    void Page_Init( object sender, EventArgs e )
    {
        //
        // Because the audience URI is so strict, we must ensure that 
        //  the current URL the client is viewing includes the filename, not just
        //  the default path.
        //
        if( false == Page.IsPostBack && false == Request.Url.AbsolutePath.EndsWith( "/Default.aspx" ) )
        {
            UriBuilder builder = new UriBuilder( );
            builder.Scheme = Request.Url.Scheme;
            builder.Host = Request.Url.Host;
            builder.Path = ResolveUrl("~/Default.aspx");
            builder.Query = Request.Url.Query;
            Response.Redirect( builder.Uri.ToString() );
        }
        

        LoginError.Text = "";
        LoginError.Visible = false;
        SigninButton.Visible = true;
        SignedInMessage.Visible = false;
    }

    void Page_Load(object sender, EventArgs e)
    {
        if( Page.IsPostBack )
        {
            //
            // Process the token in this form field provided by the information card control
            //
            string tokenXml = Request.Form[ "tokenXml" ];
            if( false == String.IsNullOrEmpty( tokenXml ) )
            {
                try
                {
                    SecurityToken token = ReadXmlToken( tokenXml );
                    if ( null == token )
                    {
                        LoginError.Text = "Unable to process xml token.";
                    }
                    else
                    {
                        IClaimsPrincipal principal = AuthenticateSecurityToken( Request.RawUrl, token );
                        if ( principal == null )
                        {
                            LoginError.Text = "Unable to authenticate user.";
                        }
                        else
                        {
                            CreateLoginSession( principal, token );
                        }
                    }
                }
                catch ( SecurityTokenException exception )
                {
                    LoginError.Text = exception.ToString();
                    LoginError.Visible = true;
                }
            }
        }
    }

    /// <summary>
    /// Use the configured handlers to read the provided token.
    /// </summary>
    SecurityToken ReadXmlToken( string tokenXml )
    {
        using ( StringReader strReader = new StringReader( tokenXml ) )
        {
            using ( XmlDictionaryReader reader = XmlDictionaryReader.CreateDictionaryReader( XmlReader.Create( strReader ) ) )
            {
                reader.MoveToContent();
                return _serviceConfiguration.SecurityTokenHandlers.ReadToken( reader );
            }
        }
    }


    /// <summary>
    /// Validate the token received and create a ClaimsPrincipal accordingly.
    /// </summary>
    IClaimsPrincipal AuthenticateSecurityToken( string endpoint, SecurityToken token )
    {
        ClaimsIdentityCollection claims = _serviceConfiguration.SecurityTokenHandlers.ValidateToken( token );
        IClaimsPrincipal principal = ClaimsPrincipal.CreateFromIdentities( claims );

        //
        // the ClaimsAuthenticationManager may be provided to adjust the claims received specifically for this RP
        //
        return _serviceConfiguration.ClaimsAuthenticationManager.Authenticate( endpoint, principal );
    }


    /// <summary>
    /// Create a session token for the principal after authentication.
    /// </summary>
    void CreateLoginSession( IClaimsPrincipal principal, SecurityToken token )
    {
        WSFederationAuthenticationModule activeModule = new WSFederationAuthenticationModule( );
        activeModule.SetPrincipalAndWriteSessionToken(
                                    new SessionSecurityToken( principal, GetSessionLifetime( ) ),
                                    true
                                    );
    }


    /// <summary>
    /// The lifetime of the session is defaulted unless provided by the SessionSecurityTokenHandler.
    /// </summary>
    TimeSpan GetSessionLifetime()
    {
        TimeSpan lifetime = SessionSecurityTokenHandler.DefaultTokenLifetime;
        SessionSecurityTokenHandler ssth = _serviceConfiguration.SecurityTokenHandlers[typeof( SessionSecurityToken )] as SessionSecurityTokenHandler;
        if ( ssth != null )
        {
            lifetime = ssth.TokenLifetime;
        }

        return lifetime;
    }


    /// <summary>
    /// If user is authenticated, display the claims provided.
    /// </summary>
    void Page_PreRender( object sender, EventArgs e )
    {
        if ( Thread.CurrentPrincipal.Identity.IsAuthenticated )
        {
            SigninButton.Visible = false;
            SignedInMessage.Visible = true;
            SignedInMessage.Text = "Welcome back " + User.Identity.Name;
            ShowClaims( (IClaimsPrincipal)Thread.CurrentPrincipal );
        }
    }


    void ShowClaims( IClaimsPrincipal principal )
    {
        Panel1.Visible = true;
        ListBox1.Items.Clear( );

        /* For illustrative purposes this class implementation simply shows all the parameters of 
         * claims (i.e. claim types and claim values). In production code, security implications 
         * of echoing the properties of claims to the clients should be carefully considered. For 
         * example, some of the security considerations are: (i) accepting the only claim types 
         * that are expected by application; (ii) sanitizing the claim parameters before using 
         * them; and (iii) filtering out claims that contain sensitive personal information). 
         * DO NOT use this sample code ‘as is’ in production code.
        */
        //
        // Loop through the claims and add them to the list box
        //
        foreach( ClaimsIdentity identity in principal.Identities )
        {
            foreach( Claim claim in identity.Claims )
            {
                String newItem = claim.ClaimType + " : " + claim.Value;
                ListBox1.Items.Add( newItem );
            }
        }
    }
}
