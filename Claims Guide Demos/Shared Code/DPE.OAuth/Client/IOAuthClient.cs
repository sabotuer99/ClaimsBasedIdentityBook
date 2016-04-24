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

    public interface IOAuthClient
    {
        string Profile { get; }

        string Scope { get; set; }

        OAuthAccessTokenResponse RequestAccessToken();
    }
}