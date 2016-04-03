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

<%@ Page Language="C#" MasterPageFile="~/Styling/Site.Master" AutoEventWireup="true"
    CodeBehind="HomeRealmDiscovery.aspx.cs" Inherits="Adatum.FederationProvider.HomeRealmDiscovery" %>

<asp:Content ID="HomeRealmDiscoveryHead" ContentPlaceHolderID="HeadPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="HomeRealmDiscoveryContent" ContentPlaceHolderID="ContentPlaceholder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div id="issuerSelection">
        <span class="issuerSelection-title">Home realm discovery</span>        
        <asp:Label ID="PartnerLabel" runat="server" Text="Select your organization to log in with your organizational account(Adatum acts as an FP):"
            GroupName="issuer" AutoPostBack="True" />
        <p>
            <div class="issuerSelectionBox">
                <asp:DropDownList ID="TrustedIssuersDropDownList" runat="server" CssClass="box">
                    <asp:ListItem Text="Litware" Value="http://litware/trust" />
                    <asp:ListItem Text="Adatum" Value="http://adatum/trust" />
                </asp:DropDownList>
                <samples:TooltipInformation ID="TooltipInformation1" runat="server" Text="<%$ Resources:TooltipText, AdatumAsFederationProvider %>" />
            </div>  
            
      
        </p>
        <div id="issuerSelection-signinbutton">
            <asp:Button ID="SignInButton" runat="server" Text="Sign In" OnClick="OnSignInButtonClicked" />
        </div>
    </div>
</asp:Content>
