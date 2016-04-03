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

<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<FShipping.Models.ShipmentListViewModel>" %>
<%@ Import Namespace="System.Globalization" %>
<%@ Import Namespace="FShipping.Data" %>
<%@ Import Namespace="Samples.Web.ClaimsUtilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadPlaceholder" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
    <center>
        <%
            if (this.Model.Shipments.Count() > 0)
            {%>
        <div id="shipmentlist">
            <table style="border-collapse:collapse;" border="1" cellspacing="0" rules="all">
                <thead>
                    <tr>
                        <th scope="col" width="120">Shipping to</th>
                        <th scope="col">Pickup</th>
                        <th scope="col" width="170">Details</th>
                        <th scope="col">Fee</th>
                        <th scope="col">ETA</th>
                        <th scope="col">Status</th>
                        <%
                if (this.User.IsInRole(Fabrikam.Roles.ShipmentManager))
                {%>
                            <th scope="col">Sender name</th>
                            <th scope="col" width="85px">Actions</th>
                        <%
                }%>
                    </tr>
                </thead>
            
                <tbody>
            <%
                foreach (var shipment in this.Model.Shipments)
                {%>
                <tr>
                    <td><%=this.Html.Encode(shipment.ToAddress)%></td>
                    <td><%=this.Html.Encode(shipment.PickupDate.ToShortDateString())%></td>
                    <td><%=this.Html.Encode(shipment.Details)%></td>
                    <td><%=this.Html.Encode(string.Format(CultureInfo.CurrentUICulture, "${0}", Math.Round(shipment.Fee, 2)))%></td>
                    <td><%=this.Html.Encode(shipment.EstimatedDateOfArrival.ToShortDateString())%></td>
                    <td><%=this.Html.Encode(Enum.GetName(typeof(ShipmentStatus), shipment.Status))%></td>
                    <%
                    if (this.User.IsInRole(Fabrikam.Roles.ShipmentManager))
                    {%>
                        <td><%=this.Html.Encode(shipment.Sender.UserName)%></td>
                        <td><%
                        using (this.Html.BeginForm("Cancel", "Shipment", new { id = shipment.Id.ToString() }, FormMethod.Post))
                        {%>
                            <input type="submit" value="Cancel" class="cancel-shipment" />
                            <%=this.Html.Tooltip(Resources.TooltipText.ShipmentManagerHint)%>
                            <%
                        }%>
                        </td>
                    <%
                    }%>
                </tr>
            <%
                }%>
                </tbody>        
            </table>
        </div>
        <%
            }
            else
            {%>
            There are no shipments to display. 
        <%
            }%>
    </center>
</asp:Content>
