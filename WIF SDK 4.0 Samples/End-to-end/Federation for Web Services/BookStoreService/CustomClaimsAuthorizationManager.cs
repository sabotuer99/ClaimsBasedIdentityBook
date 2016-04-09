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
using System.IO;
using System.ServiceModel;
using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    public class CustomClaimsAuthorizationManager : ClaimsAuthorizationManager
    {
        #region AccessCheck() override

        /// <summary>
        /// Implementation of the framework level access control mechanism via ClaimsAuthorizationManager
        /// </summary>
        /// <returns>True on success. False on failure.</returns>
        public override bool CheckAccess( AuthorizationContext context )
        {
            if ( context == null || context.Action == null || context.Action.Count != 1 )
            {
                return false;
            }

            if ( context.Action[0].ClaimType != ClaimTypes.Name )
            {
                return false;
            }

            string action = context.Action[0].Value;

            // BrowseBooks is always authorized, so return true
            if ( action == BookStoreServiceConfig.BrowseBooksAction )
            {
                return true;
            }

            // If the requested operation is NOT BuyBook, return false (Access Denied)
            // The only operation we support apart from BrowseBooks is BuyBook
            if ( action != BookStoreServiceConfig.BuyBookAction )
            {
                return false;
            }

            //
            // CheckAccess is called prior to impersonation in WCF, so we need to pull
            // the ClaimsPrincipal from the AuthorizationContext.Principal.
            //
            IClaimsPrincipal claimsPrincipal = context.Principal;

            // If there is no ClaimIdentityCollection in the ClaimsPrincipal, return false.
            // The issued token used to authenticate should contain claims 
            if ( claimsPrincipal.Identities == null || claimsPrincipal.Identities.Count <= 0 )
            {
                return false;
            }

            // Need to iterate through claimsidentity collection and find the right
            // claimsidentity collection that is issued by BookStoreSTS

            foreach ( ClaimsIdentity claimsId in claimsPrincipal.Identities )
            {
                // Were the identity NOT issued by the BookStoreSTS then return false (Access Denied)
                // The BookStoreService only accepts requests where the client authenticated using a token
                // issued by the BookStoreSTS.
                if ( !IssuedByBookStoreSTS( claimsId ) )
                {
                    return false;
                }
            }

            // All identities were issued by BookStoreSTS.
            return true;
        }

        #endregion

        /// <summary>
        /// Helper function to check if claims were issued by BookStoreSTS
        /// </summary>
        /// <returns>True on success. False on failure.</returns>
        private static bool IssuedByBookStoreSTS( ClaimsIdentity claimsId )
        {
            // Extract the issuer
            string issuerClaimsId = claimsId.Claims[0].Issuer;

            // Extract the Subject for the BookStoreSTS.com certificate
            string certThumbprint = X509Helper.GetX509Certificate2( BookStoreServiceConfig.CertStoreName,
                                                                    BookStoreServiceConfig.CertStoreLocation,
                                                                    BookStoreServiceConfig.IssuerCertDistinguishedName ).SubjectName.Name;

            return String.Equals( issuerClaimsId, certThumbprint );
        }
    }
}
