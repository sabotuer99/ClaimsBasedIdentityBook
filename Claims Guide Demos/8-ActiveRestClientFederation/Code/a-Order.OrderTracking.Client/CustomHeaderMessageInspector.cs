//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.Client
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IdentityModel.Tokens;
    using System.Net;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;
    using System.ServiceModel.Security;
    using System.Text;
    using System.Web.Script.Serialization;
    using Microsoft.IdentityModel.Protocols.WSTrust;
    using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
    using Microsoft.IdentityModel.SecurityTokenService;

    public class CustomHeaderMessageInspector : IClientMessageInspector
    {
        private const string ServiceEndpointFormat = "https://{0}.accesscontrol.windows.net";
        private readonly string acsRelyingParty;
        private readonly ClientCredentials clientCredentials;
        private readonly string serviceEndpoint;
        private readonly string stsEndpoint;
        private NameValueCollection oauthToken;
        private DateTime oauthTokenValidUntil = DateTime.MinValue;
        private GenericXmlSecurityToken samlToken;

        public CustomHeaderMessageInspector(ClientCredentials clientCredentials, string acsNamespace, string acsRelyingParty, string stsEndpoint)
        {
            this.clientCredentials = clientCredentials;
            this.acsRelyingParty = acsRelyingParty;
            this.stsEndpoint = stsEndpoint;
            this.serviceEndpoint = string.Format(ServiceEndpointFormat, acsNamespace);
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Making sure we have a HttpRequestMessageProperty
            HttpRequestMessageProperty httpRequestMessageProperty;
            if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                httpRequestMessageProperty = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                if (httpRequestMessageProperty == null)
                {
                    httpRequestMessageProperty = new HttpRequestMessageProperty();
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty);
                }
            }
            else
            {
                httpRequestMessageProperty = new HttpRequestMessageProperty();
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty);
            }

            // Get SWT token from ACS if no token has been requested before or the saved one has expired
            if (oauthToken == null || oauthTokenValidUntil == DateTime.MinValue || DateTime.UtcNow > oauthTokenValidUntil)
            {
                // Get SAML token from Litware
                if (samlToken == null)
                {
                    samlToken = GetSamlToken(this.serviceEndpoint, this.stsEndpoint, this.clientCredentials) as GenericXmlSecurityToken;
                }

                int timesTriedCallingAcs;
                const int maxTimesToTryCallingAcs = 2;
                for (timesTriedCallingAcs = 1; timesTriedCallingAcs <= maxTimesToTryCallingAcs; timesTriedCallingAcs++)
                {
                    try
                    {
                        // Get OAuth from ACS authenticating with the SAML token from Litware
                        oauthToken = GetOAuthToken(samlToken.TokenXml.OuterXml, this.serviceEndpoint, this.acsRelyingParty);
                        break;
                    }
                    catch (WebException)
                    {
                        if (timesTriedCallingAcs == 1)
                        {
                            // Refresh the SAML token from Litware because it may be expired before calling ACS again
                            samlToken = GetSamlToken(this.serviceEndpoint, this.stsEndpoint, this.clientCredentials) as GenericXmlSecurityToken;
                        }
                    }
                }

                if (timesTriedCallingAcs > maxTimesToTryCallingAcs)
                {
                    throw new ApplicationException("Unable to get a token from ACS with the token issued by Litware. This may be caused by an incorrect date and time in this computer.");
                }

                var tokenExpiresIn = double.Parse(oauthToken["expires_in"]);
                oauthTokenValidUntil = DateTime.UtcNow + TimeSpan.FromSeconds(tokenExpiresIn);
            }

            // Add the token to the request's authorization header
            var oauthAuthorizationHeader = string.Format("Bearer {0}", oauthToken["access_token"]);
            httpRequestMessageProperty.Headers.Add(HttpRequestHeader.Authorization, oauthAuthorizationHeader);

            return null;
        }

        private static NameValueCollection GetOAuthToken(string xmlSamlToken, string serviceEndpoint, string acsRelyingParty)
        {
            var values = new NameValueCollection
                             {
                                 { "grant_type", "urn:oasis:names:tc:SAML:2.0:assertion" }, 
                                 { "assertion", xmlSamlToken },
                                 { "scope", acsRelyingParty }

                             };

            var client = new WebClient { BaseAddress = serviceEndpoint };
            byte[] acsTokenResponse = client.UploadValues("v2/OAuth2-13", "POST", values);
            string acsToken = Encoding.UTF8.GetString(acsTokenResponse);
            var tokens = new NameValueCollection();
            var json = new JavaScriptSerializer();
            var parsed = json.DeserializeObject(acsToken) as Dictionary<string, object>;
            foreach (var item in parsed)
            {
                tokens.Add(item.Key, item.Value.ToString());
            }

            return tokens;
        }

        private static SecurityToken GetSamlToken(string realm, string stsEndpoint, ClientCredentials clientCredentials)
        {
            using (var factory = new WSTrustChannelFactory(
                new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential), 
                new EndpointAddress(new Uri(stsEndpoint))))
            {
                factory.Credentials.UserName.UserName = clientCredentials.UserName.UserName;
                factory.Credentials.UserName.Password = clientCredentials.UserName.Password;
                factory.TrustVersion = TrustVersion.WSTrust13;

                WSTrustChannel channel = null;

                try
                {
                    var rst = new RequestSecurityToken
                                  {
                                      RequestType = WSTrust13Constants.RequestTypes.Issue, 
                                      AppliesTo = new EndpointAddress(realm), 
                                      KeyType = KeyTypes.Bearer, 
                                  };

                    channel = (WSTrustChannel)factory.CreateChannel();

                    RequestSecurityTokenResponse response;
                    var token = channel.Issue(rst, out response);

                    return token;
                }
                finally
                {
                    if (channel != null)
                    {
                        channel.Abort();
                    }

                    factory.Abort();
                }
            }
        }
    }
}