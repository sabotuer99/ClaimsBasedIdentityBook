<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Windows Identity Foundation - Multi Auth Sample</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Login ID="Login1" runat="server" DisplayRememberMe="False" 
            onauthenticate="Login1_Authenticate" 
            TitleText="Enter User Name and Password to sign-in to the STS">
        </asp:Login>
    </div>
    </form>
</body>
</html>
