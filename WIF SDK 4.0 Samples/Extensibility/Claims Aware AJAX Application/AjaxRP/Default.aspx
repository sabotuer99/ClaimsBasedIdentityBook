 <%@ Page Language="C#" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="_Default" ValidateRequest="false" %>
<%@ OutputCache Location="None" %>

<%@ Register assembly="Microsoft.IdentityModel" namespace="Microsoft.IdentityModel.Web.Controls" TagPrefix="wif" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AJAX Application Default Page</title>

    <script language="javascript" type="text/javascript">
        var requestObject = false;
        if(window.XMLHttpRequest)
        {
            requestObject = new XMLHttpRequest();
        }
        else if (window.ActiveXObject)
        {
            requestObject = new ActiveXObject("MSXML2.XMLHTTP.5.0");
        }
        
        function displayText(dataSource, divID)
        {
            if(requestObject)
            {
                requestObject.open("GET", dataSource);
                requestObject.onreadystatechange = function()
                {
                    // readyState = complete
                    if(requestObject.readyState == 4)
                    {
                        // Status == 440 is a custom code that was created for the sample indicating 
                        // the client's cookie has expired.  
                        if(requestObject.status == 440)
                        {
                            window.location.href = "https://localhost/AjaxSample_RP/Default.aspx";
                        }
                        else
                        {
                            document.getElementById(divID).innerHTML = requestObject.responseText; 
                        }                        
                    }
                }
                requestObject.send(null);
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        Welcome, <% Response.Write(Server.HtmlEncode(Page.User.Identity.Name));%>.
        <br/><br/>
        <button onclick="displayText('callback.aspx', 'targetSpan'); return false;">
            Call service.
        </button>
        <span id="targetSpan">
        </span>
    </div>
    </form>
</html>
