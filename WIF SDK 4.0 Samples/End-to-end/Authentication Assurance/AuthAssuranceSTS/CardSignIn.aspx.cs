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
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Web.UI;
using System.Xml;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml11;
using Microsoft.IdentityModel.Web;

using AuthAssuranceSTS;

public partial class CardSignIn : System.Web.UI.Page
{
    SecurityTokenHandlerCollection _handlers;

    public CardSignIn()
    {
        SecurityTokenHandlerConfiguration handlerConfig = new SecurityTokenHandlerConfiguration();
        handlerConfig.IssuerNameRegistry = new TrustedIssuerNameRegistry();

        List<SecurityToken> servicetokens = new List<SecurityToken>( 1 );
        servicetokens.Add( new X509SecurityToken( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" ) ) );
        handlerConfig.ServiceTokenResolver = SecurityTokenResolver.CreateDefaultSecurityTokenResolver( servicetokens.AsReadOnly(), false );

        handlerConfig.AudienceRestriction.AllowedAudienceUris.Add( new Uri( "https://localhost/AuthAssuranceSTS/CardSignIn.aspx" ) );

        _handlers = new SecurityTokenHandlerCollection( handlerConfig );

        SamlSecurityTokenRequirement samlReqs = new SamlSecurityTokenRequirement();

        _handlers.Add( new EncryptedSecurityTokenHandler() );
        _handlers.Add( new Saml11SecurityTokenHandler( samlReqs ) );
    }

    void Page_Load( object sender, EventArgs e )
    {
        if ( Page.IsPostBack )
        {
            string tokenXml = Request.Form["tokenXml"];
            if ( !String.IsNullOrEmpty( tokenXml ) )
            {
                try
                {
                    SecurityToken token = ReadXmlToken( tokenXml );

                    if ( token == null )
                    {
                        SetLoginErrorText( "Unable to process xml token." );
                    }
                    else
                    {
                        IClaimsPrincipal principal = AuthenticateSecurityToken( Request.RawUrl, token );

                        if ( principal == null )
                        {
                            SetLoginErrorText( "Unable to authenticate user." );
                        }
                        else
                        {
                            FederatedPassiveSecurityTokenServiceOperations.ProcessRequest( Request, principal, CustomSecurityTokenServiceConfiguration.Current.CreateSecurityTokenService(), Response );
                        }
                    }
                }
                catch ( SecurityTokenException exception )
                {
                    SetLoginErrorText( exception.ToString() );
                }
            }
        }
    }

    void SetLoginErrorText( string errorText )
    {
        LoginError.Text = errorText;
        LoginError.Visible = true;
    }

    SecurityToken ReadXmlToken( string tokenXml )
    {
        using ( StringReader strReader = new StringReader( tokenXml ) )
        {
            using ( XmlDictionaryReader reader = XmlDictionaryReader.CreateDictionaryReader( XmlReader.Create( strReader ) ) )
            {
                reader.MoveToContent();
                return _handlers.ReadToken( reader );
            }
        }
    }

    IClaimsPrincipal AuthenticateSecurityToken( string endpoint, SecurityToken token )
    {
        ClaimsIdentityCollection claims = _handlers.ValidateToken( token );

        IClaimsPrincipal principal = ClaimsPrincipal.CreateFromIdentities( claims );
        return CustomSecurityTokenServiceConfiguration.Current.ClaimsAuthenticationManager.Authenticate( endpoint, principal );
    }

}
