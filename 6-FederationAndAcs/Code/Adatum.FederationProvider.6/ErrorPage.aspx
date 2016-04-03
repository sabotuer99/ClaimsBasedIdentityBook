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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="Adatum.FederationProvider.ErrorPage" ValidateRequest="false" %>

<asp:Content ID="FederationHead" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="FederationContent" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div>
        Identity Provider: <asp:Label ID="lblIdntityProvider" runat="server"></asp:Label><br />
        <asp:Label ID="lblErrorMessage" runat="server"></asp:Label> 
        <samples:TooltipInformation ID="TooltipInformation1" runat="server" Text="<%$ Resources:TooltipText, ErrorInformationHint %>" />

    </div>
    <div>
        <a href="https://localhost/a-Order.OrderTracking.6/">Return to a-Order and try again.</a>
    </div>
</asp:Content>



