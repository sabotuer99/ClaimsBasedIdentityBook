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
using System;

using Microsoft.IdentityModel.Protocols.WSTrust.Bindings;
using Microsoft.IdentityModel.Samples.WindowsTokenService.Common;

namespace Microsoft.IdentityModel.Samples.WindowsTokenService.PrintIdentityService
{
    /// <summary>
    /// Hosts the back end service implementation.
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            ServiceHost printService = null;
            try
            {
                printService = new ServiceHost( typeof( PrintIdentityService ) );
                printService.AddServiceEndpoint( typeof( IPrintIdentity ), new WindowsWSTrustBinding(), Address.PrintServiceAddress );
                printService.Open();
               
                Console.WriteLine( "The print identity service has started at {0}.\n", Address.PrintServiceAddress );
                Console.WriteLine( "Press [Enter] to stop.\n" );
                Console.ReadLine();
            }
            finally
            {
                try
                {
                    if ( printService != null )
                    {
                        printService.Close();
                    }
                }
                catch ( CommunicationException )
                {
                }
                catch ( TimeoutException )
                {
                }
            }
        }
    }
}
