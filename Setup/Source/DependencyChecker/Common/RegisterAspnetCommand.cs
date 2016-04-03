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
    public class RegisterAspnetCommand : IDependencySetupCommand
    {

        public RegisterAspnetCommand()
        {
            this.Completed = false;
        }

        public bool Completed
        {
            get;
            private set;
        }

        public void Execute(Dependency dependency)
        {
            dependency.Settings = "/iu:NetFx3;WCF-HTTP-Activation";

            var pkgMgrCmd = new PkgMgrCommand();
            pkgMgrCmd.Execute(dependency);

            var appCmd = new AppCmdWrapper();
            appCmd.RegisterAspNetInIIS();

            this.Completed = true;
        }

    }
}
