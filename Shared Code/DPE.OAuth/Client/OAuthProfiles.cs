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

    public static class OAuthProfiles
    {
        public const string WebProfile = "web_server";
        public const string ClientCredentialsProfile = "client_credentials";
        public const string AssertionProfile = "urn:oasis:names:tc:SAML:2.0:assertion";
        ////public const string AssertionProfile = "http://oauth.net/grant_type/assertion/saml/2.0/bearer";
    }
}