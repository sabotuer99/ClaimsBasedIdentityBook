//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Configuration
{
    using System.Configuration;

    public class DependencyGroup : ConfigurationElement
    {
        private const string CheckCollectionProperty = "checks";
        private const string DependencyCollectionProperty = "";
        private const string NameProperty = "name";
        private const string OsBuildProperty = "OSBuildNumber";

        [ConfigurationProperty(OsBuildProperty, IsRequired = true)]
        public string BuildNumber
        {
            get { return (string)this[OsBuildProperty]; }
            set { this[OsBuildProperty] = value; }
        }

        [ConfigurationProperty(CheckCollectionProperty, IsDefaultCollection = false, IsRequired = false)]
        public DependencyCheckCollection Checks
        {
            get { return (DependencyCheckCollection)base[CheckCollectionProperty]; }
        }

        [ConfigurationProperty(DependencyCollectionProperty, IsDefaultCollection = true, IsRequired = true)]
        public DependencyElementCollection Dependencies
        {
            get { return (DependencyElementCollection)this[DependencyCollectionProperty]; }
        }

        [ConfigurationProperty(NameProperty)]
        public string Name
        {
            get { return (string)this[NameProperty]; }
            set { this[NameProperty] = value; }
        }
    }
}