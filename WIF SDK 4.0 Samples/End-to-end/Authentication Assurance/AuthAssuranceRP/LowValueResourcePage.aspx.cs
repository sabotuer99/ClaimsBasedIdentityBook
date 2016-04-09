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
using System.Configuration;
using System.Web.UI;

public partial class ProtectedPageY : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( Page.User == null || Page.User.Identity.IsAuthenticated == false )
        {
            throw new ConfigurationErrorsException( "WSFederationAuthenticationModule must be configured." );
        }

        //
        // If the user is already authenticated then 
        // 1. Show the user's name
        // 2. Show the claims that were submitted 
        //

        Label1.Text = Page.User.Identity.Name;

        //
        // For illustrative purposes this sample application simply shows all the parameters of 
        // claims (i.e. claim types and claim values), which are issued by a security token 
        // service (STS), to its clients. In production code, security implications of echoing 
        // the properties of claims to the clients should be carefully considered. For example, 
        // some of the security considerations are: (i) accepting the only claim types that are 
        // expected by relying party applications; (ii) sanitizing the claim parameters before 
        // using them; and (iii) filtering out claims that contain sensitive personal information). 
        // DO NOT use this sample code 'as is' in production code.
        //

        ShowClaims populateTable = new ShowClaims( ClaimSetTable );
    }
}
