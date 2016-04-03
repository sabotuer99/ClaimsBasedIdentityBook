//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Adatum.SimulatedIssuer
{
    using System;
    using System.Security.Principal;
    using System.Web;

    public static class SimulatedWindowsAuthenticationOperations
    {
        public static void LogOnUser(string authenticatedUser, HttpContext context, HttpRequest request, HttpResponse response)
        {
            var genericIdentity = new GenericIdentity(authenticatedUser);
            context.User = new GenericPrincipal(genericIdentity, null);
            CreateSimulatedWindowsAuthenticationCookie(authenticatedUser, request, response);
        }

        public static void LogOutUser(HttpRequest request, HttpResponse response)
        {
            var simulatedWindowsAuthenticationCookie = new HttpCookie(".WINAUTH")
                                                           {
                                                               Path = request.ApplicationPath, 
                                                               Expires = DateTime.Now.AddDays(-1)
                                                           };
            response.Cookies.Add(simulatedWindowsAuthenticationCookie);
        }

        public static bool TryToAuthenticateUser(HttpContext context, HttpRequest request, HttpResponse response)
        {
            var simulatedWindowsAuthenticationCookie = request.Cookies[".WINAUTH"];

            if (simulatedWindowsAuthenticationCookie == null)
            {
                return false;
            }

            var authenticatedUser = simulatedWindowsAuthenticationCookie.Value;
            LogOnUser(authenticatedUser, context, request, response);

            return true;
        }

        private static void CreateSimulatedWindowsAuthenticationCookie(string authenticatedUser, HttpRequest request, HttpResponse response)
        {
            var simulatedWindowsAuthenticationCookie = new HttpCookie(".WINAUTH", authenticatedUser)
                                                           {
                                                               Path = request.ApplicationPath
                                                           };
            response.Cookies.Add(simulatedWindowsAuthenticationCookie);
        }
    }
}