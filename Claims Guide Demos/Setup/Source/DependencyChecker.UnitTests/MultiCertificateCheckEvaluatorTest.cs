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
    using DependencyChecker.Common.CheckEvaluators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MultiCertificateCheckEvaluatorTest
    {
        [TestMethod]
        public void ShouldEvaluateToTrue()
        {
            var evaluator = new MultiCertificateCheckEvaluator();
            Check check = new Check { Name = "Certficate", Value = "LocalMachine,My,CN=adatum!LocalMachine,My,CN=fabrikam" };
            IEvaluationContext nullContext = null;

            bool evaluate = evaluator.Evaluate(check, nullContext);

            Assert.IsTrue(evaluate);
        }


        [TestMethod]
        public void ShouldEvaluateToFalse()
        {
            var evaluator = new MultiCertificateCheckEvaluator();
            Check check = new Check { Name = "Certficate", Value = "LocalMachine,My,CN=adatum!LocalMachine,My,CN=noexist" };
            IEvaluationContext nullContext = null;

            bool evaluate = evaluator.Evaluate(check, nullContext);

            Assert.IsFalse(evaluate);
        }
    }
}