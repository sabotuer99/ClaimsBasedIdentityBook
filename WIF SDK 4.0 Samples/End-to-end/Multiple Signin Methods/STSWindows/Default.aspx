<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="PassiveFlowSTS._Default" ValidateRequest="false" %>
<%@ OutputCache Location="None" %>

<%@ Register Assembly="Microsoft.IdentityModel, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Windows Identity Foundation - Multi Auth Sample</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Please enable Windows Authentication and disable Anonymous Authentication for the MultiAuthSTS_Windows virtual directory.
    </div>
    </form>
</body>
</html>
