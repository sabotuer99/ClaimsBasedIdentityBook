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
using System.Security.Principal;
using System.Threading;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Claims;


public partial class _Default : System.Web.UI.Page
{

    public void ShowClaimsIdentityAsIIdentity(IIdentity ci)
    {
        TableCell tc1 = new TableCell();
        tc1.Text = "";
        tc1.BorderWidth = 1;

        TableCell tc2 = new TableCell();
        tc2.Text = "IsAuthenticated:" + ci.IsAuthenticated.ToString();
        tc2.BorderWidth = 1;


        TableCell tc3 = new TableCell();
        tc3.Text = "Name:" + ci.Name;
        tc3.BorderWidth = 1;

        TableRow tr = new TableRow();
        tr.Controls.Add(tc1);
        tr.Controls.Add(tc2);
        tr.Controls.Add(tc3);



        Table table = new Table();
        table.BorderWidth = 1;
        table.Font.Name = "Franklin Gothic Book";
        table.Font.Size = 10;
        table.CellPadding = 3;
        table.CellSpacing = 3;
        table.BorderColor = System.Drawing.Color.Chocolate;
        table.Controls.Add(tr);

        this.Controls.Add(table);
    }

    /// <summary>
    /// Write a claim in a row of the table.
    /// </summary>
    /// <param name="claim">The claim value to write.</param>
    /// <param name="table">The table to write to.</param>
    private void WriteClaim(Claim claim, Table table)
    {
        TableCell tc1 = new TableCell();
        tc1.Text = claim.ClaimType;

        TableCell tc2 = new TableCell();
        tc2.Text = claim.Value;

        TableCell tc3 = new TableCell();
        tc3.Text = claim.ValueType.Substring(claim.ValueType.IndexOf('#') + 1);

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
        if ( !String.IsNullOrEmpty( claim.Issuer ) )
        {
            tc5.Text = claim.Issuer;
        }
        else
        {
            tc5.Text = "Null";
        }

        TableRow tr = new TableRow();

        tr.Controls.Add(tc1);
        tr.Controls.Add(tc2);
        tr.Controls.Add(tc3);
        tr.Controls.Add(tc4);
        tr.Controls.Add(tc5);

        tr.BorderColor = System.Drawing.Color.Chocolate;
        table.Controls.Add(tr);
    }


    /// <summary>
    /// Displays a page that welcomes the user.
    /// </summary>
    /// <param name="claimsIdentity">Identity of the user.</param>
    private void ShowName(IClaimsIdentity claimsIdentity)
    {
        if (claimsIdentity != null)
        {
            WelcomeMessage.Text = "Welcome: " + claimsIdentity.Name;
        }
        else
        {
            WelcomeMessage.Text = "Please sign in. ";
        }
    }

    private void ShowClaimsFromClaimsIdentity(IClaimsIdentity claimsIdentity)
    {
        Table table = new Table();
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

        TableHeaderRow th = new TableHeaderRow();
        th.Controls.Add(thc1);
        th.Controls.Add(thc2);
        th.Controls.Add(thc3);
        th.Controls.Add(thc4);
        th.Controls.Add(thc5);
        th.BorderWidth = 1;
        th.BorderColor = System.Drawing.Color.Chocolate;

        table.Controls.Add(th);
        foreach (Claim claim in claimsIdentity.Claims)
        {
            WriteClaim(claim, table);
        }
        table.BorderWidth = 1;
        table.Font.Name = "Franklin Gothic Book";
        table.Font.Size = 10;
        table.CellPadding = 3;
        table.CellSpacing = 3;
        table.BorderColor = System.Drawing.Color.Chocolate;
        this.Controls.Add(table);
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        IClaimsIdentity ci = (IClaimsIdentity)((IClaimsPrincipal)Thread.CurrentPrincipal).Identity;

        /* For illustrative purposes this sample application simply shows all the parameters of 
         * claims (i.e. claim types and claim values), which are issued by a security token 
         * service (STS), to its clients. In production code, security implications of echoing 
         * the properties of claims to the clients should be carefully considered. For example, 
         * some of the security considerations are: (i) accepting the only claim types that are 
         * expected by relying party applications; (ii) sanitizing the claim parameters before 
         * using them; and (iii) filtering out claims that contain sensitive personal information). 
         * DO NOT use this sample code ‘as is’ in production code.
        */
        ShowName(ci);
        ShowClaimsIdentityAsIIdentity(ci);
        ShowClaimsFromClaimsIdentity(ci);
    }
}
