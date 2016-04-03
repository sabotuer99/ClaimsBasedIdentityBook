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
    <%this.Html.BeginForm("CreateTenantWithSocialProvider", "Enrollment", FormMethod.Post, new { enctype = "multipart/form-data" }); %>
    <h4>Enroll your company in Fabrikam Shipping!</h4>
    In a real federated application, a new customer would be able to become a tenant
    of f-Shipping by completing a form similar to the one below.
    <div id="issuerOptionTabs">
        <div class="sectionexplanationcontainer" >
            <span class="titlesection">Step 3: Issuer configuration</span>
            <span class="explanationsection">
                <div id="fabrikamIssuerTab" class="issuerOptionTab">
                    <p>By filling in this form, you:</p>
                    <ul>
                        <li>Become an administrator for your organization</li>
                        <li>Start managing your organization's users and their roles <%=this.Html.Tooltip(Resources.TooltipText.SIPAdminHint)%></li>
                    </ul>
                    <div class="sampleform">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        Organization name:
                                    </td>
                                    <td>
                                    <% var model = (this.Model as FShipping.Models.EnrollmentViewModel);
                                        if(model !=null)
                                        {%>
                                        <%=this.Html.TextBox("OrganizationName", model.OrganizationName)%> 
                                        
                                        <%if (!model.IsValidModel)
                                        {%>
                                            <span class="error"> <%=this.Html.Label(model.ErrorMessage) %></span>
                                        <%}%>
                                         <%} else {%>
                                         <%=this.Html.TextBox("OrganizationName")%> 
                                         <%} %>    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Logo:
                                    </td>
                                    <td>
                                        <input type="file" name="logoFile" id="logoFile" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div style="text-align: right; margin-top: 10px;">
                        <input type="submit" value="Submit" />
                    </div>
                </div>
            </span>
        </div>
    </div>
    <% this.Html.EndForm(); %>
</asp:Content>
