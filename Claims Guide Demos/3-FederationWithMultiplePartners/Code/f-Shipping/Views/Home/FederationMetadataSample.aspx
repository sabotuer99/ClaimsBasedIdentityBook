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
    <%=this.Html.ActionLink("Home", "JoinNow")%>
    <h4>Federation metadata file</h4>
    The FederationMetadata.xml file provided by an issuer (Adatum in this sample) contains all the information 
    needed by the relying party to federate authentication with the issuer.
    <p>The important sections of this file are explained below.</p>
           
        <div class="sectionexplanationcontainer">
            <span class="titlesection">Organization section</span>
            <span class="explanationsection">The organization name is read from this section.</span>
            <span class="xmlsection">&lt;Organization&gt;
    &lt;OrganizationDisplayName xml:lang=&quot;&quot;&gt;Adatum&lt;/OrganizationDisplayName&gt;
    <span class="highlight">&lt;OrganizationName xml:lang=&quot;&quot;&gt;Adatum&lt;/OrganizationName&gt;</span>
    &lt;OrganizationURL xml:lang=&quot;&quot;&gt;http://localhost/Adatum.Portal/&lt;/OrganizationURL&gt;
&lt;/Organization&gt;</span>
        </div>
 
        <div class="sectionexplanationcontainer">
            <span class="titlesection">Issuer section</span>
            <span class="explanationsection">The issuer URL is read from this section.</span>
            <span class="xmlsection">&lt;fed:SecurityTokenServiceEndpoint&gt;
    &lt;EndpointReference xmlns=&quot;http://www.w3.org/2005/08/addressing&quot;&gt;
        <span class="highlight">&lt;Address&gt;https://localhost/Adatum.SimulatedIssuer/&lt;/Address&gt;</span>
    &lt;/EndpointReference&gt;
&lt;/fed:SecurityTokenServiceEndpoint&gt;</span>
        </div>
 
        <div class="sectionexplanationcontainer">
            <span class="titlesection">Certificate section</span>
            <span class="explanationsection">This is the certificate (encoded in base64) used by the issuer to sign the tokens.</span>
            <span class="xmlsection">&lt;RoleDescriptor ...&gt;
    &lt;KeyDescriptor use=&quot;signing&quot;&gt;
        &lt;KeyInfo xmlns=&quot;http://www.w3.org/2000/09/xmldsig#&quot;&gt;
            &lt;X509Data&gt;
               <span class="highlight">&lt;X509Certificate&gt;
                    MIIB5TCCAVKgAwIBAgIQMZCkodF3ebNNbz2T00eeBDAJBgUrDgMCHQUAMBExDzANBgNVBAMTBm
                    FkYXR1bTAeFw0wMDAxMDEwMzAwMDBaFw0zNjAxMDEwMzAwMDBaMBExDzANBgNVBAMTBmFkYXR1
                    bTCBnzANBgkqhkiG9w0BAQEFAAOBjQAwgYkCgYEA3ECphJdXyrqrri9YbFaf08PlyB1OiqR6kn
                    xdMqrCFsxypnTlgB9wG2pSleyTSD8xE+HQZxDjSkFEQk/yXKmUi6miHDq6+lvXgEhaIrpqW1Md
                    DRythP89eowmApQzUS1xuyJOjc/hde6vwG17SO40HLxh6fD8FGd/J5z3PXUuGM0CAwEAAaNGME
                    QwQgYDVR0BBDswOYAQ6eAwAJ+duKI0U40uPsmY96ETMBExDzANBgNVBAMTBmFkYXR1bYIQMZCk
                    odF3ebNNbz2T00eeBDAJBgUrDgMCHQUAA4GBAL0OSXGaiUZdl+IV9iT4deoxmbyibjBqQl8cwU
                    xiQz1oJeHGjrATqt9cw7+618Bas1qh/2d3wrH+JqwjBlIrzy1at48xDNqf1qcwXxfBezM/PQ4j
                    YFftR1m2lnsxIUrmTmxus9ACobnYW5VusjdyiOJD+Ukyey2pjD/R4LO2B3AO
                &lt;/X509Certificate&gt;</span>
            &lt;/X509Data&gt;
        &lt;/KeyInfo&gt;
    &lt;/KeyDescriptor&gt;
&lt;/RoleDescriptor&gt;</span>
        </div>
        
        <div class="sectionexplanationcontainer">
            <span class="titlesection">Claims section</span>
            <span class="explanationsection">All the claims offered by the issuer are detailed here.</span>
            <span class="xmlsection">&lt;RoleDescriptor ...&gt;
    &lt;fed:ClaimTypesOffered&gt;
        <span class="highlight">&lt;auth:ClaimType Uri=&quot;http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name&quot; 
                        Optional=&quot;True&quot; 
                        xmlns:auth=&quot;http://docs.oasis-open.org/wsfed/authorization/200706&quot;&gt;
            &lt;auth:DisplayName&gt;Name&lt;/auth:DisplayName&gt;
            &lt;auth:Description&gt;Specifies the name of an entity.&lt;/auth:Description&gt;
        &lt;/auth:ClaimType&gt;</span>
        <i>(...)</i>
    &lt;/fed:ClaimTypesOffered&gt;
&lt;/RoleDescriptor&gt;</span>
        </div>
</asp:Content>
