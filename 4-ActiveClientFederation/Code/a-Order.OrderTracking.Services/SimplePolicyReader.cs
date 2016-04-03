//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


// Microsoft patterns & practices: Guide to Claims Based Authentication Copyright � Microsoft Corporation.  All Rights Reserved. This code released under the terms of the Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)


namespace AOrder.OrderTracking.Services
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Xml;
    using Microsoft.IdentityModel.Claims;

    public class SimplePolicyReader
    {
        private static readonly Expression<Func<IClaimsPrincipal, bool>> DefaultPolicy = icp => false;

        private static readonly HasClaimDelegate HasClaimDelegateHandler = delegate(IClaimsPrincipal p, string claimType, string claimValue)
        {
            return p.Identities.Any(s =>
                s.Claims.Any(c => c.ClaimType == claimType &&
                c.ValueType == ClaimValueTypes.String &&
                c.Value.Equals(claimValue, StringComparison.OrdinalIgnoreCase)));
        };

        private delegate bool HasClaimDelegate(IClaimsPrincipal p, string claimType, string claimValue);

        public Expression<Func<IClaimsPrincipal, bool>> ReadPolicy(XmlReader reader)
        {
            if (reader.Name != "policy")
            {
                throw new InvalidOperationException("Invalid policy document");
            }

            reader.Read();

            // Instantiate a parameter for the IClaimsPrincipal so it can be evaluated against each claim constraint.
            ParameterExpression subject = Expression.Parameter(typeof(IClaimsPrincipal), "subject");

            Expression<Func<IClaimsPrincipal, bool>> result = this.ReadNode(reader, subject);
            reader.ReadEndElement();

            return result;
        }

        private static Expression<Func<IClaimsPrincipal, bool>> ReadClaim(XmlReader rdr)
        {
            string claimType = rdr.GetAttribute("claimType");
            string claimValue = rdr.GetAttribute("claimValue");

            Expression<Func<IClaimsPrincipal, bool>> hasClaim = (icp) => HasClaimDelegateHandler(icp, claimType, claimValue);

            rdr.Read();

            return hasClaim;
        }

        private Expression<Func<IClaimsPrincipal, bool>> ReadNode(XmlReader rdr, ParameterExpression subject)
        {
            Expression<Func<IClaimsPrincipal, bool>> policyExpression;

            if (!rdr.IsStartElement())
            {
                throw new InvalidOperationException("Invalid Policy format.");
            }

            switch (rdr.Name)
            {
                case "and":
                    policyExpression = this.ReadAnd(rdr, subject);
                    break;
                case "or":
                    policyExpression = this.ReadOr(rdr, subject);
                    break;
                case "claim":
                    policyExpression = ReadClaim(rdr);
                    break;
                default:
                    policyExpression = DefaultPolicy;
                    break;
            }

            return policyExpression;
        }

        private Expression<Func<IClaimsPrincipal, bool>> ReadOr(XmlReader rdr, ParameterExpression subject)
        {
            rdr.Read();

            BinaryExpression lambda1 = Expression.OrElse(
                    Expression.Invoke(this.ReadNode(rdr, subject), subject),
                    Expression.Invoke(this.ReadNode(rdr, subject), subject));

            rdr.ReadEndElement();

            Expression<Func<IClaimsPrincipal, bool>> lambda2 = Expression.Lambda<Func<IClaimsPrincipal, bool>>(lambda1, subject);
            return lambda2;
        }

        private Expression<Func<IClaimsPrincipal, bool>> ReadAnd(XmlReader rdr, ParameterExpression subject)
        {
            rdr.Read();

            BinaryExpression lambda1 = Expression.AndAlso(
                    Expression.Invoke(this.ReadNode(rdr, subject), subject),
                    Expression.Invoke(this.ReadNode(rdr, subject), subject));

            rdr.ReadEndElement();

            Expression<Func<IClaimsPrincipal, bool>> lambda2 = Expression.Lambda<Func<IClaimsPrincipal, bool>>(lambda1, subject);
            return lambda2;
        }
    }
}
