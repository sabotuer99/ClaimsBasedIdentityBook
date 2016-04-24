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

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceholder" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <div id="issuerSelection">
        <span class="issuerSelection-title">
            Home realm discovery  
            <samples:TooltipInformation ID="TooltipInformation1" runat="server" Text="<%$ Resources:TooltipText, HomeRealmDiscoveryHint %>" />
        </span> 

        <asp:RadioButton ID="ParterRadioButton" runat="server" 
            Text="Sign in with your organization account:"
            GroupName="issuer" AutoPostBack="True" 
            oncheckedchanged="OnParterRadioButtonCheckedChanged"
            CssClass="box-title" />
        
        <div class="issuerSelectionBox">
            <asp:DropDownList ID="TrustedIssuersDropDownList" runat="server" CssClass="box" Enabled="false">
                <asp:ListItem Text="Litware" Value="http://litware/trust" />
                <asp:ListItem Text="Adatum" Value="http://adatum/trust" />
            </asp:DropDownList>
        </div>

        <asp:RadioButton ID="SocialRadioButton" runat="server" 
            Text="Sign in with your favorite social network account:" 
            GroupName="issuer" AutoPostBack="True" 
            oncheckedchanged="OnSocialRadioButtonCheckedChanged"
            CssClass="box-title" />
        
        <div class="issuerSelectionBox">
            <asp:DropDownList ID="SocialIssuersDropDownList" runat="server" CssClass="box" Enabled="false">
                <asp:ListItem Text="Windows Live" Value="live.com" />
                <asp:ListItem Text="Google" Value="gmail.com" />
                <asp:ListItem Text="Facebook" Value="facebook.com" />
            </asp:DropDownList><samples:TooltipInformation ID="TooltipInformation2" runat="server" Text="<%$ Resources:TooltipText, SocialIdentityHint %>" />
        </div>

        <asp:RadioButtonList ID="IssuerRadioButtonList" runat="server" AutoPostBack="true" Visible="false">
            <asp:ListItem Value="adatumAsIdentityProvider" Text=" Login with an Adatum account (Adatum issuer will act as an identity provider)" />
            <asp:ListItem Value="adatumAsFederationProvider" Text="Enter you email address to log in (Adatum issuer will act as a federation provider):" />
        </asp:RadioButtonList>

        <div id="issuerSelection-signinbutton">
            <asp:Button ID="SignInButton" runat="server" Text="Sign In" OnClick="OnSignInButtonClicked" />
        </div>
    </div>
</asp:Content>
