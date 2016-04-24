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
        $(document).ready(
            function () {
                $('input[name=issuerOption]:radio').click(
                    function () {
                        $('#issuerOptionTabs').show();
                        $('#issuerOptionTabs div.issuerOptionTab').hide();

                        if ($('input[name=issuerOption]:radio:checked').val() == 'yourIssuerOption')
                            $('#yourIssuerTab').show();
                        else
                            $('#fabrikamIssuerTab').show();
                    });
            });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
    <h4>
        Enroll your company in Fabrikam Shipping!</h4>
    In a real federated application, a new customer would be able to become a tenant
    of f-Shipping by completing a form similar to the one below.
    <div id="issuerOptionTabs">
        <div class="sectionexplanationcontainer">
            <span class="titlesection">Step 3: Organization and Issuer configuration</span>
            <span class="explanationsection">
                <div id="yourIssuerTab" class="issuerOptionTab">
                    <div class="sampleform">
                        <%this.Html.BeginForm("CreateTenantManually", "Enrollment", FormMethod.Post, new { enctype = "multipart/form-data" }); %>
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        Organization name:
                                    </td>
                                    <td>
                                        <% var model = (this.Model as FShipping.Models.EnrollmentViewModel); %>
                                        <%=this.Html.TextBox("organizationName", model.OrganizationName)%>
                                        <%if (!model.IsValidModel)
                                          {%>
                                        <span class="error">
                                            <%=this.Html.Label(model.ErrorMessage) %></span>
                                        <%}%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Issuer name:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("issuerName",string.Empty, new { size = "60" })%>
                                        <%=this.Html.Tooltip(Resources.TooltipText.EntityIDHint)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Admin claim type:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("adminClaimType",string.Empty, new { size = "60" })%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Admin claim value:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("adminClaimValue",string.Empty, new { size = "60" })%>
                                        <%=this.Html.Tooltip(Resources.TooltipText.AdminClaimHint)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Cost center claim type:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("costCenterClaimType",string.Empty, new { size = "60" })%>
                                        <%=this.Html.Tooltip(Resources.TooltipText.CostCenterHint)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Logo:
                                    </td>
                                    <td>
                                        <input type="file" name="logoFile" id="logoFile" size="60" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Sign-in URL:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("iPStsAddress", "", new { size = "70" })%>
                                        <%=this.Html.Tooltip(Resources.TooltipText.SignInAddressHint)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Certificate:
                                    </td>
                                    <td>
                                        <input type="file" name="certificateFile" id="certificateFile" size="60" />
                                        <%=this.Html.Tooltip(Resources.TooltipText.CertificateHint)%>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div style="text-align: right; margin-top: 10px;">
                            <input type="submit" value="Submit" />
                        </div>
                        <% this.Html.EndForm(); %>
                    </div>
                </div>
            </span>
        </div>
    </div>
</asp:Content>
