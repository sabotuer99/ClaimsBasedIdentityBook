//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Microsoft.Samples.DPE.OAuth.Message
{
    using System;

    public class TokenResponseMessage
    {
        private string accessToken;
        private string refreshToken;
        private TimeSpan accessTokenExpiresIn;

        public string AccessToken
        {
            get { return this.accessToken; }
            set { this.accessToken = value; }
        }
      
        public string RefreshToken
        {
            get { return this.refreshToken; }
            set { this.refreshToken = value; }
        }

        public TimeSpan AccessTokenExpiresIn
        {
            get { return this.accessTokenExpiresIn; }
            set { this.accessTokenExpiresIn = value; }
        }
    }
}