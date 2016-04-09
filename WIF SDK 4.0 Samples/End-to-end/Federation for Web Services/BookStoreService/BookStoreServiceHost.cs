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
using System.ServiceModel;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    /// <summary>
    /// Custom ServiceHost that sets up the relevant Application Settings.
    /// </summary>
	class BookStoreServiceHost : ServiceHost
	{
		#region BookStoreServiceHost Constructor
        /// <summary>
        /// Sets up the BookStoreService by loading relevant Application Settings
        /// </summary>
		public BookStoreServiceHost(params Uri[] addresses) : base(typeof(BookStoreService), addresses)
		{
            BookStoreServiceConfig.LoadAppSettings();
		}
		#endregion
	}
}
