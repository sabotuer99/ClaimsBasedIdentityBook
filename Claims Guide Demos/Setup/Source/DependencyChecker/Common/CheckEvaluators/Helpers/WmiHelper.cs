//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common.CheckEvaluators.Helpers
{
    using System.Management;

    public static class WmiHelper
    {
        public static ManagementObjectSearcher RunWmiQuery(string query)
        {
            var selectQuery = new SelectQuery(query);
            var searcher = new ManagementObjectSearcher(selectQuery);

            return searcher;
        }
    }
}