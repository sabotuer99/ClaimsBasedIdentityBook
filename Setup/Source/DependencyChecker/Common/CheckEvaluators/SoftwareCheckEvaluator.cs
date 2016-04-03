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
    using DependencyChecker.Common.CheckEvaluators.Helpers;

    public class SoftwareCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            return RegistryHelper.IsInKey(@"SOFTWARE\microsoft\Windows\CurrentVersion\Uninstall", 
                                          "DisplayName", 
                                          check.Value);
        }
    }
}