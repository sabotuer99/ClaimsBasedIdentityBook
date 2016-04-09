<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" ValidateRequest="false"%>
<%@ OutputCache Location="None" %>

<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>
    
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Windows Identity Foundation - Auth Assurance Sample - Intro Page</title>
    <style type="text/css">
        .style1
        {
            color: #808000;
        }
        .style2
        {
            color: #800000;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>    
    <span style="font-family: Tahoma"><span style="font-size: 16pt">
        <span class="style1">Windows Identity Foundation - Authentication Assurance Sample Site</span><br /></span></span>
        <br />
    <span style="font-family: Tahoma"><span style="font-size: 13pt">
        <span class="style2">Introduction page</span><br class="style2" />
        <br />
        </span></span>
        
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/LowValueResourcePage.aspx" >
            Access low value resources
        </asp:HyperLink>
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/HighAssuranceSignInPage.aspx" >
            Access high value resources
        </asp:HyperLink>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
