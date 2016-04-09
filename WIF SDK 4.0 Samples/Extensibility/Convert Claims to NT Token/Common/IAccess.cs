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

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.Common
{
    /// <summary>
    /// The IAccess contract definition.
    /// </summary>
    [ServiceContract]
    public interface IAccess
    {
        [OperationContract]
        void Access( string address );
    }
}
