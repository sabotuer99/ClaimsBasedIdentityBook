//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common.CheckEvaluators.Expressions
{
    public class PropConjunction : PropExpression
    {
        public PropConjunction(PropExpression left, PropExpression right)
        {
            this.Left = left;
            this.Right = right;
        }

        public PropExpression Left { get; set; }

        public PropExpression Right { get; set; }
    }
}