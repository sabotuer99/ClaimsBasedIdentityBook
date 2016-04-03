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
    using System.IO;
    using System.Web;
    using Microsoft.Samples.DPE.OAuth.AuthorizationServer;

    public abstract class OAuthMessageHandler
    {
        public OAuthMessageHandler()
        {
            this.ServiceConfiguration = new OAuthServiceConfiguration();
            if (!this.ServiceConfiguration.IsInitialized)
            {
                this.ServiceConfiguration.Initialize();
            }
        }

        protected OAuthServiceConfiguration ServiceConfiguration { get; set; }

        public virtual TokenRequestMessage ReadMessage(StreamReader reader)
        {
            NameValueCollection requestParameters;
            string requestString;
            requestString = reader.ReadToEnd();
            reader.Close();            
            requestParameters = HttpUtility.ParseQueryString(requestString);

            var message = new TokenRequestMessage();
            foreach (string key in requestParameters.AllKeys)
            {
                if (key == OAuthConstants.GrantType)
                {
                    message.Type = requestParameters[key];
                    requestParameters.Remove(key);
                }

                message.Parameters = requestParameters;
            }

            return message;
        }

        public virtual bool CanValidateMessage(TokenRequestMessage message)
        {
            return false;
        }

        public abstract NameValueCollection Validate(TokenRequestMessage message);

        protected virtual void EnsureClientExists(TokenRequestMessage message)
        {
            var clientId = message.Parameters[OAuthConstants.ClientId];
            if (!this.ClientStore.ClientExists(clientId))
            {
                throw new OAuthException(OAuthErrorCodes.InvalidClient, string.Format("The client_id '{0}' is not registered", clientId));
            }
        }
    }
}