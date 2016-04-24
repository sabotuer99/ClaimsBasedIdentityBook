namespace MyCustomToken
{
    using Microsoft.IdentityModel.Claims;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IdentityModel.Tokens;

    /// <summary>
    /// Summary description for MyCustomToken
    /// </summary>
    public class MyCustomToken : SecurityToken
    {
        private MyCustomTokenInternal myCustomTokenInternal;

        public MyCustomToken()
        {
        }

        public MyCustomToken(MyCustomTokenInternal myCustomTokenInternal)
        {
            // TODO: Complete member initialization
            this.myCustomTokenInternal = myCustomTokenInternal;
        }

        public override string Id
        {
            get { return myCustomTokenInternal.Id; }
        }

        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get { return null; }
        }

        public override DateTime ValidFrom
        {
            get { return myCustomTokenInternal.ValidFrom; }
        }

        public override DateTime ValidTo
        {
            get { return myCustomTokenInternal.ValidTo; }
        }

        public IEnumerable<Claim> Claims
        {
            get { return this.myCustomTokenInternal.Claims; }
        }
    }
}
