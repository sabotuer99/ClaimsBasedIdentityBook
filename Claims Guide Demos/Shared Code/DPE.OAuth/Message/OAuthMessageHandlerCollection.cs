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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IO;

    public class OAuthMessageHandlerCollection : Collection<OAuthMessageHandler>
    {
        private Dictionary<string, OAuthMessageHandler> listByType = new Dictionary<string, OAuthMessageHandler>();

        public OAuthMessageHandlerCollection(IEnumerable<OAuthMessageHandler> handlers)
        {
            foreach (OAuthMessageHandler handler in handlers)
            {
                Add(handler);
            }
        }

        public IEnumerable<string> RequestTypeIdentifiers
        {
            get
            {
                return this.listByType.Keys;
            }
        }

        public static OAuthMessageHandlerCollection CreateDefaultSecurityTokenHandlerCollection()
        {
            // TODO add other message Handlers as they are implemented
            OAuthMessageHandlerCollection collection = 
                new OAuthMessageHandlerCollection(
                    new OAuthMessageHandler[] 
                    {
                        new AssertionMessageHandler(),
                        new AuthorizationCodeMessageHandler(),
                        new ClientCredentialsMessageHandler()
                    });

            return collection;
        }        

        public TokenRequestMessage ReadMessage(StreamReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            foreach (OAuthMessageHandler handler in this)
            {
                return handler.ReadMessage(reader);
            }

            return null;
        }        

        public NameValueCollection Validate(TokenRequestMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException("message");
            }

            foreach (OAuthMessageHandler handler in this)
            {
                if (handler.CanValidateMessage(message))
                {
                    return handler.Validate(message);
                }
            }
            
            return null;
        }
    }
}