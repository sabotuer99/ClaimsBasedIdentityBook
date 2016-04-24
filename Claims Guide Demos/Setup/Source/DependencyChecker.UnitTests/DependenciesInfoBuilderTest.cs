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
    using DependencyChecker.Common;
    using DependencyChecker.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DependenciesInfoBuilderTest
    {
        [TestMethod]
        public void FindsExactMatch()
        {
            DependenciesSection section = TestHelper.GetDependenciesSection();
            var builder = new DependenciesInfoBuilder(null);

            DependenciesInfo info = builder.BuildDependenciesInfo(section, 100);

            Assert.IsNotNull(info);
            Assert.AreEqual(100, info.CompatibleOsBuild);
        }

        [TestMethod]
        public void CommonChecksAreIncluded()
        {
            DependenciesSection section = TestHelper.GetDependenciesSection();
            var builder = new DependenciesInfoBuilder(null);

            DependenciesInfo info = builder.BuildDependenciesInfo(section, 300);

            Assert.AreEqual(6, TestHelper.Count(info.EvaluationContext.GetCheckNames()));
        }

        [TestMethod]
        public void CustomOSChecksOverrideCommonChecks()
        {
            DependenciesSection section = TestHelper.GetDependenciesSection();
            var builder = new DependenciesInfoBuilder(null);

            DependenciesInfo info = builder.BuildDependenciesInfo(section, 100);

            Assert.AreEqual(7, TestHelper.Count(info.EvaluationContext.GetCheckNames()));
        }

        [TestMethod]
        public void LoadsDependeciesList()
        {
            DependenciesSection section = TestHelper.GetDependenciesSection();
            var builder = new DependenciesInfoBuilder(null);

            DependenciesInfo info = builder.BuildDependenciesInfo(section, 200);

            Assert.AreEqual(2, info.Dependencies.Count);
            Assert.AreEqual("MinimumRequirement", info.Dependencies[0].Title);
            Assert.AreEqual(true, info.Dependencies[0].Enabled);
            Assert.AreEqual("This might be handy in all cases", info.Dependencies[0].Explanation);
            Assert.AreEqual("http://www.microsoft.com", info.Dependencies[0].DownloadUrl);
            Assert.IsTrue(string.IsNullOrEmpty(info.Dependencies[0].InfoUrl));
            Assert.IsTrue(string.IsNullOrEmpty(info.Dependencies[0].ScriptName));
            Assert.AreEqual("Optional", info.Dependencies[0].Category);
            Assert.AreEqual("Reg2", info.Dependencies[0].Check);
            Assert.AreEqual("Microsoft Visual Studio 2005", info.Dependencies[1].Title);
            Assert.AreEqual(true, info.Dependencies[1].Enabled);
            Assert.AreEqual("In order to use the Guidance Package, you will need to install Visual Studio 2005", info.Dependencies[1].Explanation);
            Assert.AreEqual("http://www.microsoft.com/VisualStudio", info.Dependencies[1].DownloadUrl);
            Assert.IsTrue(string.IsNullOrEmpty(info.Dependencies[1].InfoUrl));
            Assert.IsTrue(string.IsNullOrEmpty(info.Dependencies[1].ScriptName));
            Assert.AreEqual("MyCategory", info.Dependencies[1].Category);
            Assert.AreEqual("(Reg1 || Soft1) && Exp12", info.Dependencies[1].Check);
        }

        [TestMethod]
        public void ChecksMinimumBuildNumber()
        {
            DependenciesSection section = TestHelper.GetDependenciesSection();

            var builder = new DependenciesInfoBuilder(null);

            builder.BuildDependenciesInfo(section, 20);

            try
            {
                builder.BuildDependenciesInfo(section, 5);
                Assert.Fail("Does not check for minimum build.");
            }
            catch (NotSupportedOperatingSystemException)
            {
            }
        }
    }
}