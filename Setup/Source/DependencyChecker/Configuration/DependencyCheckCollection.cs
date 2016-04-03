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

    public class DependencyCheckCollection : ConfigurationElementCollection
    {
        public DependencyCheckCollection()
        {
            this.AddElementName = "check";
        }

        public new DependencyCheck this[string name]
        {
            get { return (DependencyCheck)this.BaseGet(name); }
        }


        public DependencyCheck this[int index]
        {
            get { return (DependencyCheck)this.BaseGet(index); }
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
            return new DependencyCheck();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DependencyCheck)element).Name;
        }
    }
}