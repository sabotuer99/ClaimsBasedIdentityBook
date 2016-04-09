<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PassiveSTS._Default" %>
<%@ OutputCache Location="None" %>

<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>
<%@ Import Namespace="System.Drawing" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <span style="font-family: Tahoma"><span style="font-size: 16pt">Windows Identity Foundation
            Samples<br />
        </span>
            <br />
            <span style="font-size: 14pt">This page will sign the user in if sent a passive sign-in
                request.
                <br />
                <br />
                It will sign the user out if sent a passive sign-out request.
                <br />
            </span>
            <br />
            <%
                if ( HttpContext.Current.User == null || !HttpContext.Current.User.Identity.IsAuthenticated )
                {
                    Label1.Text = "Caller not authenticated.\n" +
                        "Have you enabled Windows authentication and disabled Anonymous authentication for the PassiveSTS web application in the IIS management console?";
                    Label1.ForeColor = Color.Red;
                }
                else
                {
                    Label1.Text = Server.HtmlEncode( HttpContext.Current.User.Identity.Name );
                }
            %>
            Caller's name:
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </span>
    </div>
    <asp:LoginName ID="LoginName1" runat="server" />
    </form>
</body>
</html>
