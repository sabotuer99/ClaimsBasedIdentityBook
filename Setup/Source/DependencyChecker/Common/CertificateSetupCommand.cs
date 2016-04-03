//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common
{
    using System;
    using System.IO;
    using System.Security.AccessControl;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Principal;

    public class CertificateSetupCommand : IDependencySetupCommand
    {
        public CertificateSetupCommand()
        {
            this.Completed = false;
        }

        public bool Completed { get; private set; }

        public void Execute(Dependency dependency)
        {
            string user = GetAppPoolUserName();

            var litwarePfx = ImportPfx(@".\certs\litware.pfx", "Passw0rd!", "My", StoreLocation.LocalMachine);
            AddAccessToCertificate(litwarePfx, user);
            ImportPfx(@".\certs\litware.pfx", "Passw0rd!", "TrustedPeople", StoreLocation.LocalMachine);

            var adatumPfx = ImportPfx(@".\certs\adatum.pfx", "Passw0rd!", "My", StoreLocation.LocalMachine);
            AddAccessToCertificate(adatumPfx, user);
            ImportPfx(@".\certs\adatum.pfx", "Passw0rd!", "TrustedPeople", StoreLocation.LocalMachine);

            var fabrikamPfx = ImportPfx(@".\certs\fabrikam.pfx", "Passw0rd!", "My", StoreLocation.LocalMachine);
            AddAccessToCertificate(fabrikamPfx, user);
            ImportPfx(@".\certs\fabrikam.pfx", "Passw0rd!", "TrustedPeople", StoreLocation.LocalMachine);

            var localhostPfx = ImportPfx(@".\certs\localhost.pfx", "xyz", "My", StoreLocation.LocalMachine);
            AddAccessToCertificate(localhostPfx, user);
            ImportPfx(@".\certs\localhost.pfx", "xyz", "TrustedPeople", StoreLocation.LocalMachine);

            ImportCer(@".\certs\root.cer", "AuthRoot", StoreLocation.LocalMachine);

            this.Completed = true;
        }

        private static void AddAccessToCertificate(X509Certificate2 cert, string user)
        {
            var rsa = cert.PrivateKey as RSACryptoServiceProvider;

            if (rsa != null)
            {
                string keyfilepath =
                    FindKeyLocation(rsa.CspKeyContainerInfo.UniqueKeyContainerName);

                var file = new FileInfo(keyfilepath + "\\" +
                                        rsa.CspKeyContainerInfo.UniqueKeyContainerName);

                FileSecurity fs = file.GetAccessControl();

                var account = new NTAccount(user);
                fs.AddAccessRule(new FileSystemAccessRule(account, FileSystemRights.FullControl, AccessControlType.Allow));

                file.SetAccessControl(fs);
            }
        }
        
        private static string FindKeyLocation(string keyFileName)
        {
            string text1 =
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string text2 = text1 + @"\Microsoft\Crypto\RSA\MachineKeys";
            string[] textArray1 = Directory.GetFiles(text2, keyFileName);
            if (textArray1.Length > 0)
            {
                return text2;
            }

            string text3 =
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string text4 = text3 + @"\Microsoft\Crypto\RSA\";
            textArray1 = Directory.GetDirectories(text4);
            if (textArray1.Length > 0)
            {
                foreach (string text5 in textArray1)
                {
                    textArray1 = Directory.GetFiles(text5, keyFileName);
                    if (textArray1.Length != 0)
                    {
                        return text5;
                    }
                }
            }

            return "Private key exists but is not accessible";
        }

        private static string GetAppPoolUserName()
        {
            string val = Environment.OSVersion.Version.ToString();
            if (val.StartsWith("6.1"))
            {
                return "IIS_IUSRS";
            }

            return "Network Service";
        }

        private static void ImportCer(string cerPath, string storeName, StoreLocation storeLocation)
        {
            var cer = new X509Certificate2();
            cer.Import(cerPath);

            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.MaxAllowed);
            store.Add(cer);
            store.Close();
        }

        private static X509Certificate2 ImportPfx(string pfxPath, string pfxPwd, string storeName, StoreLocation storeLocation)
        {
            const X509KeyStorageFlags options = X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet;
            var pfx = new X509Certificate2();
            pfx.Import(pfxPath, pfxPwd, options);
            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.MaxAllowed);
            store.Add(pfx);
            store.Close();

            return pfx;
        }
    }
}