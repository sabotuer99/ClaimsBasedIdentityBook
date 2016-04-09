<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default"  ValidateRequest="false" %>
<%@ OutputCache Location="None" %>

<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Federation Relying Party Sample Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="WelcomeMessage" runat="server" Font-Bold="True" 
            Font-Names="Franklin Gothic Book" Font-Size="10pt"></asp:Label>
    </div>
    <div>
        <wif:FederatedPassiveSignInStatus SignOutAction="FederatedPassiveSignOut" SignOutPageUrl="https://localhost/CorpWebSite/Default.aspx" SignInButtonType="Button" runat="server" />
    </div>
    </form>
</body>
</html>
