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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AOrder.Default" ValidateRequest="false" %>

<%@ Register TagPrefix="samples" TagName="OrdersGrid" Src="controls/OrdersGrid.ascx" %>

<asp:Content ID="DefaultHead" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="DefaultContent" ContentPlaceholderID="ContentPlaceholder" runat="server">

    <samples:OrdersGrid ID="OrdersGrid" runat="server"></samples:OrdersGrid>
    
</asp:Content>
