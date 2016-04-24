//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Microsoft.Samples.DPE.OAuth.Tokens
{
    using System;
    using System.IdentityModel.Tokens;
    using System.Web;
    using System.Xml;
    using Microsoft.IdentityModel.Tokens;

    public abstract class StringTokenHandler : SecurityTokenHandler
    {
        public override bool CanWriteToken
        {
            get
            {
                return true;
            }
        }

        public override bool CanValidateToken
        {
            get { return false; }
        }

        public override bool CanReadToken(XmlReader reader)
        {
            if (reader == null)
            {
                // TODO throw reader null exception
            }

            return reader.IsStartElement("stringToken");
        }

        public override SecurityToken ReadToken(XmlReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }

            if (!this.CanReadToken(reader))
            {
                throw new Exception("cannot read token");
            }

            string token = reader.ReadElementContentAsString();            
            var securityToken = this.GetTokenFromString(token);

            return securityToken;
        }

        public override void WriteToken(XmlWriter writer, SecurityToken token)
        {
            writer.WriteStartElement("stringToken");
            string tokenString = this.GetTokenAsString(token);
            writer.WriteString(tokenString);
            writer.WriteEndElement();
        }

        public abstract SecurityToken GetTokenFromString(string token);

        public abstract string GetTokenAsString(SecurityToken token);
    }
}