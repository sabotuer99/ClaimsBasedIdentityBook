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
using System.Collections;
using System.Configuration;
using System.Threading;
using System.Web.UI;

using Microsoft.IdentityModel.Claims;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        Response.Cache.SetNoStore();

        if ( Page.User == null || Page.User.Identity.IsAuthenticated == false )
        {
            throw new ConfigurationErrorsException( "WSFederationAuthenticationModule must be configured." );
        }

        if ( ClaimsUtil.GetAuthStrengthClaim( Thread.CurrentPrincipal.Identity as IClaimsIdentity ) != AuthenticationMethods.X509 )
        {
            // The user is not authenticated with high assurance.
            throw new UnauthorizedAccessException( "CustomClaimsAuthorizationManager must be configured." );
        }

        // For illustrative purposes this sample application simply shows all the parameters of 
        // claims (i.e. claim types and claim values), which are issued by a security token 
        // service (STS), to its clients. In production code, security implications of echoing 
        // the properties of claims to the clients should be carefully considered. For example, 
        // some of the security considerations are: (i) accepting the only claim types that are 
        // expected by relying party applications; (ii) sanitizing the claim parameters before 
        // using them; and (iii) filtering out claims that contain sensitive personal information). 
        // DO NOT use this sample code 'as is' in production code.

        //Show the claims in a table
        ShowClaims populateTable = new ShowClaims( ClaimSetTable );
    }

    protected void FederatedPassiveSignIn1_SignInError( object sender, Microsoft.IdentityModel.Web.Controls.ErrorEventArgs error )
    {
        throw error.Exception;
    }
}
