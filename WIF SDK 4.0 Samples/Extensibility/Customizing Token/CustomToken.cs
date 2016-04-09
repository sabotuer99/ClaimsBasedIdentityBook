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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Samples.CustomToken
{
    /// <summary>
    /// Defines a custom token which is a symmetric key based token. 
    /// </summary>
    class MyDecisionToken : SecurityToken
    {
        string _id = UniqueId.CreateRandomId();
        bool _decision = false;
        DateTime _creation ;
        DateTime _expiration;
        List<SecurityKey> _keys = new List<SecurityKey>( 1 );
        byte[] _keyBytes;
        SigningCredentials _signingCredentials;

        /// <summary>
        /// This constructor is being used on the STS side to initialize a token
        /// </summary>
        /// <param name="decision">The decision.</param>
        /// <param name="signingCredentials">The signing credentials.</param>
        /// <param name="keyBytes">The key.</param>
        public MyDecisionToken(bool decision, SigningCredentials signingCredentials, byte[] keyBytes)
            : this( decision, signingCredentials, keyBytes, UniqueId.CreateRandomId() )
        {
        }

        
        /// <summary>
        /// This constructor is usually used on the Relying Party side to be able to 
        /// deserialize the token including the id. By default the token expires in 4 hours.
        /// </summary>
        /// <param name="decision">The decision.</param>
        /// <param name="signingCredentials">The signing credentials.</param>
        /// <param name="keyBytes">The key.</param>
        /// <param name="id">The id.</param>
        public MyDecisionToken( bool decision, SigningCredentials signingCredentials, byte[] keyBytes, string id )
            : this( decision, signingCredentials, keyBytes, id, DateTime.UtcNow, DateTime.UtcNow.AddHours( 4 ) )
        {
        }

        /// <summary>
        /// Use this constructor if you need to customize the validFrom and validTo values of the token.
        /// </summary>
        /// <param name="decision">The decision.</param>
        /// <param name="signingCredentials">The signing credentials.</param>
        /// <param name="keyBytes">The key.</param>
        /// <param name="id">The id.</param>
        /// <param name="validFrom">The time the token is valid from.</param>
        /// <param name="validTo">The time the token is valid to.</param>
        public MyDecisionToken( bool decision, SigningCredentials signingCredentials, byte[] keyBytes, string id, DateTime validFrom, DateTime validTo )
            : base()
        {
            if ( signingCredentials == null )
            {
                throw new ArgumentNullException( "signingCredentials" );
            }

            validFrom = validFrom.ToUniversalTime();
            validTo = validTo.ToUniversalTime();

            if ( validFrom > validTo )
            {
                throw new InvalidOperationException( "ValidFrom must be earlier than ValidTo." );
            }

            _signingCredentials = signingCredentials;
            _decision = decision;
            _id = id;
            _keyBytes = keyBytes;
            _keys.Add( new InMemorySymmetricSecurityKey(keyBytes) );
            _creation = validFrom;
            _expiration = validTo; 
        }

        /// <summary>
        /// Returns the decision.
        /// </summary>
        public bool Decision
        {
            get
            {
                return _decision;
            }
        }

        /// <summary>
        /// Returns a random ID.
        /// </summary>
        public override string Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Gets the creation time of this token.
        /// </summary>
        public override DateTime ValidFrom
        {
            get
            {
                return _creation;
            }
        }

        /// <summary>
        /// Gets the expiration time of this token. 
        /// </summary>
        public override DateTime ValidTo
        {
            get
            {
                return _expiration;
            }
        }

        /// <summary>
        /// Retrieves the key bytes.
        /// </summary>
        /// <returns>The key bytes.</returns>
        public byte[] RetrieveKeyBytes()
        {
            InMemorySymmetricSecurityKey key = SecurityKeys[0] as InMemorySymmetricSecurityKey;
            return key.GetSymmetricKey();
        }

        /// <summary>
        /// Returns a collection of security key.
        /// </summary>
        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get
            {
                return _keys.AsReadOnly();
            }
        }

        /// <summary>
        /// Returns the signing credential.
        /// </summary>
        public SigningCredentials SigningCredentials
        {
            get
            {
                return _signingCredentials;
            }
        }
    }
}
