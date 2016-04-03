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
    using System;
    using System.Configuration;

    public class DependencyElement : ConfigurationElement
    {
        private const string CategoryProperty = "category";
        private const string CheckProperty = "check";
        private const string DownloadUrlProperty = "downloadUrl";
        private const string EnabledProperty = "enabled";
        private const string ExplanationProperty = "explanation";
        private const string InfoUrlProperty = "infoUrl";
        private const string ScriptNameProperty = "scriptName";
        private const string SettingsProperty = "settings";
        private const string TitleProperty = "title";

        [ConfigurationProperty(CategoryProperty)]
        public string Category
        {
            get { return (string)base[CategoryProperty]; }
            set { base[CategoryProperty] = value; }
        }

        [ConfigurationProperty(CheckProperty)]
        public string Check
        {
            get { return (string)base[CheckProperty]; }
            set { base[CheckProperty] = value; }
        }

        [ConfigurationProperty(DownloadUrlProperty)]
        public string DownloadUrl
        {
            get
            {
                var downloadUrl = (string)base[DownloadUrlProperty];

                if (Uri.IsWellFormedUriString(downloadUrl, UriKind.Absolute))
                {
                    return downloadUrl;
                }

                return String.Empty;
            }
            set { base[DownloadUrlProperty] = value; }
        }

        [ConfigurationProperty(EnabledProperty)]
        public bool Enabled
        {
            get { return (bool)base[EnabledProperty]; }
            set { base[EnabledProperty] = value; }
        }


        [ConfigurationProperty(ExplanationProperty)]
        public string Explanation
        {
            get { return (string)base[ExplanationProperty]; }
            set { base[ExplanationProperty] = value; }
        }


        [ConfigurationProperty(InfoUrlProperty)]
        public string InfoUrl
        {
            get
            {
                var infoUrl = (string)base[InfoUrlProperty];

                if (Uri.IsWellFormedUriString(infoUrl, UriKind.Absolute))
                {
                    return infoUrl;
                }

                return String.Empty;
            }
            set { base[InfoUrlProperty] = value; }
        }

        [ConfigurationProperty(ScriptNameProperty)]
        public string ScriptName
        {
            get { return (string)base[ScriptNameProperty]; }
            set { base[ScriptNameProperty] = value; }
        }

        [ConfigurationProperty(SettingsProperty)]
        public string Settings
        {
            get { return (string)base[SettingsProperty]; }
            set { base[SettingsProperty] = value; }
        }

        [ConfigurationProperty(TitleProperty)]
        public string Title
        {
            get { return (string)base[TitleProperty]; }
            set { base[TitleProperty] = value; }
        }
    }
}