<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="PassiveFlowIPSTS2._Default" ValidateRequest="false" %>
<%@ OutputCache Location="None" %>

<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Windows Integrated Authentication Passive Sign In</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        At the Identity Provider STS-2. Federation Provider STS has redirected the user to authenticate here.
    </div>
    </form>
</body>
</html>
