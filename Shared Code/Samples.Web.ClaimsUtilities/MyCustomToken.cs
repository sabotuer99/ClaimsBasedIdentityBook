namespace MyCustomToken
{
    using System;
    using System.Collections.ObjectModel;
    using System.IdentityModel.Tokens;

    /// <summary>
    /// Summary description for MyCustomToken
    /// </summary>
    public class MyCustomToken : SecurityToken
    {
        public MyCustomToken()
        {
        }

        public override string Id
        {
            get { throw new NotImplementedException(); }
        }

        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime ValidFrom
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime ValidTo
        {
            get { throw new NotImplementedException(); }
        }
    }
}
