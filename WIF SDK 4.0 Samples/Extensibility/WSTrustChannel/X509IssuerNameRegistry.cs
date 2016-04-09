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

using System.Collections.Generic;
using System.IdentityModel.Tokens;

using Microsoft.IdentityModel.Tokens;

namespace Microsoft.IdentityModel.Samples.TrustChannel
{
    /// <summary>
    /// Implements an IssuerNameRegistry that only recognizes a specific
    /// set of issuer subject names.
    /// </summary>
    class X509IssuerNameRegistry : IssuerNameRegistry
    {
        List<string> _trustedSubjectNames = new List<string>();

        /// <summary>
        /// Constructs an instance of X509IssuerNameRegistry.
        /// </summary>
        /// <param name="trustedSubjectNames">The subject names that can be recognized.</param>
        public X509IssuerNameRegistry( params string[] trustedSubjectNames )
        {
            _trustedSubjectNames = new List<string>( trustedSubjectNames );
        }

        /// <summary>
        /// Determines what the issuer name will be on claims contained in tokens.
        /// </summary>
        /// <param name="securityToken">
        /// The security token to extract the issuer name from. This token typically signed the
        /// token containing claims and represents the issuer.
        /// </param>
        /// <returns>The issuer name to be put on claims.</returns>
        public override string GetIssuerName( SecurityToken securityToken )
        {
            X509SecurityToken x509Token = securityToken as X509SecurityToken;
            if ( x509Token != null )
            {
                //
                // Check the list of trusted/permissible issuers
                //
                if ( _trustedSubjectNames.Contains( x509Token.Certificate.SubjectName.Name ) )
                {
                    return x509Token.Certificate.SubjectName.Name;
                }
            }

            //
            // Complain in all other situations.
            //
            throw new SecurityTokenException( "Untrusted issuer." );
        }
    }
}
