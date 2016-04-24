//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Microsoft.Samples.DPE.OAuth
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;

    public static class OAuthHelper
    {
        public static string CreateQueryString(NameValueCollection parameters)
        {
            string result = String.Empty;
            string key, value;

            for (int i = 0; i < parameters.Count; i++)
            {
                if (!string.IsNullOrEmpty(result))
                {
                    result += "&";
                }

                key = parameters.GetKey(i);
                value = HttpUtility.UrlEncode(parameters[key]);
                
                result = result + key + "=" + value;
            }

            return result;
        }

        public static void SendErrorResponse(OAuthException ex, HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var error = new OAuthError
            {
                Error = ex.ErrorCode,
                Error_Description = ex.Message
            };

            var serializer = new JavaScriptSerializer();
            var json = serializer.Serialize(error);

            context.Response.Write(json);
            context.Response.Flush();
            context.ApplicationInstance.CompleteRequest();
        }

        public static void SendUnauthorizedResponse(OAuthError error, HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            string errorMessage = string.Format("OAuth error='{0}', error_description='{1}'", error.Error, error.Error_Description);
            
            context.Response.AddHeader("WWW-Authenticate", errorMessage);
            context.Response.Flush();
            context.Response.End();            
        }

        public static string CreateAuthorizationHeader(string accessToken)
        {
            string authHeader = "Bearer " + HttpUtility.UrlEncode(accessToken);
            return authHeader;
        }

        public static NameValueCollection ExtractOAuthParametersFromBody(HttpRequest request)
        {
            StreamReader requestReader;
            int length;
            string body;
            NameValueCollection parameters = new NameValueCollection();
            
            length = request.ContentLength;
            requestReader = new StreamReader(request.InputStream);
            body = requestReader.ReadToEnd();
            requestReader.Close();

            parameters = HttpUtility.ParseQueryString(body);

            return parameters;
        }

        public static NameValueCollection ExtractOAuthParametersFromHeader(HttpRequest request)
        {
            NameValueCollection parameters = new NameValueCollection();
           
            // Change this to only look for oauth headers
            parameters = request.Headers;

            return parameters;
        }

        public static NameValueCollection ExtractOAuthParametersFromQS(HttpRequest request)
        {
            NameValueCollection qstring = new NameValueCollection(request.QueryString);
            
            foreach (string key in qstring.AllKeys)
            {
                qstring[key] = HttpUtility.UrlDecode(qstring[key]);
            }

            return qstring;
        }

        public static string ExtractAcessTokenFromAuthenticateHeader(HttpRequest request)
        {
            NameValueCollection headers = new NameValueCollection();
            string authHeader;
            string token;

            authHeader = request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authHeader))
            {
                token = null;
            }
            else
            {
                string header = "Bearer ";
                if (string.CompareOrdinal(authHeader, 0, header, 0, header.Length) == 0)
                {
                    token = authHeader.Remove(0, header.Length);
                }
                else
                {
                    throw new Exception("the authorization header was invalid");
                }
            }
            
            return token;
        }

        public static bool IsOAuthAuthorization(HttpRequest request)
        {
            if (!string.IsNullOrEmpty(request.Headers["Authorization"]))
            {
                if (request.Headers["Authorization"].StartsWith("Bearer "))
                {
                    return true;
                }
            }

            return false;
        }

        public static byte[] StrToByteArray(string str)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            return encoding.GetBytes(str);
        }
    }
}