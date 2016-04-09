<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" ValidateRequest='false' %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Accepting a Username/Password backed Managed Information Card</title>
</head>
<body>
    <object type='application/x-informationCard' id='icardObj'>
      <param name='issuer' value='https://localhost/CustomUserNameCardStsHostFactory/Service.svc' />
      <param name='tokenType' value='urn:oasis:names:tc:SAML:1.0:assertion' />
      <param name='requiredClaims' value='http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname' />
      <param name='optionalClaims' value='http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress' />
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
            Accepting a Username/Password backed Managed Information Card<br />
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
             <asp:Label
                ID="SignedInMessage"
                runat='server'
                Visible='false'
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
                />                       
            &nbsp;<asp:Panel ID="Panel1" runat="server" Height="264px" Visible="False" Width="677px">
                The following claims were found in the submitted card:<br />
                <br />
                <asp:ListBox ID="ListBox1" runat="server" Height="190px" Width="664px"></asp:ListBox></asp:Panel>
        </span>
      </div>
    </form>
</body>
</html>
