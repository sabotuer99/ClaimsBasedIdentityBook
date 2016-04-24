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
    using System.Collections.Specialized;

    public class ClientCredentialsProfileOAuthClient : OAuthClientBase
    {
        private Uri authorizationServerUrl;
        private ClientCredentials credentials;

        public ClientCredentialsProfileOAuthClient(Uri authorizationServerUrl, string scope)
            : base(authorizationServerUrl, scope)
        {
            this.authorizationServerUrl = authorizationServerUrl;
            this.credentials = new ClientCredentials();
        }

        public override string Profile
        {
            get { return OAuthProfiles.ClientCredentialsProfile; }
        }

        public ClientCredentials Credentials
        {
            get
            {
                return this.credentials;
            }
        }

        protected override void Validate()
        {
            if (string.IsNullOrEmpty(this.Credentials.UserName))
            {
                throw new ArgumentNullException("Username cannot be null");
            }

            if (string.IsNullOrEmpty(this.Credentials.Password))
            {
                throw new ArgumentNullException("Password cannot be null");
            }
        }

        protected override void FillRequestParameters(NameValueCollection parameters)
        {
            parameters.Add(OAuthConstants.ClientId, this.Credentials.UserName);
            parameters.Add(OAuthConstants.ClientSecret, this.Credentials.Password);
        }
    }
}