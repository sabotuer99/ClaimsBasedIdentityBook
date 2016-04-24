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

<asp:Content ID="Content2" ContentPlaceHolderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceholder" runat="server">
    <h4>Welcome to Fabrikam Shipping!</h4>
    

    <p>To test the application, these tenants that have already been provisioned: <%=this.Html.Tooltip(Resources.TooltipText.PreProvisionedHint)%></p>
    <div id="configured-tenants">
        <ul class="tenants-list">
        <%
            
            var tenants = (this.Model as IEnumerable<FShipping.Data.Organization>);
            foreach (var tenant in tenants)
            {
             %>

               <li>
                <div class="configured-tenant-logo">
                    <a href="<%=this.Url.Action("Index", "Shipment", new { organization = tenant.Name }, "https")%>" class="configured-tenants-links">
                            <img src="<%=this.Url.Content(tenant.LogoPath)%>" alt="<%=tenant.DisplayName %> logo" />
                        </a>
                </div>
                <div class="configured-tenant-description">
                    <%=tenant.DisplayName %> company has already been configured to <b>authenticate using <%=tenant.DisplayName %>'s issuer</b>.
                </div>
            </li>


            <%}%>

        </ul>
    </div>
</asp:Content>

