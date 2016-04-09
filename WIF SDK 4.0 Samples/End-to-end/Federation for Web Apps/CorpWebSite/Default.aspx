<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sample Corp Website Home Page</title>
    <style type="text/css">
        .style1
        {
            font-family: Arial, Helvetica, sans-serif;
            font-size: large;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <p>
        <span class="style1">Windows Identity Foundation - Federation For Web Apps Sample</span><br />
    </p>
    <form id="form1" runat="server">
    <div>
        Click <a href="https://localhost/PassiveRP/default.aspx?whr=https://localhost/PassiveIPSTS1/default.aspx"> here </a> to 
        access a resource that is hosted in partner site and requires single identity 
        provider (IPSTS-1 will be used).
        <br />
        <br />
        Click <a href="https://localhost/PassiveRP/default.aspx"> here </a> to 
        access a resource that is hosted in partner site and requires selection of 
        identity providers (Drop down list with IPSTS-1 and IPSTS-2 will be shown for 
        selection).    
    </div>
    </form>
</body>
</html>
