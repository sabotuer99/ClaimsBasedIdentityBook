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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;

    public abstract class OAuthClientBase : IOAuthClient
    {
        private readonly Uri authorizationServerUrl;

        public OAuthClientBase(Uri authorizationServerUrl, string scope)
        {
            if (authorizationServerUrl == null)
            {
                throw new ArgumentNullException("authorizationServerUrl");
            }

            this.authorizationServerUrl = authorizationServerUrl;

            if (string.IsNullOrEmpty(scope))
            {
                throw new ArgumentNullException("scope");
            }

            this.Scope = scope;
        }

        public abstract string Profile
        {
            get;
        }

        public string Scope
        {
            get;
            set;
        }

        public OAuthAccessTokenResponse RequestAccessToken()
        {
            this.Validate();

            NameValueCollection parameters = this.CreateRequestParameters();
            OAuthAccessTokenResponse response = this.RequestAccessTokenCore(parameters);

            return response;
        }

        protected virtual NameValueCollection CreateRequestParameters()
        {
            var parameters = new NameValueCollection();
            parameters.Add(OAuthConstants.Scope, this.Scope);
            parameters.Add(OAuthConstants.GrantType, this.Profile);

            this.FillRequestParameters(parameters);
            
            return parameters;
        }

        protected abstract void FillRequestParameters(NameValueCollection parameters);

        protected abstract void Validate();

        protected virtual OAuthAccessTokenResponse RequestAccessTokenCore(NameValueCollection parameters)
        {
            var body = CreateRequestBody(parameters);

            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = DoPost(this.authorizationServerUrl, body);
            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                {
                    httpResponse = (HttpWebResponse)wex.Response;
                }
            }

            var reader = new StreamReader(httpResponse.GetResponseStream());
            string data = reader.ReadToEnd();

            OAuthAccessTokenResponse accessTokenResponse;
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                var responseData = this.ParseOAuthResponse(data);
                accessTokenResponse = this.MapResult(responseData);
            }
            else
            {
                var responseData = this.ParseOAuthError(data);
                accessTokenResponse = this.MapError(responseData);
            }                       

            return accessTokenResponse;
        }

        protected virtual OAuthAccessTokenResponse MapResult(NameValueCollection responseData)
        {
            var accessTokenResponse = new OAuthAccessTokenResponse
            {
                AccessToken = responseData[OAuthConstants.AccessToken],
                RefreshToken = responseData[OAuthConstants.RefreshToken],
                TokenType = responseData[OAuthConstants.TokenType],
                Scope = responseData[OAuthConstants.Scope],
                ValidTo = this.GetDateTimeFromExpiresOn(Convert.ToUInt64(responseData[OAuthConstants.TokenExpiresIn]))
            };

            return accessTokenResponse;
        }

        protected virtual OAuthAccessTokenResponse MapError(NameValueCollection responseData)
        {
            var accessTokenResponse = new OAuthAccessTokenResponse
            {
                ErrorCode = responseData[OAuthConstants.Error],
                ErrorDescription = responseData[OAuthConstants.ErrorDescription]                
            };

            return accessTokenResponse;
        }

        protected virtual NameValueCollection ParseOAuthResponse(string response)
        {
            NameValueCollection tokens = new NameValueCollection();
            if (response.StartsWith("{"))
            {
                // Is json
                var json = new JavaScriptSerializer();
                var parsed = json.DeserializeObject(response) as Dictionary<string, object>;
                foreach (var item in parsed)
                {
                    tokens.Add(item.Key, item.Value.ToString());
                }
            }
            else
            {
                tokens = HttpUtility.ParseQueryString(response);
            }

            return tokens;
        }

        protected virtual NameValueCollection ParseOAuthError(string response)
        {
            OAuthError error;
            if (response.StartsWith("{"))
            {
                // Is json
                var json = new JavaScriptSerializer();
                error = json.Deserialize<OAuthError>(response);
            }
            else
            {
                error = new OAuthError
                {
                    Error = OAuthErrorCodes.InvalidRequest,
                    Error_Description = response
                };
            }

            var collection = new NameValueCollection();
            collection[OAuthConstants.Error] = error.Error;
            collection[OAuthConstants.ErrorDescription] = error.Error_Description;

            return collection;
        }

        private static string CreateRequestBody(NameValueCollection parameters)
        {
            string result = string.Empty;

            for (int i = 0; i < parameters.Count; i++)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "&";
                }

                var key = parameters.GetKey(i);
                var value = HttpUtility.UrlEncode(parameters[key]);
                result += key + "=" + value;
            }

            return result;
        }

        private static HttpWebResponse DoPost(Uri authorizationServerUrl, string body)
        {
            var request = HttpWebRequest.Create(authorizationServerUrl) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = body.Length;

            using (StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.ASCII))
            {
                requestWriter.Write(body);
            }

            var response = request.GetResponse() as HttpWebResponse;

            return response;
        }

        private DateTime GetDateTimeFromExpiresOn(ulong seconds)
        {
            var start = DateTime.Now;
            var expiresOn = start.AddSeconds(seconds);

            return expiresOn;
        }
    }
}
