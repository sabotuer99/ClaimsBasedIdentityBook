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
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using System.Xml;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.XmlSignature;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;


namespace Microsoft.IdentityModel.Samples.CustomToken
{
    /// <summary>
    /// This class derives from the SecurityTokenHandler class so that the security token service can issue
    /// a custom token.
    /// </summary>
    class CustomTokenHandler : SecurityTokenHandler
    {
        public const string TokenName = "MyDecisionToken";
        public const string Decision = "Decision";
        public const string TokenNamespace = "http://localhost/TokenNamespace";
        public const string DecisionTokenType = "http://localhost/TokenType";
        public const string Id = "Id";
        public const string Key = "Key";
        public const string DecisionClaimType = "http://localhost/DecisionClaimType";

        TimeSpan _maxClockSkew = TimeSpan.FromMinutes( 5 ); // default skew is 5 minutes

        public CustomTokenHandler()
        {
        }

        /// <summary>
        /// Checks if the reader is positioned at an element named "MyDecisionToken".
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        /// <returns>True if it is a decision token.</returns>
        public override bool CanReadToken( XmlReader reader )
        {
            return ( reader.IsStartElement( TokenName, TokenNamespace ) );
        }

        /// <summary>
        /// Always returns true because this class provides serialization support for the custom token.
        /// </summary>
        public override bool CanWriteToken
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Creates a MyDecision type security token based on the description in the security token descriptor.
        /// </summary>
        /// <param name="tokenDescriptor">The token descriptor.</param>
        /// <returns>The security token.</returns>
        public override SecurityToken CreateToken( SecurityTokenDescriptor tokenDescriptor )
        {
            Console.WriteLine("CustomTokenHandler.CreateToken called");
            SymmetricProofDescriptor symmetric = tokenDescriptor.Proof as SymmetricProofDescriptor;

            if ( symmetric == null )
            {
                throw new InvalidOperationException( "The MyDecisionToken must be symmetric key based." );
            }

            bool decision = false;

            //
            // Retrieve the decision from the issued claims
            //
            foreach ( Claim claim in tokenDescriptor.Subject.Claims )
            {
                if ( StringComparer.Ordinal.Equals( claim.ClaimType, DecisionClaimType ) && 
                    StringComparer.Ordinal.Equals( claim.ValueType, ClaimValueTypes.Boolean ) )
                {
                    Console.WriteLine( "- decision claim found: {0}", claim.Value );
                    decision = Convert.ToBoolean( claim.Value );
                }
            }

            //
            // This is just an example to show how to issue a custom token. The key is created by the STS
            // through the proof token.
            //
            SecurityToken token = new MyDecisionToken( decision, tokenDescriptor.SigningCredentials, symmetric.GetKeyBytes() );

            //
            // Encrypt the token
            //
            EncryptingCredentials encryptingCredentials = GetEncryptingCredentials( tokenDescriptor );
            if ( encryptingCredentials != null )
            {
                token = new EncryptedSecurityToken( token, encryptingCredentials );
            }

            return token;
        }

        /// <summary>
        /// Override this method to change the token encrypting credentials. 
        /// </summary>
        /// <param name="tokenDescriptor">The token descriptor.</param>
        /// <returns>The token encrypting credentials.</returns>
        /// <exception cref="ArgumentNullException">When the given tokenDescriptor is null.</exception>
        protected virtual EncryptingCredentials GetEncryptingCredentials( SecurityTokenDescriptor tokenDescriptor )
        {
            if ( null == tokenDescriptor )
            {
                throw new ArgumentNullException( "tokenDescriptor" );
            }

            EncryptingCredentials encryptingCredentials = null;

            if ( null != tokenDescriptor.EncryptingCredentials )
            {
                encryptingCredentials = tokenDescriptor.EncryptingCredentials;

                if ( encryptingCredentials.SecurityKey is AsymmetricSecurityKey )
                {
                    //
                    // Here we will create a symmetric key 
                    //
                    encryptingCredentials = new EncryptedKeyEncryptingCredentials( encryptingCredentials, SecurityAlgorithmSuite.Default.DefaultSymmetricKeyLength, SecurityAlgorithmSuite.Default.DefaultEncryptionAlgorithm );
                }
            }

            return encryptingCredentials;
        }

        /// <summary>
        /// Since this sample does not use unattached reference, it returns the local reference id for 
        /// both attached and an unattached references for simplicity. Please modify it if you plan to use
        /// a custom unattached reference. 
        /// </summary>
        /// <param name="token">The security token.</param>
        /// <param name="attached">Boolean that indicates if an attached or unattached reference needs to be created.</param>
        /// <returns>A security key identifier clause.</returns>
        public override SecurityKeyIdentifierClause CreateSecurityTokenReference( SecurityToken token, bool attached )
        {
            return token.CreateKeyIdentifierClause<LocalIdKeyIdentifierClause>();
        }

        /// <summary>
        /// This gets called on the relying party on deserialization. 
        /// 
        /// The token looks like this
        ///     <MyDecisionToken Id="someId here">
        ///         <Decision>True</Decision>
        ///         <Key>key bytes</Key>
        ///         <Signature>...</Signature>
        ///     </MyDecisionToken>
        /// This sample is expecting the token in the right format. Omit some xml validation for simplicity.
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        /// <returns>The security token.</returns>
        /// <exception cref="InvalidOperationException">When the token cannot be read.</exception>
        public override SecurityToken ReadToken( XmlReader reader )
        {
            Console.WriteLine( "CustomTokenHandler.ReadToken called" );
            if ( !CanReadToken( reader ) )
            {
                throw new InvalidOperationException( "Reader must be positioned at the beginning of the DecisionToken. " );
            }

            EnvelopedSignatureReader envReader = new EnvelopedSignatureReader( reader, WSSecurityTokenSerializer.DefaultInstance );

            // Read the Id attribute
            string id = envReader.GetAttribute( Id );
            envReader.Read();
            envReader.MoveToContent();

            // Read the Decision element            
            string decision = envReader.ReadElementContentAsString( Decision, TokenNamespace );
            Console.WriteLine( "- decision read: {0}", decision);

            // Read the Key element
            byte[] key = Convert.FromBase64String( envReader.ReadElementString( Key, TokenNamespace ) );

            envReader.ReadEndElement();

            return new MyDecisionToken( Convert.ToBoolean( decision ), envReader.SigningCredentials, key, id );
        }

        /// <summary>
        /// Gets the System.Type of the SecurityToken this instance handles.
        /// </summary>
        public override Type TokenType
        {
            get
            {
                return typeof( MyDecisionToken );
            }
        }

        /// <summary>
        /// Gets the URI used in requests to identify a token of the type handled
        /// by this instance. 
        /// </summary>
        /// <remarks>
        /// For example, this should be the URI value used 
        /// in the RequestSecurityToken's TokenType element to request this
        /// sort of token.
        /// </remarks>
        public override string[] GetTokenTypeIdentifiers()
        {
            return new string[] { DecisionTokenType };
        }

        /// <summary>
        /// This gets called on the STS to serialize the token 
        /// The token looks like this
        ///     <MyDecisionToken Id="someId here">
        ///         <Decision>True</Decision>
        ///         <Key>key bytes</Key>
        ///         <Signature>...</Signature>
        ///     </MyDecisionToken>
        /// </summary>
        /// <param name="writer">The xml writer.</param>
        /// <param name="token">The security token that will be written.</param>
        /// <exception cref="ArgumentException">When the token is null.</exception>
        public override void WriteToken( XmlWriter writer, SecurityToken token )
        {
            Console.WriteLine( "CustomTokenHandler.WriteToken called" );
            MyDecisionToken decisionToken = token as MyDecisionToken;

            if ( decisionToken == null )
            {
                throw new ArgumentException( "The given token must be a MyDecisionToken", "token" );
            }

            EnvelopedSignatureWriter envWriter = new EnvelopedSignatureWriter( writer, decisionToken.SigningCredentials, decisionToken.Id, WSSecurityTokenSerializer.DefaultInstance );

            // Start the tokenName
            envWriter.WriteStartElement( TokenName, TokenNamespace );
            envWriter.WriteAttributeString( Id, token.Id );

            // Write the decision element
            Console.WriteLine( "- decision being written: {0}", decisionToken.Decision );
            envWriter.WriteElementString( Decision, TokenNamespace, Convert.ToString( decisionToken.Decision ) );

            // Write the key
            envWriter.WriteElementString( Key, TokenNamespace, Convert.ToBase64String( ( (MyDecisionToken)token ).RetrieveKeyBytes() ) );

            // Close the TokenName element
            envWriter.WriteEndElement();
        }

        /// <summary>
        /// Indicates whether this handler supports validation of tokens 
        /// handled by this instance.
        /// </summary>
        /// <returns>True if the class is capable of SecurityToken
        /// validation.</returns>
        public override bool CanValidateToken
        {
            get { return true; }
        }

        /// <summary>
        /// Validates a token and returns its claims.
        /// </summary>
        /// <param name="token">The security token.</param>
        /// <returns>The collection of claims contained in the token.</returns>
        public override ClaimsIdentityCollection ValidateToken( SecurityToken token )
        {
            Console.WriteLine( "CustomTokenHandler.ValidateToken called" );
            if ( token is MyDecisionToken )
            {
                //
                // First check if the token has become valid and has not yet expired.
                //
                DateTime now = DateTime.UtcNow;

                if ( now.Add( _maxClockSkew ) < token.ValidFrom )
                {
                    throw new SecurityTokenException( "The token has not become valid yet." );
                }

                if ( token.ValidTo.Add( _maxClockSkew ) < now )
                {
                    throw new SecurityTokenException( "The token has expired." );
                }

                //
                // Now generate the decision claim based on the decision
                //
                string issuer = null;
                foreach ( SecurityKeyIdentifierClause clause in ( (MyDecisionToken)token ).SigningCredentials.SigningKeyIdentifier )
                {
                    if ( clause is X509RawDataKeyIdentifierClause )
                    {
                        X509Certificate2 cert = new X509Certificate2( ( (X509RawDataKeyIdentifierClause)clause ).GetX509RawData() );
                        issuer = cert.SubjectName.Name;
                    }
                }

                string claimValue = ((MyDecisionToken) token).Decision.ToString();
                Claim decision = new Claim( CustomTokenHandler.DecisionClaimType, claimValue, ClaimValueTypes.Boolean, issuer );
                Console.WriteLine( "- claim added. type={0} value={1}", CustomTokenHandler.DecisionClaimType, claimValue );
                return new ClaimsIdentityCollection( new IClaimsIdentity[] { new ClaimsIdentity( new Claim[] { decision } ) } );
            }
            else
            {
                throw new InvalidOperationException( "We are expecting the MyDecision token" );
            }
        }

    }
}
