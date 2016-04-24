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
                <p>Select the option that best matches your company: <%=this.Html.Tooltip(Resources.TooltipText.IssuerTypeHint)%></p>
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
                    <p>Your issuer can be configured by choosing one of the following options: <%=this.Html.Tooltip(Resources.TooltipText.IssuerConfigurationIPHint)%></p>
                    <div class="sampleform">
                        <ul>
                            <li><a href="<%=this.Url.Action("EnrollWithFedMetadataFile","Enrollment") %> ">Upload a federation metadata file</a></li>
                            <li><a href="<%=this.Url.Action("EnrollManually","Enrollment") %> ">Fill federation metadata manually</a></li>
                        </ul>
                    </div>
                </div>
                <div id="fabrikamIssuerTab" class="issuerOptionTab" style="display: none;">
                    <p>Select your identity provider: <%=this.Html.Tooltip(Resources.TooltipText.IssuerConfigurationSIPHint)%></p>
                    <div class="sampleform">
                        <ul>
                                        <%
                                            var ips = (this.Model as IEnumerable<SelectListItem>);
                                            foreach (var item in ips)
                                            { %>
                                            <li><a href="<%=this.Url.Action("EnrollWithSocialProvider","Enrollment", new { socialip = Url.Encode(item.Value) }) %> ">
                                            <%=item.Text %>
                                            </a></li>
                                        <%
                                            }         
                                        %>
                            
                            </ul>
                    </div>
                </div>
            </span>
        </div>
    </div>
</asp:Content>
