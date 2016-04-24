//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Microsoft.Samples.DPE.OAuth.ProtectedResource
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IdentityModel.Tokens;
    using System.Linq;

    public class DictionaryBasedKeyIdentifierClause : SecurityKeyIdentifierClause
    {
        private IDictionary<string, string> dictionary;

        public DictionaryBasedKeyIdentifierClause(IDictionary<string, string> dictionary)
            : base(null)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException("dictionary");
            }

            this.dictionary = dictionary;
        }

        public IDictionary<string, string> Dictionary
        {
            get
            {
                return this.dictionary;
            }
        }

        public override bool Matches(SecurityKeyIdentifierClause keyIdentifierClause)
        {
            DictionaryBasedKeyIdentifierClause objB = keyIdentifierClause as DictionaryBasedKeyIdentifierClause;
            return object.ReferenceEquals(this, objB) || ((objB != null) && objB.Matches(this.dictionary));
        }

        public bool Matches(IDictionary<string, string> dictionary)
        {
            return this.dictionary.Keys.SequenceEqual(dictionary.Keys) &&
                   this.dictionary.Values.SequenceEqual(dictionary.Values);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "DictionaryBasedKeyIdentifierClause(Keys = '{0}', Values ='{1}')", string.Join(",", this.dictionary.Keys.ToArray()), string.Join(",", this.dictionary.Values.ToArray()));
        }        
    }
}
