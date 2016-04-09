<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CardSignIn.aspx.cs" ValidateRequest="false" Inherits="CardSignIn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Card SignIn Page</title>
</head>
<body>
    <object type='application/x-informationCard' id='icardObj'>
      <param name='issuer' value='https://localhost/AuthAssuranceSTS/' />
      <param name='tokenType' value='urn:oasis:names:tc:SAML:1.0:assertion' />
      <param name='requiredClaims' value='http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod' />
      <param name='optionalClaims' value='http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth http://schemas.xmlsoap.org/ws/2005/05/identity/claims/postalcode http://schemas.xmlsoap.org/ws/2005/05/identity/claims/mobilephone' />
    </object>
    <form id="form1" runat="server">
    <div>
        <span style="font-family: Tahoma">
            <span style="font-size: 16pt">Windows Identity Foundation Samples<br /></span>
        </span>
        <hr />
        <span style="font-family: Tahoma">
            <br />
            <span style="font-size: 14pt">
            Accepting an X509 certificate-backed Managed Information Card<br />
            </span>
            <br />
            <br />       
            <input type='hidden' name='tokenXml' value='' />
           <span style='padding-left:20px;'>
             <asp:Button
                runat='server'
                Id='SigninButton'
                Text='Click Here'
                Visible='true'
                OnClientClick='javascript:tokenXml.value=icardObj.value;'
                />
           </span>
           &nbsp;&nbsp;
            <%-- You can set properties such as DisplayType and RequireUserInteraction on the following InformationCard Control 
                 to invoke the corresponding Windows Cardspace 2.0 features --%>      
            &nbsp;<br />
            <br />
           <asp:Label 
                ID="LoginError" 
                runat='server' 
                Text="" 
                ForeColor='Red' 
                Visible='false'
                />                       
        </span>
      </div>
    </form>
</body>
</html>
