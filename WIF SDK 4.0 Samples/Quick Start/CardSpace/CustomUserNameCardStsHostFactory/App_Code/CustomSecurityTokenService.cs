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
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSIdentity;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;


/// <summary>
/// Summary description for CustomSecurityTokenService
/// </summary>
public class CustomSecurityTokenService : SecurityTokenService
{
    public const string ExpectedAddress = "https://localhost/CustomUserNameCardStsHostFactoryWebSite/Default.aspx";

    Dictionary<string, string> localizedDisplayClaimTable;

    /// <summary>
    /// The constructor.
    /// </summary>
    /// <param name="config">The <see cref="SecurityTokenServiceConfiguration"/> object to be 
    /// passed to the base class.</param>
    public CustomSecurityTokenService( SecurityTokenServiceConfiguration config )
        : base( config )
    {
        // Set up the localized DisplayClaim table for the "Given Name" display-tag in English, Italian, Spanish, and French locales
        localizedDisplayClaimTable = new Dictionary<string, string>();
        localizedDisplayClaimTable.Add( "en-US", "Given Name" );
        localizedDisplayClaimTable.Add( "it-IT", "Nome di Battesimo" );
        localizedDisplayClaimTable.Add( "es-ES", "Nombre de Pila" );
        localizedDisplayClaimTable.Add( "fr-FR", "Prénom" );
    }

    /// <summary>
    /// Override this method to provide scope specific encrypting credentials.
    /// </summary>
    /// <param name="principal">The principal.</param>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
    {
        //
        // Verify that the target service is valid for this STS
        //
        ValidateAppliesTo( request.AppliesTo );

        Scope scope = new Scope( request.AppliesTo.Uri.ToString() );

        scope.SigningCredentials = new X509SigningCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" ) );

        // In this sample app only a single RP identity is shown, which is localhost, 
        // and the certificate of that RP is populated as EncryptingCredentials.
        // If you have multiple RPs for the STS you would select the certificate 
        // that is specific to the RP that requests the token and then use that for EncryptingCredentials.
        scope.EncryptingCredentials = new X509EncryptingCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, "CN=localhost" ) );

        return scope;
    }

    /// <summary>
    /// This overriden method returns a collection of output subjects to be included in the issued token.
    /// </summary>
    /// <param name="principal">The principal that represents the identity of the requestor.</param>
    /// <param name="request">The token request parameters that arrived in the call.</param>
    /// <param name="scope">The scope information about the Relying Party.</param>
    /// <returns>The claims collection that will be placed inside the issued token.</returns>        
    protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
    {
        List<Claim> claims = new List<Claim>();
        foreach ( RequestClaim requestedClaim in request.Claims )
        {
            Claim claim = GetClaim( requestedClaim.ClaimType, principal, scope, request );

            if ( claim != null )
            {
                claims.Add( claim );
            }
            else if ( requestedClaim.IsOptional )
            {
                claims.Add( new Claim( requestedClaim.ClaimType, "N/A" ) );
            }
            else
            {
                throw new NotSupportedException( "Server has no claim value found for required claim: " + requestedClaim.ClaimType );
            }
        }

        return new ClaimsIdentity( claims );
    }

    /// <summary>
    /// Gets the DisplayToken.
    /// </summary>
    /// <param name="requestedDisplayTokenLanguage">The requested DisplayToken language.  This is an optional parameter and may be null.</param>
    /// <param name="subject">The IClaimsIdentity representing the collection of claims that will be placed in the issued security token.</param>
    /// <returns>The DisplayToken to be included in the response.</returns>
    /// <remarks>
    /// This operation might expose information about the user. The code below is intended for illustrative purposes only.
    /// </remarks>
    protected override DisplayToken GetDisplayToken( string requestedDisplayTokenLanguage, IClaimsIdentity subject )
    {
        string localizedDisplayTag;
        // Try looking up the localized DisplayClaim table for the requestedDisplayTokenLanguage
        if ( !localizedDisplayClaimTable.TryGetValue( requestedDisplayTokenLanguage, out localizedDisplayTag ) )
        {
            localizedDisplayTag = "Given Name"; // default to English
        }
        DisplayClaimCollection collection = new DisplayClaimCollection();
        DisplayClaim claim = new DisplayClaim( ClaimTypes.GivenName );
        claim.DisplayTag = localizedDisplayTag; // The input subject can be used to further qualify the DisplayClaim being generated, such as including an optional DisplayValue.
        collection.Add( claim );
        DisplayToken token = new DisplayToken( requestedDisplayTokenLanguage, collection );

        return token;
    }

    /// <summary>
    /// Returns a claim based on the requested claim type from the security token request.
    /// </summary>
    /// <param name="requestedClaimType">The requested claim type</param>
    /// <param name="principal">The IClaimsPrincipal that represents the identity of the requestor.</param>
    /// <param name="scope">The scope information about the Relying Party.</param>
    /// <param name="request">The token request parameters that arrived in the call.</param>
    /// <returns>The claim that matches the requested claim type.</returns>
    private Claim GetClaim( string requestedClaimType, IClaimsPrincipal principal, Scope scope, RequestSecurityToken request )
    {
        IClaimsIdentity identity = ( IClaimsIdentity )principal.Identity;

        foreach ( Claim claim in identity.Claims )
        {
            if ( requestedClaimType == claim.ClaimType )
            {
                return claim;
            }
        }

        return null;
    }


    /// <summary>
    /// Validates the appliesTo and throws an exception if the appliesTo is null or appliesTo contains some unexpected address.  
    /// </summary>
    void ValidateAppliesTo( EndpointAddress appliesTo )
    {
        if ( appliesTo == null )
        {
            throw new InvalidRequestException( "The appliesTo is null." );
        }

        if ( !appliesTo.Uri.Equals( new Uri( ExpectedAddress ) ) )
        {
            throw new InvalidRequestException( String.Format( "The relying party address is not valid. Expected value is {0}, the actual value is {1}.", ExpectedAddress, appliesTo.Uri.AbsoluteUri ) );
        }
    }
}
