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
    using System.Collections.Generic;
    using DependencyChecker.Common;

    public class MockCheckEvaluator : ICheckEvaluator
    {
        private readonly IDictionary<string, int> hitsPerCheck;
        private readonly IDictionary<string, bool> returnValues;

        public MockCheckEvaluator()
        {
            this.returnValues = new Dictionary<string, bool>();
            this.hitsPerCheck = new Dictionary<string, int>();
        }

        public IDictionary<string, int> HitsPerCheck
        {
            get { return this.hitsPerCheck; }
        }

        public IDictionary<string, bool> ReturnValues
        {
            get { return this.returnValues; }
        }

        public bool Evaluate(Check check, IEvaluationContext context)
        {
            this.hitsPerCheck[check.Name]++;
            return this.returnValues[check.Name];
        }

        public void ResetHitsPerCheck()
        {
            this.hitsPerCheck.Clear();
            foreach (string name in this.returnValues.Keys)
            {
                this.hitsPerCheck.Add(name, 0);
            }
        }
    }
}