//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------


using System;

using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;

namespace Microsoft.IdentityModel.Samples.CustomRequestSecurityToken
{
    /// <summary>
    /// This class derives from WSTrust13RequestSerializer, and adds support for deserialization of the
    /// custom element under the RST. The serialization part is handled by the WCF client side. See Program.GetClientBinding 
    /// method for more information on this.
    /// </summary>
    public class CustomWSTrust13RequestSerializer : WSTrust13RequestSerializer
    {
        /// <summary>
        /// Override ReadXml method to deserialize the custom element inside the RST.
        /// </summary>
        /// <param name="reader">The xml reader to read from</param>
        /// <param name="rst">The rst object that is going to be populated with the new custom element</param>
        /// <param name="serializer">The token serializer to serialize the token related elements</param>
        public override void ReadXmlElement( System.Xml.XmlReader reader, RequestSecurityToken rst, WSTrustSerializationContext context )
        {
            if ( reader == null )
            {
                throw new ArgumentNullException( "reader" );
            }

            if ( rst == null )
            {
                throw new ArgumentNullException( "rst" );
            }

            if ( context == null )
            {
                throw new ArgumentNullException( "context" );
            }

            if ( reader.IsStartElement( CustomElementConstants.LocalName, CustomElementConstants.Namespace ) )
            {
                if ( rst is CustomRequestSecurityToken )
                {
                    CustomRequestSecurityToken customRST = rst as CustomRequestSecurityToken;
                    if ( customRST != null )
                    {
                        //
                        // reading the custom element in the RST
                        //
                        customRST.CustomElement = reader.ReadElementContentAsString();
                    }
                }
           }
            else
            {
                //
                // The rest is just normal thing
                //
                base.ReadXmlElement( reader, rst, context );
            }
        }

        public override RequestSecurityToken CreateRequestSecurityToken()
        {
            return new CustomRequestSecurityToken( CustomElementConstants.DefaultElementValue );
        }
    }

    /// <summary>
    /// This class derives from the RequestSecurityToken class and demonstrates the usage of custom 
    /// elements in the RequestSecurityToken class. The custom element needs to be appropriately
    /// serialized/deserialized. An example of which is provided in the class CustomWSTrust13RequestSerializer.
    /// </summary>
    public class CustomRequestSecurityToken : RequestSecurityToken
    {
        string _customElement;
        string _localName;
        string _namespace;

        public CustomRequestSecurityToken(  string customElementValue )
            : base()
        {
            if ( string.IsNullOrEmpty( customElementValue ) )
            {
                throw new ArgumentNullException( "customElementValue" );
            }
            _customElement = customElementValue;
            _localName = CustomElementConstants.LocalName;
            _namespace = CustomElementConstants.Namespace;
        }

        public string CustomElement
        {
            get
            {
                return _customElement;
            }
            set
            {
                _customElement = value;
            }
        }
        
        public string LocalName
        {
            get
            {
                return _localName;
            }
        }

        public string Namespace
        {
            get
            {
                return _namespace;
            }
        }
    }
}
