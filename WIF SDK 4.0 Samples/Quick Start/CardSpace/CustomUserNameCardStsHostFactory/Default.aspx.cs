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
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

public partial class _Default : System.Web.UI.Page
{
    protected void btnIssue_Click( object sender, EventArgs e )
    {
        X509Certificate2 cert = CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" );

        //
        // The WS-Identity schema requires cards to be signed using SHA-1.
        //
        X509SigningCredentials signingCredentials = new X509SigningCredentials( cert, SecurityAlgorithms.RsaSha1Signature, SecurityAlgorithms.Sha1Digest );

        //
        // Create an InformationCard
        //
        InformationCard card = new InformationCard( signingCredentials, UserNameCardStsHostFactory.StsAddress );
        card.CardImage = new CardImage( Context.Request.PhysicalApplicationPath + @"\information-card.png" );
        card.CardName = "CustomUserNameCardStsHostFactory Managed Card";
        card.TimeExpires = card.TimeIssued.Value + TimeSpan.FromDays( 7.0 ); // one week
        card.Language = "en-us";
        card.TokenServiceList.Add( new TokenService( new TokenServiceEndpoint( UserNameCardStsHostFactory.StsAddress, UserNameCardStsHostFactory.StsMexAddress, 
                                                                                UserCredentialType.UserNamePasswordCredential, cert ),
                                                     new UserNamePasswordCredential( "terry" ) ) );
        List<string> claimTypes = new List<string>();
        claimTypes.Add( ClaimTypes.GivenName );
        claimTypes.Add( ClaimTypes.Surname );
        claimTypes.Add( ClaimTypes.PPID );
        claimTypes.Add( ClaimTypes.HomePhone );            

        card.SupportedClaimTypeList.AddRange( GetDisplayClaimsFromClaimTypes( claimTypes ) );

        IEnumerable<SecurityTokenHandlerCollection> handlersList = FederatedAuthentication.ServiceConfiguration.SecurityTokenHandlerCollectionManager.SecurityTokenHandlerCollections;
        foreach( SecurityTokenHandlerCollection handlers in handlersList )
        {
            if( null != handlers )
            {
                foreach( string tokenType in handlers.TokenTypeIdentifiers )
                {
                    card.SupportedTokenTypeList.Add( tokenType );
                }
            }
        }

        //
        // This writes the card out in XML as a .CRD file into the HTTP response.
        // Internet Explorer will recognize the MIME type and prompt the user.
        // 
        InformationCardSerializer serializer = new InformationCardSerializer();
        Response.ClearContent();
        Response.AddHeader( "Content-Disposition", "attachment; filename=InformationCard.crd" );
        Response.ContentType = "application/x-informationcardfile";
        serializer.WriteCard( Response.OutputStream, card );
        Response.End();
    }

    DisplayClaimCollection GetDisplayClaimsFromClaimTypes( IEnumerable<string> claimTypes )
    {
        DisplayClaimCollection result = new DisplayClaimCollection();

        foreach ( string claimType in claimTypes )
        {
            result.Add( new DisplayClaim( claimType ) );
        }

        return result;
    }

}
