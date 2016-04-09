<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_Default" ValidateRequest="false" Src="Default.aspx.cs"%>
<%@ OutputCache Location="None" %>

<%@ Register assembly="Microsoft.IdentityModel" namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Book Publisher Service</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Large" 
            Text="Book Hosting Serivce"></asp:Label>
        <br />
        <br />
    
        <asp:Label ID="WelcomeMsg" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="RoleMessage" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <asp:Label ID="BookNameDetails" runat="server" Text="Label"></asp:Label>
        <br />
        <br />
        <asp:TextBox ID="BookContents" runat="server" Height="166px" 
            TextMode="MultiLine" Width="549px"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="ButtonSave" runat="server" Text="Save" 
            ToolTip="Save the changes." onclick="ButtonSave_Click" />
        <br />
        <br />
        <wif:FederatedPassiveSignInStatus ID="SignInStatus1" SignInButtonType="Button" runat="server" />
        <br />
    
    </div>
    </form>
</body>
</html>
