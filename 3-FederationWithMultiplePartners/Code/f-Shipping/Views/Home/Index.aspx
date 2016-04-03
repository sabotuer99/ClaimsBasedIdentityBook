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
    <%=this.Html.ActionLink("Enroll your company", "JoinNow")%> in Fabrikam Shipping!
    

    <p>
        To test the application, these tenants that have already been provisioned:
        <%=this.Html.Tooltip(Resources.TooltipText.PreProvisionedHint)%>
    </p>
    
    <div id="configured-tenants">
        <ul class="tenants-list">
            <li>
                <div class="configured-tenant-logo">
                    <a href="<%=this.Url.Action("Index", "Shipment", new { organization = "adatum" }, "https")%>" class="configured-tenants-links">
                            <img src="<%=this.Url.Content("~/Content/images/adatum-logo.png")%>" alt="Adatum logo" />
                        </a>
                </div>
                <div class="configured-tenant-description">
                    Adatum company has already been configured to <b>authenticate using Adatum's issuer</b>.
                </div>
            </li>
            <li>
                <div class="configured-tenant-logo">
                    <a href="<%=this.Url.Action("Index", "Shipment", new { organization = "litware" }, "https")%>" class="configured-tenants-links">
                            <img src="<%=this.Url.Content("~/Content/images/litware-logo.png")%>" alt="Litware logo" />
                        </a>
                </div>
                <div class="configured-tenant-description">
                    Litware company has already been configured to <b>authenticate using Litware's issuer</b>.
                </div>
            </li>
            <li>
                <div class="configured-tenant-logo">
                    <a href="<%=this.Url.Action("Index", "Shipment", new { organization = "contoso" }, "https")%>" class="configured-tenants-links">
                        <img src="<%=this.Url.Content("~/Content/images/contoso-logo.png")%>" alt="Contoso logo" />
                    </a>
                </div>
                <div class="configured-tenant-description">
                    Contoso company has already been configured to <b>authenticate using Fabrikam's issuer</b>.
                </div>
            </li>
        </ul>
    </div>
</asp:Content>

