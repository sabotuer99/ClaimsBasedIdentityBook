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
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

/// <summary>
/// A custom SecurityTokenService implementation.
/// </summary>
public class MySecurityTokenService : SecurityTokenService
{
    static readonly string EncryptingCertificateName = "CN=localhost";
    static readonly string PassiveRedirectBasedClaimsAwareWebApp = "https://localhost/PassiveRedirectBasedClaimsAwareWebAppInWebFarm/";

    /// <summary>
    /// Creates an instance of MySecurityTokenService.
    /// </summary>
    /// <param name="configuration">The SecurityTokenServiceConfiguration.</param>
    public MySecurityTokenService( SecurityTokenServiceConfiguration configuration )
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

        // Throw if the appliesTo doesn't match either of the RPs for which this STS is meant to issue tokens.
        if ( !appliesTo.Uri.Equals( new Uri( PassiveRedirectBasedClaimsAwareWebApp ) ) )
        {
            throw new InvalidRequestException( String.Format( "The AppliesTo address is not valid. Expected value is {0}, the actual value is {1}.", 
                                                              PassiveRedirectBasedClaimsAwareWebApp, appliesTo.Uri.AbsoluteUri ) );
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
        // Note: In this sample app, the same certificate (localhost) is used to encrypt the token for 
        // PassiveRedirectBasedClaimsAwareWebApp
        // In a production deployment, you would need to select the certificate that is specific to the RP that is requesting the token.
        scope.EncryptingCredentials = new X509EncryptingCredentials( CertificateUtil.GetCertificate( StoreName.My, StoreLocation.LocalMachine, EncryptingCertificateName ) );

        // Set the ReplyTo address for the WS-Federation passive protocol (wreply). This is the address to which responses will be directed. 
        // In this sample, we set this to the Default.aspx page on the Relying Party. Note that this is not used in the WS-Trust active case.
        scope.ReplyToAddress = scope.AppliesToAddress + "/Default.aspx";

        return scope;
    }
    
    /// <summary>
    /// This method computes the NT Account name translated from the primary sid claim found
    /// in the provided claims principal.
    /// </summary>
    /// <param name="principal">The claims principal containing the primary sid claim.</param>
    /// <returns>The NT Account name corresponding to the primary sid claim found in the claims principal.</returns>
    private string GetNTAccountName( IClaimsPrincipal principal )
    {
        IClaimsIdentity claimsIdentity = principal.Identity as IClaimsIdentity;

        // The STS has Windows Authentication enabled. With this mode of authentication
        // the user will have a PrimarySid claim.
        List<Claim> primarySidsFound = ( from c in claimsIdentity.Claims
                                         where c.ClaimType == ClaimTypes.PrimarySid
                                         select c ).ToList();

        if ( primarySidsFound.Count == 1 )
        {
            Claim primarySidClaim = primarySidsFound[0];
            SecurityIdentifier sid = new SecurityIdentifier( primarySidClaim.Value );
            return ((NTAccount) sid.Translate( typeof( NTAccount ) )).Value;
        }
        else
        {
            throw new UnauthorizedAccessException(
                String.Format( "Found {0} PrimarySid claims.", primarySidsFound.Count ) );
        }
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
            throw new InvalidRequestException("The caller's principal is null.");
        }

        // Get User Name from SID and issue that as a Name Claim.
        string ntAccountValue = GetNTAccountName( principal );
        ntAccountValue = ntAccountValue.Substring( ntAccountValue.IndexOf( '\\' ) + 1 );
        outputIdentity.Claims.Add( new Claim( Microsoft.IdentityModel.Claims.ClaimTypes.Name, ntAccountValue ) );

        // Issue Custom Claims.
        ntAccountValue = ntAccountValue + "@contoso.com";
        outputIdentity.Claims.Add( new Claim( "http://WindowsIdentityFoundationSamples/myID", ntAccountValue, ClaimValueTypes.String ) );
        outputIdentity.Claims.Add( new Claim( "http://WindowsIdentityFoundationSamples/2008/05/AgeClaim", "25", ClaimValueTypes.Integer ) );

        return outputIdentity;
    }
}

