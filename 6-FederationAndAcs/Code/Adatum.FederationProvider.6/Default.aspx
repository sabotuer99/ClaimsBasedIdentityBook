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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Adatum.FederationProvider.Default" %>

<asp:Content ID="DeafultHead" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="DefaultContent" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <asp:Label ID="ActionExplanationLabel" runat="server" />
    <br/>
    <br/>
    <asp:Label ID="RelyingPartyLabel" runat="server" Text="Relying Parties" Visible="false"/>
    <asp:Panel ID="RelyingPartySignOutLinks" runat="server">
    </asp:Panel>
    
    <asp:Label ID="IssuerLabel" runat="server" Text="Issuers" Visible="false"/>
    <asp:Panel ID="IssuerSignOutLinks" runat="server">
    </asp:Panel>
</asp:Content>
