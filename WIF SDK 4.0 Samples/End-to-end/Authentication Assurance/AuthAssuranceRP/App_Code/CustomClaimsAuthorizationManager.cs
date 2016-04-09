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
using Microsoft.IdentityModel.Claims;

/// <summary>
/// A CustomClaimsAuthorizationManager that authorizes access to the resources in this RP via claims.
/// </summary>
public class CustomClaimsAuthorizationManager : ClaimsAuthorizationManager
{
    public const string HighValueResourceUrl = "https://localhost/AuthAssuranceRP/HighValueResourcePage.aspx";

    public override bool CheckAccess( AuthorizationContext context )
    {
        string resource = context.Resource[0].Value;

        //
        // To access high value resources, the caller must have an AuthenticationMethod claim of X509.
        //
        if ( resource.Equals( HighValueResourceUrl ) )
        {
            IClaimsPrincipal principal = context.Principal;

            foreach ( IClaimsIdentity identity in principal.Identities )
            {
                string authStrengthClaim = ClaimsUtil.GetAuthStrengthClaim( identity );

                if ( String.Equals( authStrengthClaim, AuthenticationMethods.X509, StringComparison.Ordinal ) )
                {
                    //
                    // Found X509 authentication claim. Return true.
                    //
                    return true;
                }
            }

            //
            // No X509 authentication claim. Return false.
            //
            return false;
        }
        else
        {
            //
            // This is not an access to high value resources. Any set of claims is fine.
            //
            return true;
        }
    }
}
