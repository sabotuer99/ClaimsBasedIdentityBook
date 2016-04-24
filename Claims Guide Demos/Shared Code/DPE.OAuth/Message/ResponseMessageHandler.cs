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
    using System.IO;
    using System.Web;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Samples.DPE.OAuth.Tokens;

    public class ResponseMessageHandler
    {
        private OAuthServiceConfiguration serviceConfig;

        public ResponseMessageHandler(OAuthServiceConfiguration serviceConfig)
        {
            this.serviceConfig = serviceConfig;
        }

        public TokenResponseMessage CreateResponse(TokenRequestMessage message, NameValueCollection additionalInfo)
        {
            TokenResponseMessage response = new TokenResponseMessage();
            response.AccessToken = this.CreateAccessToken(message, additionalInfo);
            response.RefreshToken = this.CreateRefreshToken();
            response.AccessTokenExpiresIn = TimeSpan.FromSeconds(this.serviceConfig.SimpleWebTokenHandlerConfiguration.Issuer.TokenExpirationInSeconds);
            
            return response;
        }

        public void SendResponse(TokenResponseMessage message)
        {
            HttpResponse response = HttpContext.Current.Response;
            string body;
            body = OAuthConstants.AccessToken + '=' + HttpUtility.UrlEncode(message.AccessToken);
            body += '&' + OAuthConstants.TokenExpiresIn + '=' + ((int)message.AccessTokenExpiresIn.TotalSeconds).ToString();
            body += '&' + OAuthConstants.RefreshToken + '=' + message.RefreshToken;
            response.Write(body);
            response.End();
        }

        private static SimpleWebToken CreateSimpleWebToken(string issuer, string scope, TimeSpan validity, NameValueCollection additionalInfo)
        {
            var swt = new SimpleWebToken(issuer) { Audience = scope, TokenValidity = validity };

            if (additionalInfo != null)
            {
                foreach (string key in additionalInfo.AllKeys)
                {
                    swt.Parameters.Add(key, additionalInfo[key]);
                }
            }

            return swt;
        }

        private static string SerializeToken(SimpleWebToken accessToken, SecurityTokenHandlerCollection handlers)
        {
            if (handlers.CanWriteToken(accessToken))
            {
                string token = String.Empty;
                using (var sw = new StringWriter())
                {
                    var writer = new XmlTextWriter(sw);
                    handlers.WriteToken(writer, accessToken);

                    // remove the envelope <stringToken>
                    var envelope = sw.ToString();
                    token = XElement.Parse(envelope).Value;
                }

                return token;
            }

            return null;
        }

        private string CreateRefreshToken()
        {
            // TODO: implement refresh token
            return "PlaceHolderRefreshToken";
        }

        private string CreateAccessToken(TokenRequestMessage message, NameValueCollection additionalInfo)
        {
            var scope = message.Parameters["scope"];
            var validity = TimeSpan.FromSeconds(this.serviceConfig.SimpleWebTokenHandlerConfiguration.Issuer.TokenExpirationInSeconds);
            var swt = CreateSimpleWebToken(this.serviceConfig.SimpleWebTokenHandlerConfiguration.Issuer.IssuerIdentifier, scope, validity, additionalInfo);
            var accessToken = SerializeToken(swt, this.serviceConfig.SecurityTokenHandlers);

            return accessToken;
        }
    }
}