//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Litware.SimulatedIssuer
{
    public static class SimulatedActiveDirectory
    {
        // In a production scenario, the user would be authenticated using Windows Authentication or
        // against a database that stores all the company employees.
        // Possible values for the username that will be logged in are:
        // - LITWARE\\rick
        // * 'Sales' role
        public static string UserName
        {
            get { return "LITWARE\\rick"; }
        }
    }
}