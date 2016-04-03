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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="SimulatedWindowsAuthentication.aspx.cs" Inherits="Adatum.SimulatedIssuer.SimulatedWindowsAuthentication" %>

<asp:Content ID="Content1" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    
    <div id="login">
        Adatum issuer is logging you in using Windows Integrated Authentication. Please select a User to continue:
        <samples:TooltipInformation ID="TooltipInformation1" runat="server" Text="<%$ Resources:TooltipText, SimulatingWindowsAuthHint %>" />
        <div id="UserOptions">
            <asp:RadioButtonList ID="UserList" runat="server">
                <asp:ListItem Text="ADATUM\johndoe (Group: 'Customer Service'; Role:'Order Tracker')" Value="ADATUM\johndoe" Selected="True" />
                <asp:ListItem Text="ADATUM\mary (Group: 'Customer Service'; Roles:'Order Tracker' & 'Order Approver')" Value="ADATUM\mary" />
                <asp:ListItem Text="ADATUM\peter (Groups: 'Order Fulfillments' & 'IT Admins'; Role:'Order Tracker')" Value="ADATUM\peter" />
            </asp:RadioButtonList>
        </div>
        <asp:Button ID="ContinueButton" runat="server" class="tooltip" Text="Continue with login..." OnClick="ContinueButtonClick"  />
         <samples:TooltipInformation ID="TooltipInformation2" runat="server" Text="<%$ Resources:TooltipText, SameAsAdfsHint %>" />
    </div>
</asp:Content>
