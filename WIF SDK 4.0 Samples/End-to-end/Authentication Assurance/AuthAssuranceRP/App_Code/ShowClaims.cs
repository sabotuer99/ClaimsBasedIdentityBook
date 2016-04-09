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
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Microsoft.IdentityModel.Claims;
using System.Threading;

 // For illustrative purposes this class implementation simply shows all the parameters of 
 // claims (i.e. claim types and claim values). In production code, security implications 
 // of echoing the properties of claims to the clients should be carefully considered. For 
 // example, some of the security considerations are: (i) accepting the only claim types 
 // that are expected by application; (ii) sanitizing the claim parameters before using 
 // them; and (iii) filtering out claims that contain sensitive personal information). 
 // DO NOT use this sample code ‘as is’ in production code.

public class ShowClaims
{
    string[] ExpectedClaims = new string[]  {   ClaimTypes.Name, 
                                                ClaimTypes.DateOfBirth,
                                                ClaimTypes.AuthenticationMethod,
                                                ClaimTypes.PostalCode,
                                                ClaimTypes.MobilePhone
                                            };

    public ShowClaims( Table ClaimSetTable )
    {
        //
        // Enumerate the claims and populate the table
        //
        IClaimsIdentity federatedIdentity = Thread.CurrentPrincipal.Identity as IClaimsIdentity;

        if ( null != federatedIdentity )
        {
            AddClaimRow( federatedIdentity, ClaimSetTable );
        }

    }

    /// <summary>
    /// Write a claim out in a row of the table.
    /// </summary>
    /// <param name="claim"> Claim value to write out </param>
    /// <param name="table"> The table to write the row out to </param>
    private void WriteClaim( Claim claim, Table table )
    {
        TableCell tc1 = new TableCell();
        tc1.Text = claim.ClaimType;

        TableCell tc2 = new TableCell();
        tc2.Text = claim.Value;

        TableCell tc3 = new TableCell();
        tc3.Text = claim.ValueType.Substring( claim.ValueType.IndexOf( '#' ) + 1 );

        TableCell tc4 = new TableCell();
        if ( ( claim.Subject ) != null && ( String.IsNullOrEmpty( claim.Subject.Name ) ) == false )
        {
            tc4.Text = claim.Subject.Name;
        }
        else
        {
            tc4.Text = "Null";
        }

        TableCell tc5 = new TableCell();
        if ( ( claim.Issuer ) != null && ( String.IsNullOrEmpty( claim.Issuer ) ) == false )
        {
            tc5.Text = claim.Issuer;
        }
        else
        {
            tc5.Text = "Null";
        }

        TableRow tr = new TableRow();

        tr.Controls.Add( tc1 );
        tr.Controls.Add( tc2 );
        tr.Controls.Add( tc3 );
        tr.Controls.Add( tc4 );
        tr.Controls.Add( tc5 );

        tr.BorderColor = System.Drawing.Color.Chocolate;

        table.Controls.Add( tr );
    }

    /// <summary>
    /// Adds the relevant details of a claim as a row in the table passed in.
    /// </summary>
    /// <param name="claim">The claim to be described.</param>
    /// <param name="table">The table to add a row to.</param>
    private void AddClaimRow( IClaimsIdentity claimsId, Table table )
    {
        TableHeaderCell thc1 = new TableHeaderCell();
        thc1.Text = "Claim Type";

        TableHeaderCell thc2 = new TableHeaderCell();
        thc2.Text = "Claim Value";

        TableHeaderCell thc3 = new TableHeaderCell();
        thc3.Text = "Value Type";

        TableHeaderCell thc4 = new TableHeaderCell();
        thc4.Text = "Subject Name";

        TableHeaderCell thc5 = new TableHeaderCell();
        thc5.Text = "Issuer Name";

        // Add the needed columns to the table
        TableHeaderRow th = new TableHeaderRow();
        th.Controls.Add( thc1 );
        th.Controls.Add( thc2 );
        th.Controls.Add( thc3 );
        th.Controls.Add( thc4 );
        th.Controls.Add( thc5 );
        th.BorderWidth = 1;
        th.BorderColor = System.Drawing.Color.Chocolate;
        table.Controls.Add( th );

        //Populate the table with claims information
        // Show the ones that are expected claims
        StringCollection collClaims = new StringCollection();
        collClaims.AddRange( ExpectedClaims );

        foreach ( Claim claim in claimsId.Claims )
        {
            // Validate whether the claim is what we expect 
            // if not, disregard that to show in the claims table
            if ( collClaims.Contains( claim.ClaimType ) )
            {
                WriteClaim( claim, table );
            }
        }

        // Set table properties for easy reading
        table.BorderWidth = 1;
        table.Font.Name = "Franklin Gothic Book";
        table.Font.Size = 10;
        table.CellPadding = 3;
        table.CellSpacing = 3;
        table.BorderColor = System.Drawing.Color.Chocolate;
    }

}
