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
using System.IO;
using System.ServiceModel;
using System.Threading;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    /// <summary>
    /// Summary description for CustomSecurityTokenService
    /// </summary>
    public class BookStoreSecurityTokenService : Microsoft.IdentityModel.SecurityTokenService.SecurityTokenService
    {
        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="config">The <see cref="SecurityTokenServiceConfiguration"/> object to be 
        /// passed to the base class.</param>
        public BookStoreSecurityTokenService( SecurityTokenServiceConfiguration config )
            : base( config )
        {
        }

        /// <summary>
        /// Override this method to provide scope specific encrypting credentials.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
        {
            ValidateAppliesTo( request.AppliesTo );
            Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri );
            scope.SigningCredentials = new X509SigningCredentials( X509Helper.GetX509Certificate2( BookStoreSTSServiceConfig.CertStoreName,
                                                                        BookStoreSTSServiceConfig.CertStoreLocation,
                                                                        BookStoreSTSServiceConfig.CertDistinguishedName ) );
            // Note: In this sample app only a single RP identity is shown, which is localhost, and the certificate of that RP is 
            // populated as EncryptingCredentials
            // If you have multiple RPs for the STS you would select the certificate that is specific to 
            // the RP that requests the token and then use that for EncryptingCredentials

            scope.EncryptingCredentials = new X509EncryptingCredentials( X509Helper.GetX509Certificate2( BookStoreSTSServiceConfig.CertStoreName,
                                                                        BookStoreSTSServiceConfig.CertStoreLocation,
                                                                        BookStoreSTSServiceConfig.TargetDistinguishedName ) );
            return scope;
        }

        /// <summary>
        /// Override this method to return a collection of output subjects to be included in the issued token.
        /// </summary>
        /// <param name="scope">The scope information about the Relying Party.</param>
        /// <param name="principal">The IClaimsPrincipal that represents the identity of the requestor.</param>
        /// <param name="request">The token request parameters that arrived in the call.</param>
        /// <returns>The claims collection that will be placed inside the issued token.</returns>        
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            string clientName = String.Empty;
            string purchaseLimitValue = String.Empty;

            // Need to iterate through claimsidentity collection and find the right
            // claimsidentity collection that is issued by HomeRealmSTS

            foreach ( ClaimsIdentity fedId in principal.Identities )
            {
                if ( IssuedByHomeRealmSTS( fedId ) )
                {
                    foreach ( Claim claim in fedId.Claims )
                    {
                        if ( claim.ClaimType == ClaimTypes.Name )
                        {
                            clientName = claim.Value;
                        }

                        if ( claim.ClaimType == ScenarioConstants.PurchaseLimitClaim )
                        {
                            purchaseLimitValue = claim.Value;
                        }
                    }
                }
            }

            if ( String.IsNullOrEmpty( clientName ) || String.IsNullOrEmpty( purchaseLimitValue ) )
            {
                throw ( new InvalidRequestException( "Required claim types were not submitted." ) );
            }

            List<Claim> claims = new List<Claim>();

            claims.Add( new Claim( ClaimTypes.Name, clientName ) );
            claims.Add( new Claim( ScenarioConstants.PurchaseLimitClaim, purchaseLimitValue ) );

            return new ClaimsIdentity( claims );
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

            if ( !appliesTo.Uri.Equals( new Uri( BookStoreSTSServiceConfig.ExpectedAppliesToURI ) ) )
            {
                throw new InvalidRequestException( String.Format( "The relying party address is not valid. Expected value is {0}, the actual value is {1}.", BookStoreSTSServiceConfig.ExpectedAppliesToURI, appliesTo.Uri.AbsoluteUri ) );
            }
        }


        #region Helper Methods

        /// <summary>
        /// Helper function to check if SAML Token was issued by HomeRealmSTS
        /// </summary>
        /// <returns>True on success. False on failure.</returns>
        private static bool IssuedByHomeRealmSTS( ClaimsIdentity claimsId )
        {
            // Extract the issuer ClaimSet
            string issuerClaimsId = claimsId.Claims[0].Issuer;

            // Extract the thumbprint for the HomeRealmSTS.com certificate
            string certSubjectName = X509Helper.GetX509Certificate2( BookStoreSTSServiceConfig.CertStoreName,
                                                                     BookStoreSTSServiceConfig.CertStoreLocation,
                                                                     BookStoreSTSServiceConfig.IssuerDistinguishedName ).SubjectName.Name;

            return String.Equals( issuerClaimsId, certSubjectName );
        }
        #endregion
    }
}
