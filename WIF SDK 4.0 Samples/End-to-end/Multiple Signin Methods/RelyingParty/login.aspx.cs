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

public partial class _Login : System.Web.UI.Page 
{    
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void FederatedPassiveSignIn1_SignInError( object sender, Microsoft.IdentityModel.Web.Controls.ErrorEventArgs error )
    {
        this.Label1.Visible = true;
        this.Label1.ToolTip = ( "Exception thrown is: " + error.Exception.ToString() );
    }

    protected void FederatedPassiveSignIn3_SignInError( object sender, Microsoft.IdentityModel.Web.Controls.ErrorEventArgs error )
    {
        this.Label2.Visible = true;
        this.Label2.ToolTip = ( "Exception thrown is: " + error.Exception.ToString() );
    }
}
