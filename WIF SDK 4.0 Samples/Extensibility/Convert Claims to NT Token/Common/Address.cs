//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.Common
{
    /// <summary>
    /// Hosting addresses of different services.
    /// </summary>
    public static class Address
    {
        public const string ServiceAddress1         = "http://localhost:8080/AccessService1";
        public const string ServiceAddress2         = "http://localhost:8084/AccessService2";
        public const string PrintServiceAddress     = "http://localhost:8083/PrintIdentityService";
        public const string StsAddress              = "http://localhost:8081/STS";
    }
}
