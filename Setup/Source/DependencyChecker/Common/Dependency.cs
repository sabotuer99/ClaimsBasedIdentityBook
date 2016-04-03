//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common
{
    public class Dependency
    {
        public string Category { get; set; }

        public string Check { get; set; }
        public string DownloadUrl { get; set; }
        public bool Enabled { get; set; }

        public string Explanation { get; set; }

        public string InfoUrl { get; set; }

        public string ScriptName { get; set; }

        public string Settings { get; set; }
        public string Title { get; set; }
    }
}