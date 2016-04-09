<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WFE._Default" %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>WFE page</title>
</head>
<body>
    <p>
        Welcome:
        <asp:LoginName ID="LoginName1" runat="server" />
        , Authentication type:
        <%=HttpContext.Current.User.Identity.AuthenticationType%>
    </p>
    <form runat="server" ID="form1">
    The input value:
    <input name="value" /><br />
    <asp:Button UseSubmitBehavior="true" id="btnSubmit" runat="server" Text="Submit" />
    </form>
    <asp:Label id="lblResult" runat="server" />
</body>
</html>
