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
    using System.Management;
    using DependencyChecker.Common.CheckEvaluators.Helpers;

    public class WmiCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            ManagementObjectSearcher searcher = WmiHelper.RunWmiQuery(check.Value);
            if (searcher.Get().Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}