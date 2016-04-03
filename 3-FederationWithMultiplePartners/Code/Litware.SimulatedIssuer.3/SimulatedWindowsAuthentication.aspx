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
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    
    <div id="login">
        Litware issuer is logging you in using Windows Integrated Authentication as <i><asp:Label ID="LoginUserNameLabel" runat="server" Text="Label"></asp:Label></i>
        <samples:TooltipInformation ID="TooltipInformation1" runat="server" Text="<%$ Resources:TooltipText, SimulatingWindowsAuthHint %>" />
        <br />
        <br />
        <asp:Button ID="ContinueButton" runat="server" class="tooltip"
         Text="Click here to continue..." OnClick="OnContinueButtonClicked"  />
         <samples:TooltipInformation ID="TooltipInformation2" runat="server" Text="<%$ Resources:TooltipText, SameAsAdfsHint %>" />
    </div>
</asp:Content>
