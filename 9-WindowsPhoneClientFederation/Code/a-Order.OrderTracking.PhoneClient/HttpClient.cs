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
    using System.Net;

    public static class HttpClient
    {
        public static HttpWebRequest RequestTo(Uri uri)
        {
            var request = (HttpWebRequest)WebRequest.Create(uri);
            return request;
        }
    }
}