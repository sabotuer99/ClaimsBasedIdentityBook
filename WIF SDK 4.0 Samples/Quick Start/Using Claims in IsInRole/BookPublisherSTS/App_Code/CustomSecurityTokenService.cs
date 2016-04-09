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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

/// <summary>
/// A custom SecurityTokenService implementation.
/// </summary>
public class CustomSecurityTokenService : SecurityTokenService
{
    static readonly string EncryptingCertificateName = "CN=localhost";
    static readonly string ExpectedAppliesTo = "https://localhost/BookHostingService";

    /// <summary>
    /// Initializes an instance of CustomSecurityTokenService
    /// </summary>
    /// <param name="config">Configuration for the Security Token Service.</param>
    public CustomSecurityTokenService( SecurityTokenServiceConfiguration config )
        : base( config )
    {
    }

    /// <summary>
    /// Validates appliesTo and throws an exception if the appliesTo is null or appliesTo contains some unexpected address.
    /// </summary>
    /// <param name="appliesTo">The AppliesTo parameter in the request that came in (RST)</param>
    /// <exception cref="InvalidRequestException">The AppliesTo did not match the expected AppliesTo Uri.</exception>
    void ValidateAppliesTo( EndpointAddress appliesTo )
    {
        if ( appliesTo == null )
        {
            throw new InvalidRequestException( "The AppliesTo is null." );
        }

        // Throw if the appliesTo doesn't match either of the RPs for which this STS is meant to issue tokens.
        if ( !appliesTo.Uri.Equals( new Uri( ExpectedAppliesTo ) ) )
        {
            throw new InvalidRequestException( String.Format( "The AppliesTo address is not valid. Expected value is {0}, the actual value is {2}.",
                                                              ExpectedAppliesTo, appliesTo.Uri.AbsoluteUri ) );
        }
    }

    /// <summary>
    /// This method returns the configuration for the token issuance request. The configuration
    /// is represented by the Scope class. In our case, we are only capable of issuing a token to a
    /// single RP identity represented by the EncryptingCertificateName.
    /// </summary>
    /// <param name="principal">The caller's principal</param>
    /// <param name="request">The incoming RST</param>
    /// <returns>The scope information to be used for the token-issuance.</returns>
    protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
    {
        ValidateAppliesTo( request.AppliesTo );

        Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri, SecurityTokenServiceConfiguration.SigningCredentials );

        // Setting the encrypting credentials
        // Note: In this sample app, the certificate CN=localhost is used to encrypt the token for a single relying party.
        // In a production deployment, you would need to select the certificate that is specific to the RP that is requesting the token.
        scope.EncryptingCredentials = new X509EncryptingCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, EncryptingCertificateName ) );

        // Set the ReplyTo address for the WS-Federation passive protocol (wreply). This is the address to which responses will be directed. 
        // In this sample, we set this to the Default.aspx page on the Relying Party. 
        scope.ReplyToAddress = scope.AppliesToAddress + "/Default.aspx";

        return scope;
    }

    /// <summary>
    /// Gets the Name Claim for the given user.
    /// </summary>
    /// <param name="principal">Principal of the current user.</param>
    /// <returns>Value of the Name Claim of the user.</returns>
    /// <exception cref="UnauthorizedAccessException">No Name claim was found.</exception>
    string GetName( IClaimsPrincipal principal )
    {
        IClaimsIdentity claimsIdentity = (IClaimsIdentity)principal.Identity;
        IEnumerable<Claim> claimCollection = ( from c in claimsIdentity.Claims
                                               where c.ClaimType == ClaimTypes.Name
                                               select c );
        if ( claimCollection.Count<Claim>() > 0 )
        {
            return claimCollection.First<Claim>().Value;
        }
        
        throw new UnauthorizedAccessException( "Unable to find a valid Name Claim for the user." );
    }

    /// <summary>
    /// This method returns the claims to be issued in the token.
    /// </summary>
    /// <param name="scope">The scope information corresponding to this request.</param>
    /// <param name="principal">The caller's principal</param>
    /// <param name="request">The incoming RST.</param>
    /// <returns>The IClaimsIdentity to be included in the issued token.</returns>
    protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
    {
        ClaimsIdentity outputIdentity = new ClaimsIdentity();

        // Get the Name Claim.
        string name = GetName( principal );
        outputIdentity.Claims.Add( new Claim( ClaimTypes.Name, name ) );

        // Issue Custom Claims.
        // In this sample, we simply map Bob to Editor and everyone else (Joe) to Reviewer
        // In production, there would probably be a database to make these decisions.
        if ( StringComparer.OrdinalIgnoreCase.Equals( name, "Bob" ) )
        {
            outputIdentity.Claims.Add( new Claim( StsClaimTypes.RoleClaimType, "Editor", ClaimValueTypes.String ) );
        }
        else
        {
            outputIdentity.Claims.Add( new Claim( StsClaimTypes.RoleClaimType, "Reviewer", ClaimValueTypes.String ) );
        }

        outputIdentity.Claims.Add( new Claim( StsClaimTypes.BookNameClaimType, "FrameworkSamples.txt", ClaimValueTypes.String ) );

        return outputIdentity;
    }
}

