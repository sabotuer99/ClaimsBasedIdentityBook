//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Microsoft.Samples.DPE.OAuth.Client
{
    using System;

    public static class OAuthClient
    {
        public static AssertionProfileOAuthClient CreateAssertionProfile(Uri authorizationServerUrl, string scope)
        {
            return new AssertionProfileOAuthClient(authorizationServerUrl, scope);
        }

        public static ClientCredentialsProfileOAuthClient CreateClientCredentialsProfile(Uri authorizationServerUrl, string scope)
        {
            return new ClientCredentialsProfileOAuthClient(authorizationServerUrl, scope);
        }
    }
}