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


namespace Microsoft.IdentityModel.Samples.CustomRequestSecurityToken
{
    /// <summary>
    /// The EchoService implementation
    /// </summary>
    public class EchoService : IEcho
    {
        /// <summary>
        /// This method simply echo back the request string.
        /// </summary>
        /// <param name="request">The request string</param>
        /// <returns>The response string</returns>
        /// <exception cref="EchoException">When the request is null or empty string</exception>
        public string Echo( string request )
        {
            if ( string.IsNullOrEmpty( request ) )
            {
                throw new EchoException( "Empty String!" );
            }

            return request;
        }
    }
}

