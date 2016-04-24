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
    using System.Collections.Generic;

    public interface IEvaluationContext
    {
        /// <summary>
        ///   Returns the Check object that corresponds to name
        /// </summary>
        /// <param name = "name"></param>
        /// <returns></returns>
        Check this[string name] { get; set; }

        bool Evaluate(Check check);

        bool Evaluate(string check);

        IEnumerable<string> GetCheckNames();
        ICheckEvaluator GetEvaluatorForCheckType(string checkType);
    }
}