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

    public class AssertionProfileOAuthClient : OAuthClientBase
    {
        private AssertionClientCredentials credentials;

        public AssertionProfileOAuthClient(Uri authorizationServerUrl, string scope)
            : base(authorizationServerUrl, scope)
        {
            this.credentials = new AssertionClientCredentials();
        }

        public override string Profile
        {
            get 
            {
                return OAuthProfiles.AssertionProfile;
            }
        }

        public AssertionClientCredentials Credentials
        {
            get
            {
                return this.credentials;
            }
        }
        
        protected override void Validate()
        {
            if (string.IsNullOrEmpty(this.Credentials.Assertion))
            {
                throw new ArgumentNullException("Assertion cannot be null. ClientCredentials must be set.");
            }
        }

        protected override void FillRequestParameters(NameValueCollection parameters)
        {
            parameters.Add(OAuthConstants.Assertion, this.Credentials.Assertion);
        }
    }
}