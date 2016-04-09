<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Login" Src="Login.aspx.cs"%>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Book Publisher Security Token Service - Sign-In</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="Large" 
            Text="Book Publisher Security Token Service"></asp:Label>
        <br />
        <br />
    
        <asp:Label ID="Label1" runat="server" Text="Enter your Username/Password" 
            Font-Bold="True"></asp:Label>
    
    </div>
    <asp:Login ID="Login1" runat="server" onauthenticate="OnAuthenticate" 
        BorderStyle="Solid" Font-Bold="True">
    </asp:Login>
    </form>
</body>
</html>
