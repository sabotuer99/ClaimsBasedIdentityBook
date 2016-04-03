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
    using Microsoft.Web.PlatformInstaller;

    public class WpiCheckEvaluator : ICheckEvaluator, IRequiresProductManager
    {
        private ProductManager productManager;

        public ProductManager ProductManager
        {
            get { return this.productManager; }
            set { this.productManager = value; }
        }

        public bool Evaluate(Check check, IEvaluationContext context)
        {
            var settings = check.Value.Split('!');
            bool ret = false;
            foreach (var setting in settings)
            {
                Product product = this.productManager.GetProduct(setting);
                ret = product.IsInstalled(false);
                if (!ret)
                {
                    break;
                }
            }
            return ret;
        }
    }
}