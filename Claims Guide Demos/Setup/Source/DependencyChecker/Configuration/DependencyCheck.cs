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

    public class DependencyCheck : ConfigurationElement
    {
        private const string CheckTypeProperty = "checkType";
        private const string NameProperty = "name";
        private const string ValueProperty = "value";

        [ConfigurationProperty(CheckTypeProperty, IsRequired = true)]
        public string CheckType
        {
            get { return (string)base[CheckTypeProperty]; }
            set { base[CheckTypeProperty] = value; }
        }

        [ConfigurationProperty(NameProperty, IsRequired = true)]
        public string Name
        {
            get { return (string)base[NameProperty]; }
            set { base[NameProperty] = value; }
        }

        [ConfigurationProperty(ValueProperty, IsRequired = true)]
        public string Value
        {
            get { return (string)base[ValueProperty]; }
            set { base[ValueProperty] = value; }
        }
    }
}