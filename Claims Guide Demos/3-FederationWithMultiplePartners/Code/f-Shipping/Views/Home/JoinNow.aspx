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
            function() {
                $('input[name=issuerOption]:radio').click(
                    function() {
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
    <h4>Enroll your company in Fabrikam Shipping!</h4>
    In a real federated application, a new customer would be able to become a tenant
    of f-Shipping by completing a form similar to the one below.
    <div id="issuerOption">
        <div class="sectionexplanationcontainer">
            <span class="titlesection">Step 1: Issuer type</span>
            <span class="explanationsection">
                <p>Select the option that best matches your company:</p>
                <p>
                    <%=this.Html.RadioButton("issuerOption", "yourIssuerOption", false)%>
                    My company provides an issuer
                </p>
                <p>
                    <%=this.Html.RadioButton("issuerOption", "fabrikamIssuerOption", false)%>
                    My company does not have an issuer
                </p>
            </span>
        </div>
    </div>
    <div id="issuerOptionTabs" style="display: none;">
        <div class="sectionexplanationcontainer" >
            <span class="titlesection">Step 2: Issuer configuration</span>
            <span class="explanationsection">
                <div id="yourIssuerTab" class="issuerOptionTab" style="display: none;">
                    <p>Your issuer can be configured by choosing one of the following options:</p>
                    <p>
                        <%=this.Html.RadioButton("enterConfigurationOption", "fedMetadata", true)%>
                        Enter issuer information from federation metadata</p>
                    <div class="sampleform">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        Federation metadata document:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("fedMetadataDocument", "http://localhost/Adatum.SimulatedIssuer/FederationMetadata/2007-06/FederationMetadata.xml", new { size = "92" })%>
                                        <div id="samplefile">
                                            <%=this.Html.ActionLink("view sample file", "FederationMetadataSample")%></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Logo:
                                    </td>
                                    <td>
                                        <input type="file" name="logo" id="File2" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <p>
                        <%=this.Html.RadioButton("enterConfigurationOption", "fedMetadata")%>
                        Enter issuer information manually</p>
                    <div class="sampleform">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        Organization name:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("OrganizationName", Adatum.OrganizationName)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Logo:
                                    </td>
                                    <td>
                                        <input type="file" name="logo" id="logo" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Sign-in URL:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("IPStsAddress", "https://localhost/Adatum.SimulatedIssuer/", new { size = "55" })%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Certificate:
                                    </td>
                                    <td>
                                        <input type="file" name="certificate" size="55" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div style="text-align: right; margin-top: 10px;">
                        <input type="button" onclick="alert('This page is just a mockup, new tenants cannot be added to the f-Shipping sample application.');"
                            value="Add" />
                    </div>
                </div>
                <div id="fabrikamIssuerTab" class="issuerOptionTab" style="display: none;">
                    <p>By filling in this form, you:</p>
                    <ul>
                        <li>Become an administrator for your organization</li>
                        <li>Start managing your organization's users and their roles</li>
                    </ul>
                    <div class="sampleform">
                        <table>
                            <tbody>
                                <tr>
                                    <td>
                                        Organization name:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("OrganizationName", Contoso.OrganizationName)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Your email:
                                    </td>
                                    <td>
                                        <%=this.Html.TextBox("Email", "bill@contoso.com")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Password:
                                    </td>
                                    <td>
                                        <%=this.Html.Password("Password", "********")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Logo:
                                    </td>
                                    <td>
                                        <input type="file" name="logo" id="File1" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div style="text-align: right; margin-top: 10px;">
                        <input type="button" onclick="alert('This page is just a mockup, new tenants cannot be added to the f-Shipping sample application.');"
                            value="Add" />
                    </div>
                </div>
            </span>
        </div>
    </div>
</asp:Content>
