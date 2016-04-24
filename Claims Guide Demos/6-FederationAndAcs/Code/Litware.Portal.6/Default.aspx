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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Litware.Portal.Default" %>

<asp:Content ID="DefaultHead" ContentPlaceholderID="HeadPlaceholder" runat="Server">
</asp:Content>

<asp:Content ID="DefaultContent" ContentPlaceholderID="ContentPlaceholder" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div id="sitesnavigation">
        The main entry point for Litware sites:
        <ul>
            <li>
                l-EmployeesManager
            </li>
            <li>
                l-Newsletter
            </li>
            <li>
                <a href="https://localhost/a-Order.OrderTracking.6/">a-Order</a>
                <samples:TooltipInformation ID="TooltipInformation4" runat="server" Text="<%$ Resources:TooltipText, aOrderIsFromAdatum %>" />
            </li>
        </ul>
    </div>
</asp:Content>