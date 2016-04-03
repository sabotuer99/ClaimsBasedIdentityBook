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

    /// <summary>
    ///   This class represents dependencies and evaluation context.
    /// </summary>
    public class DependenciesInfo
    {
        public int CompatibleOsBuild { get; set; }

        public IList<Dependency> Dependencies { get; set; }
        public IEvaluationContext EvaluationContext { get; set; }
    }
}