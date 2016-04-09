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

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    /// <summary>
    /// Custom Exception thrown by X509Helper when Certificate lookup fails.
    /// </summary>
    [Serializable]
    public class X509HelperException : Exception
    {
        public X509HelperException( string message )
            : base( message )
        {
        }

        protected X509HelperException( SerializationInfo info, StreamingContext context )
            : base( info, context )
        {
        }

        public X509HelperException()
            : base()
        {
        }

        public X509HelperException( string message, Exception inner )
            : base( message, inner )
        {
        }
    }
}
