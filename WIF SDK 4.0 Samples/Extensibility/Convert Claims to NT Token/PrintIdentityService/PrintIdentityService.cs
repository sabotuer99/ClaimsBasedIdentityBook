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
using System.Threading;

using Microsoft.IdentityModel.Samples.WindowsTokenService.Common;

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.PrintIdentityService
{
    /// <summary>
    /// The print identity service implementation.
    /// </summary>
    class PrintIdentityService : IPrintIdentity
    {
        /// <summary>
        /// Prints the current identity's name.
        /// </summary>
        public void Print()
        {
            Console.WriteLine( "Hello " + Thread.CurrentPrincipal.Identity.Name + ".\n" );
        }
    }
}
