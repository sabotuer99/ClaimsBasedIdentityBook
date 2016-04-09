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
using System.Threading;
using System.Web.UI;

using Microsoft.IdentityModel.Claims;

public partial class HighAssuranceSignInPage : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        //
        // If the user is authenticated with a high assurance log in,
        // allow him to access the high value resource page.
        //
        // If the user is not authenticated, ask the user to log in through 
        // the high assurance sign-in page.
        //
        if ( Page.User != null && Page.User.Identity.IsAuthenticated == true )
        {
            if ( ClaimsUtil.GetAuthStrengthClaim( Thread.CurrentPrincipal.Identity as IClaimsIdentity ) == AuthenticationMethods.X509 )
            {
                // The user is authenticated with high assurance; allow access to 
                // high value resource page.
                Response.Redirect( "HighValueResourcePage.aspx" );
            }
            else
            {
                // The user is authenticated with low assurance. Ask the user to do a high assurance sign-in.

                // Enable the account summary page link so that the user opt to
                // go back to that page.
                HyperLink1.Visible = true;
            }
        }
        else
        {
            // The user is not authenticated. He needs to sign in with the high assurance authentication method.
            // Hide the link back to account summary page because the user is not yet authenticated.
            HyperLink1.Visible = false;
        }
    }

    protected void FederatedPassiveSignIn1_SignInError( object sender, Microsoft.IdentityModel.Web.Controls.ErrorEventArgs error )
    {
        // Print the exception thrown.
        this.Label1.Visible = true;
        this.Label1.Text = "Exception thrown is: " + error.Exception.ToString() + "\n\nInner exception is:" + error.Exception.InnerException.ToString();
    }
}
