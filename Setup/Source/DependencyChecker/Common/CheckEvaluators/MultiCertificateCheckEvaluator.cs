//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common.CheckEvaluators
{
    public class MultiCertificateCheckEvaluator : CertificateCheckEvaluator
    {
        public override bool Evaluate(Check check, IEvaluationContext context)
        {
            var checks = check.Value.Split('!');
            foreach (var c in checks)
            {
                var tempCheck = new Check { Value = c };
                if (!base.Evaluate(tempCheck, context))
                {
                    return false;
                }
            }
            return true;
        }
    }
}