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
    using System.IO;
    using DependencyChecker.Common;
    using DependencyChecker.Common.CheckEvaluators;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Web.PlatformInstaller;

    [TestClass]
    public class WpiCheckEvaluatorTest
    {
        private static ProductManager productManager;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            productManager = new ProductManager();
            var assembly = typeof(WpiCheckEvaluatorTest).Assembly;
            var config = assembly.GetManifestResourceStream("DependencyChecker.UnitTests.Dependencies.xml");

            var tempFile = Path.GetTempFileName();
            using (var sr = new StreamReader(config))
            {
                using (var sw = new StreamWriter(tempFile))
                {
                    sw.Write(sr.ReadToEnd());
                }
            }
            var uri = new Uri("file://" + tempFile);
            productManager.Load(uri);
        }

        [TestMethod]
        public void ShouldEvaluateToFalseMySql()
        {
            var evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            var check = new Check { CheckType = "WPI", Value = "MySQL" };
            IEvaluationContext nullContext = null;
            
            bool evaluate = evaluator.Evaluate(check, nullContext);

            Assert.IsFalse(evaluate);
        }

        [TestMethod]
        public void ShouldEvaluateToTrueWifRuntime()
        {
            var evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            var check = new Check { CheckType = "WPIRuntime", Value = "WIFRuntime" };
            IEvaluationContext nullContext = null;

            bool evaluate = evaluator.Evaluate(check, nullContext);

            Assert.IsTrue(evaluate);
        }

        [TestMethod]
        public void ShouldEvaluateToTrueWifSdk()
        {
            WpiCheckEvaluator evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            Check check = new Check { CheckType = "WPISDK", Value = "WIFSDK" };
            IEvaluationContext nullContext = null;
            
            Assert.IsTrue(evaluator.Evaluate(check, nullContext));
        }

        [TestMethod]
        public void ShouldEvaluateToTrueWCFHTTP()
        {
            WpiCheckEvaluator evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            Check check = new Check { CheckType = "WCFHTTP", Value = "WCFHTTP" };
            IEvaluationContext nullContext = null;
            ;
            Assert.IsTrue(evaluator.Evaluate(check, nullContext));
        }

        [TestMethod]
        public void ShouldEvaluateToTrueIIS7CGCC()
        {
            WpiCheckEvaluator evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            Check check = new Check { CheckType = "IIS7CGCC", Value = "IIS7CGCC" };
            IEvaluationContext nullContext = null;
            ;
            Assert.IsTrue(evaluator.Evaluate(check, nullContext));
        }

        [TestMethod]
        public void ShouldEvaluateToTrueWindowsAzureTools()
        {
            WpiCheckEvaluator evaluator = new WpiCheckEvaluator();
            evaluator.ProductManager = productManager;
            Check check = new Check { CheckType = "WindowsAzureToolsVS2010", Value = "WindowsAzureToolsVS2010" };
            IEvaluationContext nullContext = null;
            ;
            Assert.IsTrue(evaluator.Evaluate(check, nullContext));
        }
    }
}