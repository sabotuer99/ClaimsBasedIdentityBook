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

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// Implementation of a ClaimsAuthenticationManager. The class is called after an incoming token
/// has been validated. This ClaimsAuthenticationManager converts the incoming claims from the 
/// BookPublisherSTS to claims that is understandable within the Realm of the BookHostingService.
/// </summary>
public class CustomClaimsAuthenticationManager : ClaimsAuthenticationManager
{
    /// <summary>
    /// Converts the claims in the 'incomingPrincipal' to Claims that are understandable at the
    /// BookHostingService.
    /// </summary>
    /// <param name="endpointUri">The endpoint URI the Client is trying to access.</param>
    /// <param name="incomingPrincipal">The Principal generated from validating the incoming token.</param>
    /// <returns>Returns a principal with augmented claims.</returns>
    /// <exception cref="ArgumentNullException">The input argument 'incomingPrincipal' is null.</exception>
    public override IClaimsPrincipal Authenticate( string endpointUri, IClaimsPrincipal incomingPrincipal )
    {
        if ( incomingPrincipal == null )
        {
            throw new ArgumentNullException( "incomingPrincipal" );
        }

        // If the identity not authenticated yet, keep this principal and let it redirect to the STS
        if ( !incomingPrincipal.Identity.IsAuthenticated )
        {
            return incomingPrincipal;
        }

        List<IClaimsIdentity> claimsIdentities = new List<IClaimsIdentity>();

        foreach ( IClaimsIdentity identity in incomingPrincipal.Identities )
        {

            if ( identity.Claims.Count == 0 )
            {
                throw new InvalidOperationException( "The incoming token did not create any Claims." );
            }

            // Find the Issuer.
            string issuer = identity.Claims[0].Issuer;

            // Get the Role Claim Type and the Name Claim Type of this Issuer.
            string issuerRoleClaimType = GetIssuerRoleClaimType( issuer );
            string issuerBookNameClaimType = GetIssuerBookNameClaimType( issuer );

            // Look for Role and BookName claims types and re-issue these claims with ClaimTypes 
            // that this service understands.
            // While reissuing claims we do the following,
            // 1. Set the Claim.Issuer to this service name. "BookHostingService".
            // 2. Set the Claim.OriginalIssuer to the BookPublisherSTS name.
            // 3. Set the Claim.Type to a type that is recognized inside the HostingService's Realm.
            // 4. Set the Claim.Value and Claim.ValueType to the original Claim's values.
            List<Claim> reissuedClaims = new List<Claim>();
            foreach ( Claim c in identity.Claims )
            {
                Claim newClaim = null;
                if ( StringComparer.Ordinal.Equals( c.ClaimType, issuerRoleClaimType ) )
                {
                    newClaim = new Claim( ServiceClaimTypes.RoleClaimType, c.Value, c.ValueType, "BookHostingService", c.Issuer );
                }
                else if ( StringComparer.Ordinal.Equals( c.ClaimType, issuerBookNameClaimType ) )
                {
                    newClaim = new Claim( ServiceClaimTypes.BookNameClaimType, c.Value, c.ValueType, "BookHostingService", c.Issuer );
                }
                else
                {
                    newClaim = c;
                }

                reissuedClaims.Add( newClaim );
            }

            // Create an Identity with the the new Claims. Set the Role Claims type
            // correctly on the Identity that is created.
            ClaimsIdentity claimIdentity = new ClaimsIdentity( reissuedClaims, "Federated" );
            claimIdentity.RoleClaimType = ServiceClaimTypes.RoleClaimType;

            claimsIdentities.Add( claimIdentity );
        }

        // Return a new principal wrapping the reissued claims.
        return new ClaimsPrincipal( claimsIdentities );
    }

    /// <summary>
    /// Returns the Claim Type that is published as the Role Claim type for
    /// the given Issuer.
    /// </summary>
    /// <param name="issuer">Name of the Issuer.</param>
    /// <returns>Role Claim Type published by the issuer.</returns>
    /// <exception cref="InvalidOperationException">The given issuer is not a recognized issuer.</exception>
    string GetIssuerRoleClaimType( string issuer )
    {
        // Check for known issuer.
        if ( !StringComparer.Ordinal.Equals( issuer, "BookPublisherSts" ) )
        {
            throw new InvalidOperationException( String.Format( "The Issuer '{0}' is unknown.", issuer ) );
        }

        return StsClaimTypes.RoleClaimType;
    }

    /// <summary>
    /// Returns the Claim Type that is published as the Book name Claim type for
    /// the given Issuer.
    /// </summary>
    /// <param name="issuer">Name of the Issuer.</param>
    /// <returns>Book name Claim Type published by the issuer.</returns>
    /// <exception cref="InvalidOperationException">The given issuer is not a recognized issuer.</exception>
    string GetIssuerBookNameClaimType( string issuer )
    {
        // Check for known issuer.
        if ( !StringComparer.Ordinal.Equals( issuer, "BookPublisherSts" ) )
        {
            throw new InvalidOperationException( String.Format( "The Issuer '{0}' is unknown.", issuer ) );
        }

        return StsClaimTypes.BookNameClaimType;
    }
}
