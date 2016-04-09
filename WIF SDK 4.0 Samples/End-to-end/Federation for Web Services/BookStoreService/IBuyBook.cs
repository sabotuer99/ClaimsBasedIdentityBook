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

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    [ServiceContract]
    public interface IBuyBook
    {
        /// <summary>
        /// The book ID is not part of the OperationContract because it is
        /// included as an EndpointAddress header. This is in order to do resource based
        /// authorization at the service's SecurityTokenService
        /// </summary>
        /// <param name="emailAddress">The e-mail address any shipping information should be sent to</param>
        /// <param name="shipAddress">The shipping address the book being purchased should be sent to</param>
        /// <returns></returns>
        [OperationContract]
        string BuyBook( string bookName, string emailAddress, string shipAddress );
    }
}
