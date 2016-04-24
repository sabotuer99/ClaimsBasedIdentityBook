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
    using System.Diagnostics;
    using System.IO;
    using System.Management.Automation;

    public class AppCmdWrapper
    {
        private readonly string pathToAppCmdExe;
        private readonly string pathToAspnetRegIIS;
        private readonly string pathToNetSh;

        public AppCmdWrapper()
        {
            this.pathToAppCmdExe = Environment.ExpandEnvironmentVariables(@"%windir%\system32\inetsrv\appcmd");
            this.pathToAspnetRegIIS = Environment.ExpandEnvironmentVariables(@"%windir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis");
            this.pathToNetSh = "netsh";
        }

        public void LoadDefaultUserProfile()
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "set apppool \"ASP.NET v4.0\" /processModel.loadUserProfile:true";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    {
                        throw new RuntimeException(result);
                    }
                }
            }
        }

        public bool IsDefaultUserProfileEnabled()
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "list apppools /name:\"ASP.NET v4.0\" /processModel.loadUserProfile:true";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result.Contains("APPPOOL");
                }
            }
        }


        public void CreateWebApplication(string applicationPath, string applicationName)
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            string args = string.Format("add app /site.name:\"Default web site\" /path:\"/{1}\" /physicalPath:\"{0}\\{1}\"", applicationPath, applicationName);
            start.Arguments = args;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    {
                        throw new RuntimeException(result);
                    }
                }
            }
        }

        public bool ExistsApplication(string applicationName)
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "list app";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result.Contains(applicationName);
                }
            }
        }

        public bool IsHttpsEnabled()
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "list site /site.name:\"Default Web Site\"";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    return result.Contains("https");
                }
            }
        }

        public void SetHttpsBinding()
        {
            // this is the hash of the localhost certificate
            const string certHash = "5a074d678466f59dbd063d1a98b1791474723365";

            // this appId is the default used to performed this operation
            const string appId = "{4dc3e181-e14b-4a21-b022-59fc669b0914}";

            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToNetSh);
            string args = string.Format("http add sslcert ipport=0.0.0.0:443 certhash={0} appid={1}", certHash, appId);
            start.Arguments = args;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    {
                        throw new RuntimeException(result);
                    }
                }
            }

            start = this.CreateProcessStartInfo(this.pathToAppCmdExe);
            start.Arguments = "set config -section:system.applicationHost/sites /+\"[name='Default Web Site'].bindings.[protocol='https',bindingInformation='*:443:']\" /commit:apphost";
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    {
                        throw new RuntimeException(result);
                    }
                }
            }
        }

        public void RegisterAspNetInIIS()
        {
            ProcessStartInfo start = this.CreateProcessStartInfo(this.pathToAspnetRegIIS);
            
            start.Arguments = "-ir";

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("Error") || result.Contains("error"))
                    { 
                        throw new RuntimeException(result);
                    }
                }
            }
        }

        protected ProcessStartInfo CreateProcessStartInfo(string pathToExe)
        {
            var start = new ProcessStartInfo
            {
                FileName = pathToExe,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            return start;
        }

    }
}