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
using System.Security.Principal;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if ( Page.User.Identity.IsAuthenticated )
        {
            Server.Transfer( "Default.aspx" );
        }
    }

    protected void FederatedPassiveSignIn1_SignInError( object sender, Microsoft.IdentityModel.Web.Controls.ErrorEventArgs error )
    {
        this.Label1.Visible = true;
        this.Label1.Text = ( "Exception thrown is: " + error.Exception.ToString() );
    }
}
