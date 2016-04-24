namespace MyCustomToken
{
    using System;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Microsoft.IdentityModel.Claims;
    using mim =  Microsoft.IdentityModel.Tokens;

    /// <summary>
    /// Summary description for MyCustomTokenHandler
    /// </summary>
    public class MyCustomTokenHandler : mim.SecurityTokenHandler
    {
        private const string TokenNamespace = "urn:mycustomtoken";

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

        public override SecurityToken ReadToken(XmlReader reader)
        {
            // Check token signature using EnvelopedSignatureReader (more performant but more complex to use)
            // or SignedXml (easier to use but less performant)

            MyCustomToken token = new MyCustomToken(
                new MyCustomTokenInternal()
                {
                    Id = reader.GetAttribute("Id", TokenNamespace),
                    ValidFrom = XmlConvert.ToDateTime(reader.GetAttribute("ValidFrom", TokenNamespace)),
                    ValidTo = XmlConvert.ToDateTime(reader.GetAttribute("ValidTo", TokenNamespace)),
                    Audience = reader.GetAttribute("Audience", TokenNamespace),
                    Issuer = reader.GetAttribute("Issuer", TokenNamespace),
                    Claims = from el in XElement.Load(reader).Elements(XName.Get("Claim", TokenNamespace)) select new Claim(el.Attribute("Namespace").Value, el.Value)
                });

            return token;
        }

        public override bool CanValidateToken
        {
            get
            {
                return true;
            }
        }

        public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
        {
            ClaimsIdentityCollection idColl = new ClaimsIdentityCollection();
            IClaimsIdentity id = new ClaimsIdentity((token as MyCustomToken).Claims);
            idColl.Add(id);

            return idColl;
        }
    }
}