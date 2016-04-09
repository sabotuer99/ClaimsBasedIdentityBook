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
using System.Linq;
using Microsoft.IdentityModel.Claims;

/// <summary>
/// A utility class which helps to retrieve the AuthenticationMethod claim
/// </summary>
public class ClaimsUtil
{
    public static string GetAuthStrengthClaim( IClaimsIdentity claimsIdentity )
    {
        // Search for an AuthenticationMethod Claim.
        IEnumerable<Claim> claimCollection = ( from c in claimsIdentity.Claims
                                               where c.ClaimType == Microsoft.IdentityModel.Claims.ClaimTypes.AuthenticationMethod
                                               select c );
        if ( claimCollection.Count<Claim>() > 0 )
        {
            return claimCollection.First<Claim>().Value;
        }

        return String.Empty;
    }
}

