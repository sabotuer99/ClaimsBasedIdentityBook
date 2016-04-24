//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Litware.SimulatedIssuer
{
    using System;
    using System.IdentityModel.Tokens;
    using Microsoft.IdentityModel.Claims;
    using Microsoft.IdentityModel.Protocols.WSIdentity;
    using Microsoft.IdentityModel.Tokens;
    using Litware.SimulatedIssuer;

    public class CustomUserNameSecurityTokenHandler : UserNameSecurityTokenHandler
    {
        public override bool CanValidateToken
        {
            get { return true; }
        }

        public override ClaimsIdentityCollection ValidateToken(SecurityToken token)
        {
            var userNameToken = token as UserNameSecurityToken;
            if (userNameToken == null)
            {
                throw new ArgumentException(@"The security token is not a valid username security token.", "token");
            }

            string userName = userNameToken.UserName;

            IClaimsIdentity identity = new ClaimsIdentity();
            identity.Claims.Add(new Claim(WSIdentityConstants.ClaimTypes.Name, userName));

            return new ClaimsIdentityCollection { identity };
        }
    }
}