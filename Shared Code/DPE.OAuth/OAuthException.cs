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
    using System.Runtime.Serialization;

    [Serializable]
    public class OAuthException : Exception
    {
        public OAuthException(string errorCode, string message)
            : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public OAuthException(string errorCode, string message, Exception inner)
            : base(message, inner)
        {
            this.ErrorCode = errorCode;                 
        }

        public string ErrorCode { get; set; }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}