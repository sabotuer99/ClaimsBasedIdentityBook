<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="HighValueResourcePage.aspx.cs" Inherits="_Default" ValidateRequest="false"%>
<%@ OutputCache Location="None" %>

<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Windows Identity Foundation - Authentication Assurance Sample Site</title>
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
       
    <span style="font-family: Tahoma"><span style="font-size: 16pt">
        <span class="style1">Windows Identity Foundation - High Value Resource Access Site<br />
        <br />
        </span>
        <span class="style2">High value assurance page</span><br /></span></span>
    <br />
        Welcome to the high value resources page! With the additional claims you provided,
    you are now authenticated with high assurance and can now access high value 
    resources.<br />
    <br />
        Claims Provided: 
        
        <asp:Table runat="server" ID="ClaimSetTable" BorderColor="Blue" BorderStyle="Ridge">
        <asp:TableHeaderRow ID="TableHeaderRow1" runat="server">
        </asp:TableHeaderRow>
        </asp:Table>        
    
    <br />
    <br />
        <br />        
        <asp:HyperLink ID="HyperLink1" runat="server" Font-Size="Large" NavigateUrl="LowValueResourcePage.aspx">Click here to go to the low value resources summary page</asp:HyperLink>
        <br />
        <br />
        <wif:FederatedPassiveSignInStatus ID="SignInStatus1" SignOutAction="Redirect" SignOutPageUrl="~/Default.aspx" SignInButtonType="Button" runat="server" Font-Size="Large" />
        <br />    
    </form>
</body>
</html>
