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

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<FShipping.Models.MasterPageViewModel>" %>
<%@ Import Namespace="System.Globalization" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">

<%
    if ((bool)this.ViewData["UserFound"])
    {%>
    <%
        this.Html.BeginForm("Add", "Shipment", FormMethod.Post, new { enctype = "multipart/form-data" });%>
        <div id="newshipment">
            <table>
                <tbody>
                    <tr>
                        <td>
                            Sender Name:
                        </td>
                        <td>
                            <%=this.Html.Encode(this.ViewData["SenderName"])%>
                            <%=this.Html.Tooltip(Resources.TooltipText.SenderNameHint)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Sender address:
                        </td>
                        <td>
                            <%=this.Html.Encode(this.ViewData["SenderAddress"])%>
                            <%=this.Html.Tooltip(Resources.TooltipText.SenderAddressHint)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Cost center:
                        </td>
                        <td>
                            <%=this.Html.Encode(this.ViewData["SenderCostCenter"])%>
                            <%=this.Html.Tooltip(Resources.TooltipText.SenderCostCenterHint)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Shipping to address:
                        </td>
                        <td>
                            <%=this.Html.TextBox("ShippingToAddress")%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Pickup date:
                        </td>
                        <td>
                            <%=this.Html.TextBox("PickupDate", this.Html.Encode(DateTime.Now.ToString("yyyy/M/dd", CultureInfo.CurrentUICulture)))%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Service type:
                        </td>
                        <td>
                            <%=this.Html.DropDownList("ServiceType", (IEnumerable<SelectListItem>)this.ViewData["ServiceTypeListItems"])%>
                            <%=this.Html.Tooltip(Resources.TooltipText.SenderServiceTypeHint)%>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Details:
                        </td>
                        <td>
                            <%=this.Html.TextArea("Details", string.Empty, 3, 50, null)%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <input type="submit" value="Add" class="submit" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    <%
        this.Html.EndForm();%>
    <%
    }
    else
    {%>
    You have completed the enrollment process but before using this feature you should setup users' <%=this.Html.ActionLink("subscriptions", "Subscriptions", "Admin")%></div>
<%
    }%>
</asp:Content>
