//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Litware.Portal.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.Net;
    using System.Net.Security;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.UI;

    public partial class PrerequisitesControl : UserControl
    {
        private readonly ICollection<string> errors = new Collection<string>(new List<string>());

        protected ICollection<string> Errors
        {
            get { return this.errors; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            CheckIfHttpsBindingIsConfiguredInIis(this.errors);
            CheckIfCertificateExists("CN=adatum", this.errors);
            CheckIfCertificateExists("CN=litware", this.errors);

            this.Visible = this.errors.Count != 0;
        }

        private static void CheckIfCertificateExists(string certificateSubjectName, ICollection<string> errors)
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            foreach (var certificate in store.Certificates)
            {
                if (certificate.SubjectName.Name == certificateSubjectName)
                {
                    return;
                }
            }

            errors.Add(string.Format(CultureInfo.CurrentUICulture, "Certificate {0} is not installed properly.", certificateSubjectName));
        }

        private static void CheckIfHttpsBindingIsConfiguredInIis(ICollection<string> errors)
        {
            try
            {
                // Disable certificate validation for https connections.
                ServicePointManager.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;

                WebRequest.Create("https://localhost/").GetResponse();
            }
            catch (WebException)
            {
                errors.Add("HTTPS binding is not configured in IIS.");
            }
            finally
            {
                // Re-enable certificate validation for https connections.
                ServicePointManager.ServerCertificateValidationCallback = null;
            }
        }

        // This method checks if the certificate used for ssl is correct.
        // For the samples running in localhost, all the certificates are accepted.
        // In a real environment, validating the certificate is very important to
        // avoid security issues like identity theft.
        private static bool RemoteCertificateValidationCallback(
            object sender, 
            X509Certificate certificate, 
            X509Chain chain, 
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}