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
    using System.Collections.Specialized;

    public class ClientCredentialsMessageHandler : OAuthMessageHandler
    {                
        public override bool CanValidateMessage(TokenRequestMessage message)
        {
            if (message.Type == RequestGrantType.None)
            {
                return true;
            }

            return false;
        }

        public override NameValueCollection Validate(TokenRequestMessage message)
        {
            string clientId = message.Parameters[OAuthConstants.ClientId];
            string clientSecret = message.Parameters[OAuthConstants.ClientSecret];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new InvalidOperationException("client_id and client_secret must be present for this profile");
            }

            bool valid = this.ClientStore.ValidateClient(clientId, clientSecret);
            if (!valid)
            {
                throw new InvalidOperationException("client_id is not registered or client_secret is invalid");
            }

            message.Parameters.Remove(OAuthConstants.ClientSecret);
            
            return message.Parameters;
        }
    }
}