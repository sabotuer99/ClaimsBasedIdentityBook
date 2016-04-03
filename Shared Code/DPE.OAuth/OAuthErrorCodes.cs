//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Microsoft.Samples.DPE.OAuth
{
    using System;

    public static class OAuthErrorCodes
    {
        /// <summary>
        /// The request is missing a required parameter, includes an
        /// unsupported parameter or parameter value, repeats a parameter,
        /// includes multiple credentials, utilizes more than one mechanism
        /// for authenticating the client, or is otherwise malformed.
        /// </summary>
        public static readonly string InvalidRequest = "invalid_request";

        /// <summary>
        /// The client identifier provided is invalid, the client failed to
        /// authenticate, the client did not include its credentials,
        /// provided multiple client credentials, or used unsupported
        /// credentials type.
        /// </summary>
        public static readonly string InvalidClient = "invalid_client";
        
        /// <summary>
        /// The authenticated client is not authorized to use the access
        /// grant type provided.
        /// </summary>
        public static readonly string UnauthorizedClient = "unauthorized_client";
        
        /// <summary>
        /// The provided access grant is invalid, expired, or revoked (e.g. 
        /// invalid assertion, expired authorization token, bad end-user
        /// password credentials, or mismatching authorization code and
        /// redirection URI).
        /// </summary>
        public static readonly string InvalidGrant = "invalid_grant";
        
        /// <summary>
        /// The access grant included - its type or another attribute - is not supported by the authorization server.
        /// </summary>
        public static readonly string UnsupportedGrantType = "unsupported_grant_type";
        
        /// <summary>
        /// The requested scope is invalid, unknown, malformed, or exceeds
        /// the previously granted scope.
        /// </summary>
        public static readonly string InvalidScope = "invalid_scope";
    }
}
