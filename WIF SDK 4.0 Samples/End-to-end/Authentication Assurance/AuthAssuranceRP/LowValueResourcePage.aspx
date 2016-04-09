<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LowValueResourcePage.aspx.cs" Inherits="ProtectedPageY" ValidateRequest="false"%>
<%@ OutputCache Location="None" %>

<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Windows Identity Foundation - AuthN Assurance Sample - Account Summary Page</title>
    <style type="text/css">
        .style1
        {
            font-size: large;
            font-weight: bold;
            color: #808000;
        }
        .style2
        {
            font-size: large;
            font-weight: bold;
            color: #008080;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    
        <dfn style="font-style: normal">
    
        <span class="style1">Windows Identity Foundation - Authentication Assurance Sample Site<br />
        <br />
        </span>
    
        <span class="style2">Account Summary Page</span></dfn><br />
        <br />
        Welcome&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label1" runat="server" Text="Label" Width="250px"></asp:Label>
        <br />
        <br />
        Congratulations! You have successfully signed in to access your banking 
        summary statement.<br />
        <br />
        Claims submitted are:<br />
        
        <br />
        <asp:Table runat="server" ID="ClaimSetTable" BorderColor="Blue" BorderStyle="Ridge" BorderWidth="3">
        <asp:TableHeaderRow ID="TableHeaderRow1" runat="server" BorderColor="Blue" BorderStyle="Solid">
        </asp:TableHeaderRow>
        </asp:Table>        
        
        <br />
        
        <br />
        <asp:HyperLink ID="HyperLink2" runat="server" Font-Size= "Large" NavigateUrl="HighAssuranceSignInPage.aspx">Click here to access high value resources</asp:HyperLink>
        <br />
        <br />
        <wif:FederatedPassiveSignInStatus ID="SignInStatus1" SignOutAction="Redirect" SignOutPageUrl="~/Default.aspx" SignInButtonType="Button" runat="server" Font-Size="Large" />
    </form>
</body>
</html>
