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


using System.ServiceModel;

namespace Microsoft.IdentityModel.Samples.CustomToken
{
    /// <summary>
    /// The IEcho contract definition
    /// </summary>
    [ServiceContract]
    public interface IEcho
    {
        [OperationContract]
        string Echo( string request );
    }
}
