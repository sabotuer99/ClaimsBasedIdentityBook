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

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="Microsoft.IdentityModel.Claims" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
<p>This page is just a mockup, no action occurs when adding new subscriptions. This feature is only required by companies using social identity providers. Companies using their own identity provider subscriptions are indectly manage by the claim mappings.</p>
<br />
        <div style="float:none">
        <table id="subscriptions">
            <thead>
                <tr>
                    <th>Account name</th>
                    <th>Given name</th>
                    <th>Sur name</th>
                    <th>Actions</th>
                </tr>            
            </thead>
            <tbody>
                <tr>
                    <td colspan="4" style="text-align:center">No subscriptions</td>
                </tr>
            </tbody>
        </table>
    </div>

   <div id="newsubscription">
            <h2>New subscription</h2>
            <table>
                <tbody>
                    <tr>
                        <td>
                            Account name:
                        </td>
                        <td>
                            <%=this.Html.TextBox("accountName", string.Empty, new { size = "60" })%>                            
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Given name:
                        </td>
                        <td>
                            <%=this.Html.TextBox("givenName", string.Empty, new { size = "60" })%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Sur name:
                        </td>
                        <td>
                            <%=this.Html.TextBox("surName", string.Empty, new { size = "60" })%>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            Street address:
                        </td>
                        <td>
                            <%=this.Html.TextBox("streetAddress", string.Empty, new { size = "60" })%>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            State or province:
                        </td>
                        <td>
                            <%=this.Html.TextBox("stateOrProvince", string.Empty, new { size = "60" })%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Country:
                        </td>
                        <td>
                            <%=this.Html.TextBox("country", string.Empty, new { size = "60" })%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right" style="padding-right:30px">
                            <input type="button" onclick="alert('This page is just a mockup, new suscriptions cannot be added to the f-Shipping sample application.');"
                            value="Add" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
</asp:Content>
