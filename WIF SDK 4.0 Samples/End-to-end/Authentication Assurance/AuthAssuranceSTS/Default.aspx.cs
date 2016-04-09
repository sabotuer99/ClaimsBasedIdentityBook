//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

using System;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Web;
using AuthAssuranceSTS;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SignInRequestMessage requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri( Request.Url );

        if ( !String.IsNullOrEmpty( requestMessage.AuthenticationType ) )
        {
            if ( String.Equals( requestMessage.AuthenticationType, AuthenticationMethods.X509, StringComparison.Ordinal ) )
            {
                Response.Redirect( "CardSignIn.aspx" + Context.Request.Url.Query );
            }
            else
            {
                throw new NotSupportedException( String.Format( "Unrecognized authentication type requested: {0}", requestMessage.AuthenticationType ) );
            }
        }
        else
        {
            Response.Redirect( @"WindowsAuth\WindowsSignIn.aspx" + Context.Request.Url.Query );
        }
    }
}
