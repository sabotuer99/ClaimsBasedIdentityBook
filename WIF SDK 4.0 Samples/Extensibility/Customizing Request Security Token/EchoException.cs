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


using System;
using System.Runtime.Serialization;

namespace Microsoft.IdentityModel.Samples.CustomRequestSecurityToken
{
    /// <summary>
    /// The Exception thrown in the EchoService.
    /// </summary>
    [Serializable]
    public class EchoException : Exception
    {
        public EchoException( string message )
            : base( message )
        {
        }

        protected EchoException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
        }

        public EchoException()
            : base()
        {
        }

        public EchoException( string message, Exception inner )
            : base( message, inner )
        {
        }
    }
}
