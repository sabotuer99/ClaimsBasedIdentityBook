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

    public class DependencyElementCollection : ConfigurationElementCollection
    {
        public DependencyElementCollection()
        {
            this.AddElementName = "dependency";
        }


        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        public DependencyElement this[int index]
        {
            get { return (DependencyElement)this.BaseGet(index); }
            set
            {
                if (this.BaseGet(index) != null)
                {
                    this.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }


        protected override ConfigurationElement CreateNewElement()
        {
            return new DependencyElement();
        }


        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DependencyElement)element).Title;
        }

        protected override bool IsElementName(string elementName)
        {
            if (string.IsNullOrEmpty(elementName) || (elementName != "dependency"))
            {
                return false;
            }
            return true;
        }
    }
}