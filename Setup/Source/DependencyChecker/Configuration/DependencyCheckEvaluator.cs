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

    public class DependencyCheckEvaluator : ConfigurationElement
    {
        private const string NameProperty = "name";
        private const string TypeProperty = "type";

        [ConfigurationProperty(NameProperty, IsRequired = true)]
        public string Name
        {
            get { return (string)base[NameProperty]; }
            set { base[NameProperty] = value; }
        }

        [ConfigurationProperty(TypeProperty, IsRequired = true)]
        public string Type
        {
            get { return (string)base[TypeProperty]; }
            set { base[TypeProperty] = value; }
        }
    }
}