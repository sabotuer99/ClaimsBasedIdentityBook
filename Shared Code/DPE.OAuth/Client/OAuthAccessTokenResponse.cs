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

    public class OAuthAccessTokenResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime ValidTo { get; set; }

        public string TokenType { get; set; }

        public string Scope { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDescription { get; set; }
    }
}