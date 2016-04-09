<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" ValidateRequest="false" %>
<%@ OutputCache Location="None" %>

<%@ Register assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Windows Identity Foundation - Claims Aware Web App Sample</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
               <wif:FederatedPassiveSignIn ID="FederatedPassiveSignIn1" runat="server" 
                   Issuer="https://localhost/PassiveSTSForClaimsAwareWebApp/Default.aspx" 
                   Realm="https://localhost/WebControlBasedClaimsAwareWebApp/" 
                   Reply="https://localhost/WebControlBasedClaimsAwareWebApp/Login.aspx"
                   VisibleWhenSignedIn="False" 
                   onsigninerror="FederatedPassiveSignIn1_SignInError">
               </wif:FederatedPassiveSignIn>
               <br />
               <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
    </div>
    </form>
</body>
</html>
