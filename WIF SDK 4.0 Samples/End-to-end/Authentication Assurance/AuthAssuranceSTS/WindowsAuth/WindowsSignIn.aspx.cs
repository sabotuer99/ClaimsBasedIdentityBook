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
using Microsoft.IdentityModel.Web;
using AuthAssuranceSTS;

public partial class WindowsSignIn : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// We perform the WS-Federation Passive Protocol processing in this method. 
    /// </summary>
    protected void Page_PreRender( object sender, EventArgs e )
    {
        FederatedPassiveSecurityTokenServiceOperations.ProcessRequest( Request, User, CustomSecurityTokenServiceConfiguration.Current.CreateSecurityTokenService(), Response );
    }
}
