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
using System.Threading;
using System.Web.UI;

using Microsoft.IdentityModel.Claims;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load( object sender, EventArgs e )
    {
        Response.Cache.SetNoStore();

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
                // The user is authenticated with low assurance; allow access to 
                // low value resource page.
                Response.Redirect( "LowValueResourcePage.aspx" );
            }
        }
        this.Label1.Text = "Test";
        this.Label1.Visible = false;
    }

    protected void FederatedPassiveSignIn_SignInError( object sender, Microsoft.IdentityModel.Web.Controls.ErrorEventArgs error )
    {
        this.Label1.Visible = true;
        this.Label1.ToolTip = ( "Exception thrown is: " + error.Exception.ToString() );
    }
}
