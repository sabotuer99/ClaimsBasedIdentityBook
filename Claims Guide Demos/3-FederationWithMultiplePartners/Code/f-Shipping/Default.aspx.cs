//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.UI;

    public class Default : Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            string originalPath = this.Request.Path;
            HttpContext.Current.RewritePath(this.Request.ApplicationPath, false);
            IHttpHandler httpHandler = new MvcHttpHandler();
            httpHandler.ProcessRequest(HttpContext.Current);
            HttpContext.Current.RewritePath(originalPath, false);
        }
    }
}