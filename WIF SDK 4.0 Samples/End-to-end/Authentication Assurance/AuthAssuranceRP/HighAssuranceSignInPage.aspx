<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HighAssuranceSignInPage.aspx.cs" Inherits="HighAssuranceSignInPage" ValidateRequest="false" %>
<%@ OutputCache Location="None" %>

<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Windows Identity Foundation - Auth Assurance Sample - High value sign-in page</title>
    <style type="text/css">

        .style1
        {
            color: #808000;
        }
        .style2
        {
            color: #800080;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-size: large">
    <span style="font-family: Tahoma"><span style="font-size: 16pt">
        <span class="style1">Windows Identity Foundation - High Value Resources Sample Site<br />
        <br />
        </span>
        </span>
        <span class="style2">Sign-in page for high value resources</span></span><br />
        <br />
        <br />
        <span style="font-family: Tahoma"><span style="font-size: 11pt">
        In order to allow access to high value resources, our policy mandates that 
        customers obtain additional authentication information. Please provide sign in information as required below. You will be required to login with an information card that requests your smart card or a different certificate in the next screen.
        </span>
        <br />
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/HighValueResourcePage.aspx" >
            High Assurance Sign-In
        </asp:HyperLink>
        <br />
        <br />    
        <br />
            
        <asp:HyperLink ID="HyperLink1" runat="server" Font-Size="Large" NavigateUrl="LowValueResourcePage.aspx">Click here to go to account summary page</asp:HyperLink>
        <br />
        <br />
        <br />
        <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
        </div>
    </form>
