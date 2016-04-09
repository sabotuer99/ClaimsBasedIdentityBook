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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected HtmlForm form1;
    protected Label Label2;
    protected Label Label1;
    protected System.Web.UI.WebControls.Login Login1;

    protected void Page_Load( object sender, EventArgs e )
    {

    }

    /// <summary>
    /// Event fired when the user clicks the Login button.
    /// Checks the user name and password for a known set of
    /// users.
    /// </summary>
    /// <param name="sender">The object that fires the event.</param>
    /// <param name="e">EventArgs associated with the event.</param>
    protected void OnAuthenticate( object sender, AuthenticateEventArgs e )
    {
        // We recognize only two users - Editor and Reviewer.
        if ( StringComparer.OrdinalIgnoreCase.Equals( Login1.UserName, "Bob" ) ||
            ( StringComparer.OrdinalIgnoreCase.Equals( Login1.UserName, "Joe" ) ) )
        {
            if ( StringComparer.Ordinal.Equals( Login1.UserName, Login1.Password ) )
            {
                e.Authenticated = true;
            }
        }
    }
}
