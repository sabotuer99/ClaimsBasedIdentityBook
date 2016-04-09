<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Issuing a Username/Password backed Managed Information Card</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span style="font-family: Tahoma"><span style="font-size: 16pt">Windows Identity Foundation Samples <br/>  
        </span>
            <hr />
            <br />
            <span style="font-size: 14pt">
            Issuing a Username/Password backed Managed Information Card<br />
            </span>
            <br />
            <asp:Button ID="btnIssue" runat="server" OnClick="btnIssue_Click" Text="Click here" /></span></div>
    </form>
</body>
</html>
