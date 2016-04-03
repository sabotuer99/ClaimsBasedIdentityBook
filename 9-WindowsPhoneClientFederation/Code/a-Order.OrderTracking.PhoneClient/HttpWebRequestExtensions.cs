//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.PhoneClient
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Json;
    using System.Text;
    using AOrder.OrderTracking.Contracts.Data;
    using Microsoft.Phone.Reactive;

    public static class HttpWebRequestExtensions
    {
        /// Format:
        /// {0}: Message Id - Guid
        /// {1}: To - https://localhost/Litware.SimulatedIssuer.9/Issuer.svc
        /// {2}: Created - 2011-03-11T01:49:29.395Z
        /// {3}: Expires - 2011-03-11T01:54:29.395Z
        /// {4}: Username - LITWARE\rick
        /// {5}: Password - password
        /// {6}: Applies To - https://{project}.accesscontrol.windows.net/
        private const string samlSignInRequestFormat =
            @"<s:Envelope xmlns:s=""http://www.w3.org/2003/05/soap-envelope"" xmlns:a=""http://www.w3.org/2005/08/addressing"" xmlns:u=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd""><s:Header><a:Action s:mustUnderstand=""1"">http://docs.oasis-open.org/ws-sx/ws-trust/200512/RST/Issue</a:Action><a:MessageID>urn:uuid:{0}</a:MessageID><a:ReplyTo><a:Address>http://www.w3.org/2005/08/addressing/anonymous</a:Address></a:ReplyTo><a:To s:mustUnderstand=""1"">{1}</a:To><o:Security s:mustUnderstand=""1"" xmlns:o=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""><u:Timestamp u:Id=""_0""><u:Created>{2}</u:Created><u:Expires>{3}</u:Expires></u:Timestamp><o:UsernameToken u:Id=""uuid-2c0bf680-4e5e-495e-8082-0166c768e94d-1""><o:Username>{4}</o:Username><o:Password Type=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText"">{5}</o:Password></o:UsernameToken></o:Security></s:Header><s:Body><trust:RequestSecurityToken xmlns:trust=""http://docs.oasis-open.org/ws-sx/ws-trust/200512""><wsp:AppliesTo xmlns:wsp=""http://schemas.xmlsoap.org/ws/2004/09/policy""><a:EndpointReference><a:Address>{6}</a:Address></a:EndpointReference></wsp:AppliesTo><trust:KeyType>http://docs.oasis-open.org/ws-sx/ws-trust/200512/Bearer</trust:KeyType><trust:RequestType>http://docs.oasis-open.org/ws-sx/ws-trust/200512/Issue</trust:RequestType></trust:RequestSecurityToken></s:Body></s:Envelope>";

        /// Format:
        /// {0}: The saml token url encoded
        private const string swtSignInRequestFormat =
            @"grant_type=urn%3aoasis%3anames%3atc%3aSAML%3a2.0%3aassertion&assertion={0}&scope=https%3a%2f%2flocalhost%2fa-Order.OrderTracking.Services.9";

        public static IObservable<HttpWebRequest> AddAuthorizationHeader(this HttpWebRequest request, string swtToken)
        {
            var authHeader = string.Format(CultureInfo.InvariantCulture, "Bearer {0}", swtToken);
            request.Headers[HttpRequestHeader.Authorization] = authHeader;

            return Observable.Return(request);
        }

        public static IObservable<HttpWebRequest> AddAuthorizationHeader(this HttpWebRequest request, string samlEndpoint, string acsEndpoint, string serviceEnpoint)
        {
            var samlTokenRequest = GetSamlTokenRequest(samlEndpoint, acsEndpoint);

            return
                HttpClient.RequestTo(new Uri(samlEndpoint))
                    .PostSamlTokenRequest(samlTokenRequest)
                    .SelectMany(xmlToken =>
                                    {
                                        var swtTokenRequest = GetSwtTokenRequest(xmlToken);
                                        return
                                            HttpClient.RequestTo(new Uri(acsEndpoint))
                                                .PostSwtTokenRequest(swtTokenRequest);
                                    },
                                (xmlToken, swtToken) =>
                                    {
                                        var authHeader = string.Format(CultureInfo.InvariantCulture, "Bearer {0}", swtToken);
                                        request.Headers[HttpRequestHeader.Authorization] = authHeader;

                                        return request;
                                    });
        }

        public static IObservable<T> Get<T>(this HttpWebRequest request)
        {
            request.Method = "GET";

            return
                Observable
                    .FromAsyncPattern<WebResponse>(request.BeginGetResponse, request.EndGetResponse)()
                    .Select(
                        response =>
                            {
                                using (var responseStream = response.GetResponseStream())
                                {
                                    var serializer = new DataContractSerializer(typeof(Order[]));
                                    return (T)serializer.ReadObject(responseStream);
                                }
                            });
        }

        public static IObservable<string> PostSamlTokenRequest(this HttpWebRequest request, string tokenRequest)
        {
            request.Method = "POST";
            request.ContentType = "application/soap+xml; charset=utf-8";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                            {
                                using (requestStream)
                                {
                                    var buffer = Encoding.UTF8.GetBytes(tokenRequest);
                                    requestStream.Write(buffer, 0, buffer.Length);
                                    requestStream.Close();
                                }

                                return
                                    Observable.FromAsyncPattern<WebResponse>(
                                        request.BeginGetResponse,
                                        request.EndGetResponse)();
                            },
                        (requestStream, webResponse) =>
                            {
                                string res = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8).ReadToEnd();
                                var startIndex = res.IndexOf("<Assertion ");
                                var endIndex = res.IndexOf("</Assertion>");
                                var token = res.Substring(startIndex, endIndex + "</Assertion>".Length - startIndex);
                                return token;
                            });
        }

        public static IObservable<string> PostSwtTokenRequest(this HttpWebRequest request, string tokenRequest)
        {
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            return
                Observable
                    .FromAsyncPattern<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream)()
                    .SelectMany(
                        requestStream =>
                            {
                                using (requestStream)
                                {
                                    var buffer = Encoding.UTF8.GetBytes(tokenRequest);
                                    requestStream.Write(buffer, 0, buffer.Length);
                                    requestStream.Close();
                                }

                                return
                                    Observable.FromAsyncPattern<WebResponse>(
                                        request.BeginGetResponse,
                                        request.EndGetResponse)();
                            },
                        (requestStream, webResponse) =>
                            {
                                using (var responseStream = webResponse.GetResponseStream())
                                {
                                    var serializer = new DataContractJsonSerializer(typeof(AcsToken));
                                    var tokens = (AcsToken)serializer.ReadObject(responseStream);
                                    return tokens.access_token;
                                }
                            });
        }

        private static string GetSamlTokenRequest(string samlEndpoint, string realm)
        {
            var tokenRequest =
                string.Format(
                    CultureInfo.InvariantCulture,
                    samlSignInRequestFormat,
                    Guid.NewGuid(),
                    samlEndpoint,
                    DateTime.UtcNow.ToString("yyyy'-'MM'-'ddTHH':'mm':'ss'.'fff'Z'"),
                    DateTime.UtcNow.AddMinutes(15).ToString("yyyy'-'MM'-'ddTHH':'mm':'ss'.'fff'Z'"),
                    "LITWARE\\rick",
                    "PasswordIsNotChecked",
                    "https://aorderphone-dev.accesscontrol.windows.net/");

            return tokenRequest;
        }

        private static string GetSwtTokenRequest(string xmlSamlToken)
        {
            var urlEncodedXmlSamlToken = HttpUtility.UrlEncode(xmlSamlToken);

            var tokenRequest =
                string.Format(
                    CultureInfo.InvariantCulture,
                    swtSignInRequestFormat,
                    urlEncodedXmlSamlToken);

            return tokenRequest;
        }

        [DataContract]
        public class AcsToken
        {
            [DataMember] public string access_token;

            [DataMember] public string expires_in;
        }
    }
}