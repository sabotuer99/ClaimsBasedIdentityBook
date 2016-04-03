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
    using System;
    using DependencyChecker.Common;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EvaluatorContextTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CantStoreNullEvaluator()
        {
            EvaluationContext context = new EvaluationContext();
            context.SetEvaluatorForCheckType("myType", null);
        }

        [TestMethod]
        public void SetEvaluatorStoreEvaluator()
        {
            EvaluationContext context = new EvaluationContext();
            MockCheckEvaluator evaluator = new MockCheckEvaluator();
            context.SetEvaluatorForCheckType("myType", evaluator);
            ICheckEvaluator evaluator2 = context.GetEvaluatorForCheckType("myType");

            Assert.AreSame(evaluator, evaluator2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CantOverwriteEvaluators()
        {
            EvaluationContext context = new EvaluationContext();
            MockCheckEvaluator evaluator = new MockCheckEvaluator();
            context.SetEvaluatorForCheckType("myType", evaluator);
            context.SetEvaluatorForCheckType("myType", evaluator);
        }

        private class MockCheckEvaluator : ICheckEvaluator
        {
            public bool Evaluate(Check check, IEvaluationContext context)
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }
    }
}