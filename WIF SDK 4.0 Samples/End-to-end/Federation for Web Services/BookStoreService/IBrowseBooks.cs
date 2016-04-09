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


using System.Collections.Generic;
using System.ServiceModel;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
	[ServiceContract]
	public interface IBrowseBooks
	{
        /// <summary>
        /// Allows callers to get a list of books the BookStore service sells
        /// </summary>
        /// <returns>A list of book titles</returns>
        [OperationContract]
        List<string> BrowseBooks();
	}
}
