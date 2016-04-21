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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="AExpense.Login" %>

<asp:Content ID="LoginHead" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="LoginContent" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <div id="login">
        <samples:TooltipInformation ID="TooltipInformation2" runat="server" Text="<%$ Resources:TooltipText, AvailableUsersForLoginHint %>" />
        <asp:Login ID="Login1" runat="server" OnAuthenticate="Login1OnAuthenticate" TitleText="a-Expense login">
            <CheckBoxStyle CssClass="invisible" />
            <TextBoxStyle CssClass="logintextboxes" />
            <LoginButtonStyle CssClass="loginbutton" />
            <TitleTextStyle CssClass="logintitle" />
        </asp:Login>
    </div>
</asp:Content>
