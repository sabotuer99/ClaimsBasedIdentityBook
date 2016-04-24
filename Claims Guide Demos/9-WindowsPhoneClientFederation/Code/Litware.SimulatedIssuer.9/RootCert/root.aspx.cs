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
using System.Security.Cryptography.X509Certificates;

namespace Litware.SimulatedIssuer.RootCert
{
    public partial class root : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var certStore = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            certStore.Open(OpenFlags.MaxAllowed | OpenFlags.ReadOnly);
            var certificate = certStore.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, "CN=Root Agency", false)[0];

            var exportBytes = (new X509Certificate2Collection(certificate)).Export(X509ContentType.Pkcs7);
            Response.ClearHeaders();
            Response.BufferOutput = false;
            Response.ContentType = "application/x-pkcs7-certificates";
            Response.BinaryWrite(exportBytes);
            Response.End();
        }
    }
}