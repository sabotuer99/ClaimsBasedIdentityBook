//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common
{
    using System;
    using System.Collections.Generic;
    using DependencyChecker.Common.CheckEvaluators;

    public class EvaluationContext : IEvaluationContext
    {
        private readonly Dictionary<string, ICheckEvaluator> evaluators;

        private readonly ExpressionCheckEvaluator expressionEvaluator;
        private readonly Dictionary<string, Check> namedChecks;

        public EvaluationContext()
        {
            this.evaluators = new Dictionary<string, ICheckEvaluator>();
            this.namedChecks = new Dictionary<string, Check>();
            this.expressionEvaluator = new ExpressionCheckEvaluator();
        }

        public Check this[string name]
        {
            get { return this.namedChecks[name]; }
            set
            {
                if (this.namedChecks.ContainsKey(name))
                {
                    this.namedChecks.Remove(name);
                }
                this.namedChecks.Add(name, value);
            }
        }

        public bool Evaluate(Check check)
        {
            return this.GetEvaluatorForCheckType(check.CheckType).Evaluate(check, this);
        }

        public bool Evaluate(string check)
        {
            return this.expressionEvaluator.Evaluate(check, this);
        }

        public IEnumerable<string> GetCheckNames()
        {
            return this.namedChecks.Keys;
        }

        public ICheckEvaluator GetEvaluatorForCheckType(string checkType)
        {
            return this.evaluators[checkType];
        }

        public void SetEvaluatorForCheckType(string checkType, ICheckEvaluator evaluator)
        {
            if (evaluator == null)
            {
                throw new ArgumentNullException();
            }
            this.evaluators.Add(checkType, evaluator);
        }
    }
}