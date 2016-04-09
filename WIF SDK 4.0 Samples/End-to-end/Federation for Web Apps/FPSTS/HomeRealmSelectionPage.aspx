<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HomeRealmSelectionPage.aspx.cs" Inherits="HomeRealmSelectionPage" %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Home Realm</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    Select the identity provider that you would want to logon to:
    <asp:DropDownList ID="RealmSelection" runat="server" AutoPostBack="True">
    </asp:DropDownList>
    </form>
</body>
</html>
