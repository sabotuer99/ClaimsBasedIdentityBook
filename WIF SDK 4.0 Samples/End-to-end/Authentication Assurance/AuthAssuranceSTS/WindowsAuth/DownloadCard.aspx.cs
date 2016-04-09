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
using System.Security.Principal;
using System.Threading;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

using AuthAssuranceSTS;

public partial class DownloadCard : System.Web.UI.Page
{
    protected void btnIssue_Click( object sender, EventArgs e )
    {
        X509Certificate2 userCertificate = null;

        //
        // Impersonate the caller and read the client certificate from the
        // caller's Personal certificate store.
        //
        WindowsPrincipal wp = Thread.CurrentPrincipal as WindowsPrincipal;
        WindowsIdentity wi = wp.Identity as WindowsIdentity;

        using ( WindowsImpersonationContext wctxt = wi.Impersonate() )
        {
            userCertificate = CertificateUtil.GetCertificate( StoreName.My, StoreLocation.CurrentUser, "CN=bobclient" );
        }

        X509Certificate2 signingCertificate = CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" );

        //
        // The WS-Identity schema requires cards to be signed using SHA-1.
        //
        X509SigningCredentials signingCredentials = new X509SigningCredentials( signingCertificate, SecurityAlgorithms.RsaSha1Signature, SecurityAlgorithms.Sha1Digest );

        //
        // Create an InformationCard
        //
        InformationCard card = new InformationCard( signingCredentials, CustomSecurityTokenServiceConfiguration.StsIssuerAddress );
        card.CardImage = new CardImage( Context.Request.PhysicalApplicationPath + @"\information-card.png" );
        card.CardName = "AuthAssuranceSTS Managed Card";
        card.TimeExpires = card.TimeIssued.Value + TimeSpan.FromDays( 7.0 ); // one week
        card.Language = "en-us";
        TokenServiceEndpoint tokenServiceEndpoint = new TokenServiceEndpoint( X509CardStsHostFactory.StsEndpointAddress,
                                                                              X509CardStsHostFactory.StsMexAddress,
                                                                              UserCredentialType.X509V3Credential,
                                                                              signingCertificate );
        card.TokenServiceList.Add( new TokenService( tokenServiceEndpoint,
                                                     new X509CertificateCredential( userCertificate ) ) );

        List<string> claimTypes = new List<string>();
        claimTypes.Add( ClaimTypes.Name );
        claimTypes.Add( ClaimTypes.DateOfBirth );
        claimTypes.Add( ClaimTypes.AuthenticationMethod );
        claimTypes.Add( ClaimTypes.PostalCode );
        claimTypes.Add( ClaimTypes.MobilePhone );

        card.SupportedClaimTypeList.AddRange( GetDisplayClaimsFromClaimTypes( claimTypes ) );

        //
        // This card can be used to request a SAML 1.1 token.
        //
        card.SupportedTokenTypeList.Add( Microsoft.IdentityModel.Tokens.SecurityTokenTypes.Saml11TokenProfile11 );

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
