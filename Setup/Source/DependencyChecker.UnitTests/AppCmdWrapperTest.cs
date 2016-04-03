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
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass()]
    public class AppCmdWrapperTest
    {
        [TestMethod()]
        public void IsHttpsEnabledShouldReturnTrue()
        {
            AppCmdWrapper appCmd = new AppCmdWrapper(); 
            Assert.IsTrue(appCmd.IsHttpsEnabled());
        }
    }
}
