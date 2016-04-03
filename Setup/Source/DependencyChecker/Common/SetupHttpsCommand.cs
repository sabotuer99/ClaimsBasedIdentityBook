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
    public class SetupHttpsCommand : IDependencySetupCommand
    {
        public SetupHttpsCommand()
        {
            this.Completed = false;
        }

        public bool Completed { get; private set; }

        public void Execute(Dependency dependency)
        {
            var appCmd = new AppCmdWrapper();
            appCmd.SetHttpsBinding();
            this.Completed = true;
        }
    }
}