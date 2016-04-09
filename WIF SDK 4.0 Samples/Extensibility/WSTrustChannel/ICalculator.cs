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

namespace Microsoft.IdentityModel.Samples.TrustChannel
{
    /// <summary>
    /// The ICalculator contract definition
    /// </summary>
    [ServiceContract( Namespace = "http://Microsoft.ServiceModel.Samples" )]
    public interface ICalculator
    {
        [OperationContract]
        double Add( double n1, double n2 );
        [OperationContract]
        double Subtract( double n1, double n2 );
        [OperationContract]
        double Multiply( double n1, double n2 );
        [OperationContract]
        double Divide( double n1, double n2 );
    }
}
