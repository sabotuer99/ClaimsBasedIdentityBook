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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Web.Controls;

public partial class _Default : System.Web.UI.Page
{
    protected HtmlForm form1;
    protected Label Label1;
    protected Label WelcomeMsg;
    protected Label RoleMessage;
    protected Label BookNameDetails;
    protected TextBox BookContents;
    protected Button ButtonSave;
    protected FederatedPassiveSignInStatus SignInStatus1;

    /// <summary>
    /// Gets a boolean on whether the current user is Authenticated.
    /// </summary>
    private bool IsAuthenticated
    {
        get
        {
            return ( Page.User != null ) && ( Page.User.Identity != null ) && ( Page.User.Identity.IsAuthenticated );
        }
    }

    /// <summary>
    /// ASP.NET event fired during Page Load. 
    /// Checks if the user is Authenticated. If authenticated user, then 
    /// prints a Welcome message and also displays the Role of the user
    /// as recognized by this service. Presents the contents of the book
    /// that the user is Authenticated to edit or review. 
    /// </summary>
    /// <param name="sender">Object that fires this event.</param>
    /// <param name="e">EventArgs for this event.</param>
    protected void Page_Load( object sender, EventArgs e )
    {
        if ( IsAuthenticated && !this.IsPostBack )
        {
            this.WelcomeMsg.Text = "Welcome, " + Page.User.Identity.Name;
            this.RoleMessage.Text = "You are logged in with Role: " + GetRole( Thread.CurrentPrincipal as IClaimsPrincipal ) + ".";
            string bookName = GetBookName( Thread.CurrentPrincipal as IClaimsPrincipal );
            string bookNameDetails;

            ClaimsPrincipal cp = Thread.CurrentPrincipal as ClaimsPrincipal;

            if ( Thread.CurrentPrincipal.IsInRole( "Editor" ) )
            {
                bookNameDetails = String.Format( "You are authorized to edit the contents of book '{0}'. Below is the contents of the book.", bookName );
                // Set Textbox for editing and display the Save button.
                this.ButtonSave.Enabled = true;
                this.ButtonSave.Visible = true;
                this.BookContents.ReadOnly = false;
                this.BookContents.Visible = true;
            }
            else if ( Thread.CurrentPrincipal.IsInRole( "Reviewer" ) )
            {
                bookNameDetails = String.Format( "You are authorized to view the contents of book '{0}'. Below is the contents of the book.", bookName );
                // Set Textbox as ReadOnly and hide the Save Button.
                this.ButtonSave.Enabled = false;
                this.ButtonSave.Visible = false;
                this.BookContents.ReadOnly = true;
                this.BookContents.Visible = true;
            }
            else
            {
                throw new UnauthorizedAccessException( "Unable to find a valid Role Claim for the user." );
            }

            this.BookNameDetails.Text = Server.HtmlEncode( bookNameDetails );
            this.BookContents.Text = Server.HtmlEncode( GetBookContents() );
        }
    }

    /// <summary>
    /// Returns the Role of the current user.
    /// </summary>
    /// <param name="principal">Principal of the current user.</param>
    /// <returns>Role of the current user.</returns>
    /// <exception cref="ArgumentNullException">Input parameter 'principal' is null.</exception>
    /// <exception cref="UnauthorizedAccessException">The user does not have a valid Role Claim.</exception>
    string GetRole( IClaimsPrincipal principal )
    {
        if ( principal == null )
        {
            throw new ArgumentNullException( "principal" );
        }

        foreach ( IClaimsIdentity claimsIdentity in principal.Identities )
        {
            // Look for a claim with the RoleClaimType.
            IEnumerable<Claim> claimCollection = ( from c in claimsIdentity.Claims
                                                   where c.ClaimType == ServiceClaimTypes.RoleClaimType
                                                   select c );
            if ( claimCollection.Count<Claim>() > 0 )
            {
                return claimCollection.First<Claim>().Value;
            }
        }

        throw new UnauthorizedAccessException( "Unable to find a valid Role Claim for the user." );
    }

    /// <summary>
    /// Returns the name of the book the user is authorized for.
    /// </summary>
    /// <param name="principal">Principal of the current user.</param>
    /// <returns>Book Name of String.Empty if no such claim is present.</returns>
    /// <exception cref="ArgumentNullException">Input parameter 'principal' is null.</exception>
    string GetBookName( IClaimsPrincipal principal )
    {
        if ( principal == null )
        {
            throw new ArgumentNullException( "principal" );
        }

        IClaimsIdentity claimsIdentity = (IClaimsIdentity)principal.Identity;
        // The STS has Windows Authentication enabled. With this mode of authentication
        // the user will have a PrimarySid in their Identity.
        IEnumerable<Claim> claimCollection = ( from c in claimsIdentity.Claims
                                               where c.ClaimType == ServiceClaimTypes.BookNameClaimType
                                               select c );
        if ( claimCollection.Count<Claim>() > 0 )
        {
            return claimCollection.First<Claim>().Value;
        }

        return String.Empty;
    }

    /// <summary>
    /// Returns the contents of the book the user is authorized for.
    /// </summary>
    /// <returns>The contents of the book.</returns>
    string GetBookContents()
    {
        string bookName = GetBookName( Thread.CurrentPrincipal as IClaimsPrincipal );
        if ( String.IsNullOrEmpty( bookName ) )
        {
            return "The Publisher has not specified a book name that you are authorized to view.";
        }

        string filePath = Path.Combine( Path.GetDirectoryName( Request.PhysicalPath ), bookName );

        if ( !File.Exists( filePath ) )
        {
            return String.Format( "Publisher has authorized you for book {0}. Which is not Found.", bookName );
        }

        using ( FileStream fs = new FileStream( filePath, FileMode.Open, FileAccess.Read ) )
        {
            byte[] fileRead = new byte[fs.Length];
            fs.Read( fileRead, 0, (int)fs.Length );
            fs.Close();

            return Encoding.UTF8.GetString( fileRead );
        } 
    }

    /// <summary>
    /// Event triggered when the user hits the 'Save' button the form. The
    /// method saves the contents of the Book back to the file.
    /// </summary>
    /// <param name="sender">The object that fires the event.</param>
    /// <param name="e">EventArgs for the event.</param>
    /// <exception cref="InvalidOperationException">Book name is not found or the book is not available
    /// at the service.</exception>
    protected void ButtonSave_Click( object sender, EventArgs e )
    {
        if ( !Thread.CurrentPrincipal.IsInRole( "Editor" ) )
        {
            // Don't allow anyone other than an Editor to change the book contents.
            return;
        }

        string bookName = GetBookName( Thread.CurrentPrincipal as IClaimsPrincipal );
        if ( String.IsNullOrEmpty( bookName ) )
        {
            throw new InvalidOperationException( "No Book name was found." );
        }

        string filePath = Path.Combine( Path.GetDirectoryName( Request.PhysicalPath ), bookName );

        if ( !File.Exists( filePath ) )
        {
            throw new InvalidOperationException( String.Format( "Publisher has authorized you for book {0}. Which is not Found.", bookName ) );
        }

        using ( FileStream fs = new FileStream( filePath, FileMode.Open, FileAccess.Write ) )
        {
            fs.SetLength( 0 );
            byte[] bookContents = Encoding.UTF8.GetBytes( this.BookContents.Text );
            fs.Write( bookContents, 0, bookContents.Length );
            fs.Flush();
            fs.Close();
        }
    }

}
