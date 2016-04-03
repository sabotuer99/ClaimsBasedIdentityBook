//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Configuration
{
    using System.Configuration;

    public class DependenciesSection : ConfigurationSection
    {
        public const string SectionName = "DependencyCheckerSection";
        private const string CommonCheckCollectionProperty = "checks";
        private const string CheckEvaluatorCollection = "checkEvaluators";
        private const string DependencyGroupCollectionProperty = "";
        private const string DescriptionProperty = "description";
        private const string MinimumRequirementsProperty = "MinimumRequirements";
        private const string TitleProperty = "title";

        [ConfigurationProperty(CommonCheckCollectionProperty, IsDefaultCollection = false, IsRequired = false)]
        public DependencyCheckCollection CommonChecks
        {
            get { return (DependencyCheckCollection)base[CommonCheckCollectionProperty]; }
        }

        [ConfigurationProperty(CheckEvaluatorCollection, IsDefaultCollection = false, IsRequired = false)]
        public DependencyCheckEvaluatorCollection CheckEvaluators
        {
            get { return (DependencyCheckEvaluatorCollection)base[CheckEvaluatorCollection]; }
        }

        [ConfigurationProperty(DependencyGroupCollectionProperty, IsDefaultCollection = true, IsRequired = true)]
        public DependencyGroupCollection DependencyGroups
        {
            get { return (DependencyGroupCollection)base[DependencyGroupCollectionProperty]; }
        }

        [ConfigurationProperty(DescriptionProperty)]
        public string Description
        {
            get { return (string)base[DescriptionProperty]; }
        }

        [ConfigurationProperty(MinimumRequirementsProperty, IsDefaultCollection = false, IsRequired = true)]
        public MinimumRequirements MinimumRequirements
        {
            get { return (MinimumRequirements)base[MinimumRequirementsProperty]; }
        }

        [ConfigurationProperty(TitleProperty)]
        public string Title
        {
            get { return (string)base[TitleProperty]; }
        }
    }
}