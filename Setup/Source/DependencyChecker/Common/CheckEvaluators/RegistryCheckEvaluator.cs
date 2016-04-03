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

    public class RegistryCheckEvaluator : ICheckEvaluator
    {
        public bool Evaluate(Check check, IEvaluationContext context)
        {
            string[] configValues = check.Value.Split('|');
            string regPath = configValues[0];
            string reg64Path = configValues[1];
            string regValueName = configValues[2];
            string desiredValue = configValues[3];

            bool ret;
            if (string.IsNullOrEmpty(desiredValue))
            {
                if (string.IsNullOrEmpty(regValueName) && string.IsNullOrEmpty(desiredValue))
                {
                    ret = RegistryHelper.KeyExists(regPath);
                    ret |= RegistryHelper.KeyExists(reg64Path);
                }
                else
                {
                    ret = RegistryHelper.ValueExists(regPath, regValueName);
                    ret |= RegistryHelper.ValueExists(reg64Path, regValueName);
                }
            }
            else
            {
                ret = RegistryHelper.IsValueAt(regPath, regValueName, desiredValue);
                ret |= RegistryHelper.IsValueAt(reg64Path, regValueName, desiredValue);   
            }
            return ret;
        }
    }
}