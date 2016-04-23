namespace MyCustomToken
{
    using System;
    using Microsoft.IdentityModel.Tokens;
    using System.Xml;

    /// <summary>
    /// Summary description for MyCustomTokenHandler
    /// </summary>
    public class MyCustomTokenHandler : SecurityTokenHandler
    {
        public MyCustomTokenHandler()
        {
        }

        public override string[] GetTokenTypeIdentifiers()
        {
            return new string[] { "urn:mycustomtoken" };
        }

        public override Type TokenType
        {
            get { return typeof(MyCustomToken); }
        }

        public override bool CanReadToken(XmlReader reader)
        {
            if (reader.LocalName.Equals("MyCustomToken") &&
                reader.NamespaceURI.Equals("urn:mycustomtoken"))
            {
                return true;
            }

            return false;
        }
    }
}