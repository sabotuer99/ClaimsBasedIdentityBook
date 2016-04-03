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
    using System.Configuration;
    using DependencyChecker.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ConfigurationLoadTest
    {
        [TestMethod]
        public void CanLoadCommonChecksConfiguration()
        {
            Configuration config = TestHelper.GetConfiguration();

            DependenciesSection dcSection = (DependenciesSection)config.GetSection(DependenciesSection.SectionName);
            DependencyCheckCollection commonChecks = dcSection.CommonChecks;
            Assert.AreEqual(6, commonChecks.Count);
        }

        [TestMethod]
        public void CanLoadOSConfiguration()
        {
            Configuration config = TestHelper.GetConfiguration();

            DependenciesSection dcSection = (DependenciesSection)config.GetSection(DependenciesSection.SectionName);
            Assert.IsNotNull(dcSection);
            DependencyGroupCollection commonChecks = dcSection.DependencyGroups;
            Assert.IsNotNull(commonChecks);
            Assert.AreEqual(4, commonChecks.Count);
        }

        [TestMethod]
        public void CanLoadOSSpecificChecksConfiguration()
        {
            Configuration config = TestHelper.GetConfiguration();

            DependenciesSection dcSection = (DependenciesSection)config.GetSection(DependenciesSection.SectionName);
            Assert.AreEqual(2, dcSection.DependencyGroups["MockOS"].Checks.Count);
        }
    }
}