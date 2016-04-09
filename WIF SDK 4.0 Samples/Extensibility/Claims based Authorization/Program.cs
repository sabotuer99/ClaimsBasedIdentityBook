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
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using Microsoft.IdentityModel.Claims;

namespace ClaimsBasedAuthorization
{
    /// <summary>
    /// Program illustrates using Claims-based authorization
    /// </summary>
    class Program
    {        
        static void Main( string[] args )
        {

            //
            // Configure .NET Framework to use Windows Claims Principals
            // Emulates the authentication phase supported by the Windows Identity Foundation.
            //
            AppDomain.CurrentDomain.SetPrincipalPolicy( PrincipalPolicy.WindowsPrincipal );
            Thread.CurrentPrincipal = ClaimsPrincipal.CreateFromPrincipal( Thread.CurrentPrincipal );

            //
            // Method 1. Simple access check using static method. 
            // Expect this to be most common method.
            //
            ClaimsPrincipalPermission.CheckAccess( "resource", "action" );

            //
            // Method 2. Programmatic check using the permission class
            // Follows model found at http://msdn.microsoft.com/en-us/library/system.security.permissions.principalpermission.aspx
            //
            ClaimsPrincipalPermission cpp = new ClaimsPrincipalPermission( "resource", "action" );
            cpp.Demand();

            //
            // Method 3. Access check interacting directly with the authorization manager.
            //            
            ClaimsAuthorizationManager am = new ClaimsAuthorizationManager();
            am.CheckAccess( new AuthorizationContext( (IClaimsPrincipal) Thread.CurrentPrincipal, "resource", "action" ) );

            //
            // Method 4. Call a method that is protected using the permission attribute class
            //
            ProtectedMethod();

            Console.WriteLine( "Press [Enter] to continue." );
            Console.ReadLine();
        }

        //
        // Declarative access check using the permission class
        //
        [ClaimsPrincipalPermission( SecurityAction.Demand, Resource = "resource", Operation = "action")]
        [ClaimsPrincipalPermission( SecurityAction.Demand, Resource = "resource1", Operation = "action1" )]
        static void ProtectedMethod()
        {
        }
    }
}
