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
using System.Linq;
using System.Linq.Expressions;
using System.Xml;
using Microsoft.IdentityModel.Claims;

namespace ClaimsBasedAuthorization
{
    class PolicyReader
    {
        static Expression<Func<IClaimsPrincipal, bool>> DefaultPolicy = ( icp ) => false;
        delegate bool HasClaimDelegate( IClaimsPrincipal p, string claimType, string claimValue );

        /// <summary>
        /// Delegate that checks if any ClaimsIdentity associated with the given principal has the claim specified
        /// </summary>
        static HasClaimDelegate HasClaim = delegate( IClaimsPrincipal p, string claimType, string claimValue )
        {
            return p.Identities.Any( s =>
                s.Claims.Any( c => c.ClaimType == claimType &&
                c.ValueType == ClaimValueTypes.String &&
                c.Value == claimValue ) );
        };

        /// <summary>
        /// Creates an instance of the policy reader
        /// </summary>
        public PolicyReader()
        {
        }

        /// <summary>
        /// Read the policy as a LINQ expression
        /// </summary>
        /// <param name="rdr">XmlDictionaryReader for the policy Xml</param>
        /// <returns></returns>
        public Expression<Func<IClaimsPrincipal, bool>> ReadPolicy( XmlDictionaryReader rdr )
        {
            if( rdr.Name != "policy" )
            {
                throw new InvalidOperationException( "Invalid policy document" );
            }
            
            rdr.Read();

            //
            // Instantiate a parameter for the IClaimsPrincipal so it can be evaluated against
            // each claim constraint.
            // 
            ParameterExpression subject = Expression.Parameter( typeof( IClaimsPrincipal ), "subject" );

            Expression<Func<IClaimsPrincipal, bool>> result = ReadNode( rdr, subject );
            rdr.ReadEndElement();

            return result;
        }

        /// <summary>
        /// Read the policy node
        /// </summary>
        /// <param name="rdr">XmlDictionaryReader for the policy Xml</param>
        /// <param name="subject">IClaimsPrincipal subject</param>
        /// <returns>A LINQ expression created from the policy</returns>
        private Expression<Func<IClaimsPrincipal, bool>> ReadNode( XmlDictionaryReader rdr, ParameterExpression subject )
        {
            Expression<Func<IClaimsPrincipal, bool>> policyExpression;

            if( !rdr.IsStartElement() )
            {
                throw new InvalidOperationException( "Invalid Policy format." );
            }

            switch( rdr.Name )
            {
                case "and":
                    policyExpression = ReadAnd( rdr, subject );
                    break;
                case "or":
                    policyExpression = ReadOr( rdr, subject );
                    break;
                case "claim":
                    policyExpression = ReadClaim( rdr );
                    break;
                default:
                    policyExpression = DefaultPolicy;
                    break;
            }

            return policyExpression;
        }

        /// <summary>
        /// Read the claim node
        /// </summary>
        /// <param name="rdr">XmlDictionaryReader of the policy Xml</param>
        /// <returns>A LINQ expression created from the claim node</returns>
        private Expression<Func<IClaimsPrincipal, bool>> ReadClaim( XmlDictionaryReader rdr )
        {
            string claimType = rdr.GetAttribute( "claimType" );
            string claimValue = rdr.GetAttribute( "claimValue" );

            Expression<Func<IClaimsPrincipal, bool>> hasClaim = ( icp ) => HasClaim( icp, claimType, claimValue );

            rdr.Read();

            return hasClaim;
        }

        /// <summary>
        /// Read the Or Node
        /// </summary>
        /// <param name="rdr">XmlDictionaryReader of the policy Xml</param>
        /// <param name="subject">IClaimsPrincipal subject</param>
        /// <returns>A LINQ expression created from the Or node</returns>
        private Expression<Func<IClaimsPrincipal, bool>> ReadOr( XmlDictionaryReader rdr, ParameterExpression subject )
        {
            rdr.Read();

            BinaryExpression lambda1 = Expression.OrElse(
                    Expression.Invoke( ReadNode( rdr, subject ), subject ),
                    Expression.Invoke( ReadNode( rdr, subject ), subject ) );

            rdr.ReadEndElement();

            Expression<Func<IClaimsPrincipal, bool>> lambda2 = Expression.Lambda<Func<IClaimsPrincipal, bool>>( lambda1, subject );
            return lambda2;
        }

        /// <summary>
        /// Read the And Node
        /// </summary>
        /// <param name="rdr">XmlDictionaryReader of the policy Xml</param>
        /// <param name="subject">IClaimsPrincipal subject</param>
        /// <returns>A LINQ expression created from the And node</returns>
        private Expression<Func<IClaimsPrincipal, bool>> ReadAnd( XmlDictionaryReader rdr, ParameterExpression subject )
        {
            rdr.Read();

            BinaryExpression lambda1 = Expression.AndAlso(
                    Expression.Invoke( ReadNode( rdr, subject ), subject ),
                    Expression.Invoke( ReadNode( rdr, subject ), subject ) );

            rdr.ReadEndElement();

            Expression<Func<IClaimsPrincipal, bool>> lambda2 = Expression.Lambda<Func<IClaimsPrincipal, bool>>( lambda1, subject );
            return lambda2;
        }
    }
}
