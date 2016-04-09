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
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;

namespace Client
{
    class Client
    {
        static void Main( string[] args )
        {
            Service1Client client = new Service1Client();
            client.ClientCredentials.ServiceCertificate.SetDefaultCertificate( "CN=localhost", StoreLocation.LocalMachine, StoreName.My );

            try
            {
                Console.WriteLine( client.Echo( "Hello" ) );
                client.Close();
            }
            catch ( CommunicationException e )
            {
                Console.WriteLine( e.Message );
                Console.WriteLine( e.StackTrace );
                Exception ex = e.InnerException;
                while ( ex != null )
                {
                    Console.WriteLine( "===========================" );
                    Console.WriteLine( ex.Message );
                    Console.WriteLine( ex.StackTrace );
                    ex = ex.InnerException;
                }
                client.Abort();
            }
            catch ( TimeoutException )
            {
                Console.WriteLine( "Timed out..." );
                client.Abort();
            }
            catch ( Exception e )
            {
                Console.WriteLine( "An unexpected exception occured." );
                Console.WriteLine( e.StackTrace );
                client.Abort();
            }
        }
    }
}
