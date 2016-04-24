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

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<FShipping.Models.ClaimMappingListViewModel>" %>
<%@ Import Namespace="Microsoft.IdentityModel.Claims" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
In a production application, new claim mappings should be added to the federation provider to transform incoming 
claims to the claims expected by the application.
<br />
<p>This page is just a mockup, no action occurs when adding new mappings.</p>
    <%=this.Html.ValidationSummary()%>
    <div id="newmapping">
        <div id="newmapping-title">Create a new claim mapping</div>
        <%
            using (this.Html.BeginForm("AddClaimMapping", "Admin"))
            {%>
            <div class="newmapping-claim">
                <span class="newmapping-claim-title">Input</span>
                <div class="newmapping-claim-field">
                    <label for="IncomingClaimType">Incoming claim type</label>
                    <%=this.Html.DropDownList("IncomingClaimType", this.Model.IncomingClaimType.Select(t => new SelectListItem { Text = t, Value = t }))%>
                    <%=this.Html.Tooltip(Resources.TooltipText.ClaimsMappingHint)%>
                </div>
                <div class="newmapping-claim-field">
                    <label for="IncomingValue">Incoming value</label>
                    <%=this.Html.TextBox("IncomingValue", string.Empty)%>
                </div>
            </div>
            <div class="newmapping-claim">
                <span class="newmapping-claim-title">Output</span>
                <div class="newmapping-claim-field">
                    <label for="OutputClaimType">Output claim type</label>
                    <%=this.Html.TextBox("OutputClaimType", ClaimTypes.Role, new { disabled = "disabled", size = "60" })%>
                </div>
                <div class="newmapping-claim-field">
                    <label for="NewRole">Output value</label>
                    <%=this.Html.DropDownList("NewRole", this.Model.OutputRoles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }))%>
                    <input type="submit" value="+" />
                </div>
            </div>
        <%
            }%>
    </div>
    <div style="float:left">
        <table id="claimmappings" cellspacing="0" rules="all">
            <thead>
                <tr>
                    <th>Incoming claim type</th>
                    <th>Incoming value</th>
                    <th>Output role</th>
                    <th>Actions</th>
                </tr>            
            </thead>
            <tbody>
                <%
            foreach (var claimMapping in this.Model.ClaimMappings)
            {%>
                <tr>
                    <td><%=claimMapping.IncomingClaimType%></td>
                    <td><%=claimMapping.IncomingValue%></td>
                    <td><%=claimMapping.OutputRole.Name%></td>
                    <td>
                        <ul>
                            <%
                foreach (var action in claimMapping.OutputRole.Actions)
                {%>
                                <li><%=action%></li>
                            <%
                }%>
                        </ul>
                    </td>
                </tr>
                <%
            }%>
            </tbody>
        </table>
    </div>
</asp:Content>
