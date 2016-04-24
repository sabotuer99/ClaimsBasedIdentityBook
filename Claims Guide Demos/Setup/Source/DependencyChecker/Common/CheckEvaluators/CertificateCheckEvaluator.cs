//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common.CheckEvaluators
{
    using System;
    using System.Security.Cryptography.X509Certificates;

    public class CertificateCheckEvaluator : ICheckEvaluator
    {
        public virtual bool Evaluate(Check check, IEvaluationContext context)
        {
            string subject;
            string storeName;
            StoreLocation location;
            this.ExtractCertParameters(check.Value, out subject, out storeName, out location);
            var store = new X509Store(storeName, location);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectDistinguishedName, subject, false);

            return certCollection.Count > 0;
        }

        protected void ExtractCertParameters(string input, out string subject, out string storeName, out StoreLocation location)
        {
            var certData = input.Split(',');
            if (certData.Length < 3)
            {
                throw new InvalidOperationException("Missing certificate data");
            }

            subject = certData[2];
            storeName = certData[1];
            location = StoreLocation.CurrentUser;
            if (certData[0].Equals("LocalMachine"))
            {
                location = StoreLocation.LocalMachine;
            }
        }
    }
}