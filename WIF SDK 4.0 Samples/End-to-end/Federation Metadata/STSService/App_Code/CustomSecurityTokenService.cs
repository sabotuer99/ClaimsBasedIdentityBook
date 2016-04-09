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
using System.Web.Configuration;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

/// <summary>
/// A custom SecurityTokenService implementation.
/// </summary>
public class CustomSecurityTokenService : SecurityTokenService
{
    static bool enableAppliesToValidation = true;
    static readonly string[] ActiveClaimsAwareApps = { "http://localhost/ClaimsAwareService/QuoteService.svc" };

    /// <summary>
    /// Creates an instance of CustomSecurityTokenService.
    /// </summary>
    /// <param name="configuration">The SecurityTokenServiceConfiguration.</param>
    public CustomSecurityTokenService( SecurityTokenServiceConfiguration configuration )
        : base( configuration )
    {
    }

    /// <summary>
    /// Validates appliesTo and throws an exception if the appliesTo is null or appliesTo contains some unexpected address.
    /// </summary>
    /// <param name="appliesTo">The AppliesTo parameter in the request that came in (RST)</param>
    void ValidateAppliesTo( EndpointAddress appliesTo )
    {
        if ( appliesTo == null )
        {
            throw new InvalidRequestException( "The AppliesTo is null." );
        }

        if ( enableAppliesToValidation )
        {
            bool validAppliesTo = false;
            foreach ( string rpUrl in ActiveClaimsAwareApps )
            {
                if ( appliesTo.Uri.Equals( new Uri( rpUrl ) ) )
                {
                    validAppliesTo = true;
                    break;
                }
            }

            if ( !validAppliesTo )
            {
                throw new InvalidRequestException( String.Format( "The AppliesTo address {0} is not valid.", appliesTo.Uri.AbsoluteUri ) );
            }
        }
    }

    /// <summary>
    /// This method returns the configuration for the token issuance request. The configuration
    /// is represented by the Scope class. In our case, we are only capable of issuing a token for a
    /// single RP identity represented by the EncryptingCertificateName.
    /// </summary>
    /// <param name="principal">The caller's principal</param>
    /// <param name="request">The incoming RST</param>
    /// <returns>The scope information to be used for the token-issuance.</returns>
    protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
    {
        ValidateAppliesTo( request.AppliesTo );

        Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri, SecurityTokenServiceConfiguration.SigningCredentials );

        string encryptingCertificateName = WebConfigurationManager.AppSettings[ "EncryptingCertificateName" ];
        if ( !string.IsNullOrEmpty( encryptingCertificateName ) )
        {
            // Setting the encrypting credentials
            // Note: In this sample app, the same certificate (localhost) is used to encrypt the token.
            // In a production deployment, you would need to select the certificate that is specific to the RP that is requesting the token.
            scope.EncryptingCredentials = new X509EncryptingCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, encryptingCertificateName ) );
        }

        return scope;
    }


    /// <summary>
    /// This method returns the claims to be issued in the token.
    /// </summary>
    /// <param name="scope">The scope information corresponding to this request.</param>
    /// <param name="principal">The caller's principal</param>
    /// <param name="request">The incoming RST, we don't use this in our implementation</param>
    /// <returns>The outgoing claimsIdentity to be included in the issued token.</returns>
    protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
    {
        ClaimsIdentity outputIdentity = new ClaimsIdentity();

        if ( null == principal )
        {
            throw new InvalidRequestException( "The caller's principal is null." );
        }

        // Issue the common claims
        // NOTE: In this sample, these claims must match the claims actually generated in Common.GetFederationMetadata.
        //       In a production system, there would be some common data store that both use
        outputIdentity.Claims.Add( new Claim( ClaimTypes.Name, principal.Identity.Name ) );
        outputIdentity.Claims.Add( new Claim( ClaimTypes.Role, "Manager" ) );

        // Issue a custom claim if requested
        IEnumerable<RequestClaim> claimCollection = ( from c in request.SecondaryParameters.Claims
                                                      where c.ClaimType == Common.QuotationClassClaimType
                                                       select c );
        if ( claimCollection.Count<RequestClaim>() > 0 )
        {
            outputIdentity.Claims.Add( GetRandomQuoteClassClaim() );
        }

        return outputIdentity;
    }

    /// <summary>
    /// Generate a random quote class claim for sample purposes.
    /// </summary>
    /// <returns>An integer from 0-3.</returns>
    Claim GetRandomQuoteClassClaim()
    {
        int quoteClass = new Random().Next( 4 );
        return new Claim( Common.QuotationClassClaimType, quoteClass.ToString() );
    }
}
