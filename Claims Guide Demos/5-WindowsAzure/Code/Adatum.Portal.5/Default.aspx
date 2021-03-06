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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Adatum.Portal.Default" %>

<asp:Content ID="DefaultHead" ContentPlaceholderID="HeadPlaceholder" runat="Server">
</asp:Content>

<asp:Content ID="DefaultContent" ContentPlaceholderID="ContentPlaceholder" runat="Server">
    <asp:ScriptManager ID="DefaultScriptManager" runat="server" />

    <div id="sitesnavigation">
        The main entry point for Adatum sites:
        <ul>
            <li>
                <a href="https://127.0.0.1:444/">a-Expense (claims aware)</a>
                <samples:TooltipInformation ID="TooltipInformation2" runat="server" Text="<%$ Resources:TooltipText, aExpenseClaimsAwareDescription %>" />
            </li>
            <li>
                <a href="https://localhost/a-Order.ClaimsAware.5/">a-Order (claims aware)</a>
                <samples:TooltipInformation ID="TooltipInformation4" runat="server" Text="<%$ Resources:TooltipText, aOrderClaimsAwareDescription %>" />
            </li>
        </ul>
    </div>
</asp:Content>