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
using System.Web;

public partial class callback : System.Web.UI.Page
{
    /// <summary>
    /// This page returns the text from the AJAX call.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks>
    /// Do not cache this page so that token timeouts are always checked.
    /// </remarks>
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Cache.SetCacheability( HttpCacheability.NoCache );
        Response.Write(String.Format("<p>{0}, this message was returned from the service using AJAX at {1}.</p>", Server.HtmlEncode(Page.User.Identity.Name), DateTime.Now));
        Response.Flush();
        Response.End();
    }
}
