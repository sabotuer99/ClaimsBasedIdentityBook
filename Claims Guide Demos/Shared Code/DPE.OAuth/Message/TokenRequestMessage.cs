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
    using System.Collections.Specialized;

    public class TokenRequestMessage
    {
        private string grantType;
        private NameValueCollection parameters;

        public TokenRequestMessage()
        {
            this.parameters = new NameValueCollection();
        }

        public string Type
        {
            get { return this.grantType; }
            set { this.grantType = value; }
        }

        public NameValueCollection Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }
    }
}