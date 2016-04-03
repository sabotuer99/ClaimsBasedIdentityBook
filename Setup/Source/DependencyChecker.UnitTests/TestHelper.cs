//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.UnitTests
{
    using System.Collections;
    using System.Configuration;
    using System.Linq;
    using DependencyChecker.Configuration;

    internal sealed class TestHelper
    {
        public static int Count(IEnumerable source)
        {
            return source.Cast<object>().Count();
        }

        public static Configuration GetConfiguration()
        {
            var map = new ExeConfigurationFileMap { ExeConfigFilename = "TestConfiguration.config" };
            return ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
        }

        public static DependenciesSection GetDependenciesSection()
        {
            Configuration config = GetConfiguration();
            return (DependenciesSection)config.GetSection(DependenciesSection.SectionName);
        }
    }
}