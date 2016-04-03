//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

namespace Adatum.FederationProvider
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ErrorDetails error = serializer.Deserialize<ErrorDetails>(Request["ErrorDetails"]);
            this.lblIdntityProvider.Text = error.identityProvider;
            this.lblErrorMessage.Text = string.Join("<br/>", error.errors.Select(er => string.Format("Error Code {0}: {1}", er.errorCode, er.errorMessage)).ToArray());

        }
    }
    

}