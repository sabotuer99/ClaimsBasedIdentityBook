<%--
===============================================================================
 Microsoft patterns & practices
 Cliams Identity Guide V2
===============================================================================
 Copyright © Microsoft Corporation.  All rights reserved.
 This code released under the terms of the 
 Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
===============================================================================
--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Fabrikam.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Samples.Web.ClaimsUtilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceholder" runat="server">
    <script language="javascript" type="text/javascript">

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
    <h4>You have completed the enrollment process in Fabrikam Shipping! <%=this.Html.Tooltip(Resources.TooltipText.CompletedHint)%></h4>
    <div id="issuerOptionTabs">
       
       <h1>Thanks!</h1>

    </div>
</asp:Content>
