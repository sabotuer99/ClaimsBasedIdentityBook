<%--
===============================================================================
 Microsoft patterns & practices
 Cliams Identity Guide V2
===============================================================================
 Copyright © Microsoft Corporation.  All rights reserved.
 This code released under the terms of the 
 Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
===============================================================================
--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="SimulatedWindowsAuthentication.aspx.cs" Inherits="Litware.SimulatedIssuer.SimulatedWindowsAuthentication" %>

<asp:Content ID="Content1" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <div id="login">
        Username: <asp:TextBox ID="LoginUserNameTextBox" runat="server" Text="Label" ReadOnly="true"></asp:TextBox><br />
        <br />
        Password: <asp:TextBox ID="PasswordTextBox" runat="server" Text="Label" ReadOnly="true"></asp:TextBox><br />
        <br />
        <asp:Button ID="ContinueButton" runat="server" class="tooltip"
         Text="Login" OnClick="ContinueButtonClick"  />
    </div>
</asp:Content>
