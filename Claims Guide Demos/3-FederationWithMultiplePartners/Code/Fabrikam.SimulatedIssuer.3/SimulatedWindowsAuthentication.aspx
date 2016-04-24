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

<%@ Page Title="" Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true" CodeBehind="SimulatedWindowsAuthentication.aspx.cs" Inherits="Fabrikam.SimulatedIssuer.SimulatedWindowsAuthentication" %>

<asp:Content ID="SimulatedWindowsAuthHead" ContentPlaceholderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="SimulatedWindowsAuthContent" ContentPlaceholderID="ContentPlaceholder" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" />
    
    <div id="login">
        <table cellpadding="10" border="0">
			<tr>
				<td class="logintitle" align="center" colspan="2">Simple issuer login</td>
			</tr>
			<tr>
				<td align="right">User Name:</td>
				<td>
                    <asp:TextBox ID="UserName" runat="server" CssClass="box" Text="bill@contoso.com" ReadOnly="true"></asp:TextBox> 
				</td>
			</tr>
			<tr>
				<td align="right">Password:</td>
				<td>
                    <asp:TextBox ID="Password" runat="server" CssClass="box" Text="12345678" ReadOnly="true"></asp:TextBox> 				    
				</td>
			</tr>
			<tr>
				<td align="right" colspan="2">
				    <asp:Button ID="ContinueButton" runat="server" class="tooltip" Text="Click here to continue..." OnClick="OnContinueButtonClicked"  />
                    <samples:TooltipInformation ID="TooltipInformation2" runat="server" Text="<%$ Resources:TooltipText, SameAsAdfsHint %>" />
				</td>
			</tr>
		</table>
    </div>
</asp:Content>
