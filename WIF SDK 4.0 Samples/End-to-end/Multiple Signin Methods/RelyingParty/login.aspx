<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="_Login" ValidateRequest="false" %>
<%@ OutputCache Location="None" %>

<%@ Register assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Windows Identity Foundation - Multi Auth Sample</title>
</head>
<body>
    <form id="form1" runat="server">
    <div style="font-family: Verdana; font-size: 10pt">
        Select the Authentication Type you wish to use at this site<br />
        <br />
        <wif:FederatedPassiveSignIn ID="FederatedPassiveSignIn1" runat="server" 
            Issuer="https://localhost/MultiAuthSTS_Windows/Default.aspx" 
            Realm="https://localhost/MultiAuthRP" 
            TitleText="Windows Authentication based STS" DisplayRememberMe="False" 
            onsigninerror="FederatedPassiveSignIn1_SignInError">
        </wif:FederatedPassiveSignIn>
        <br />
        <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
        <br />
        <br />
        <wif:FederatedPassiveSignIn ID="FederatedPassiveSignIn3" runat="server" 
            Issuer="https://localhost/MultiAuthSTS_Forms/Default.aspx" 
            Realm="https://localhost/MultiAuthRP" 
            TitleText="Forms Authentication based STS" DisplayRememberMe="False" 
            onsigninerror="FederatedPassiveSignIn3_SignInError">
        </wif:FederatedPassiveSignIn>
        <br />
        <asp:Label ID="Label2" runat="server" Text="Label" Visible="false"></asp:Label>

    </div>
    </form>
</body>
</html>
