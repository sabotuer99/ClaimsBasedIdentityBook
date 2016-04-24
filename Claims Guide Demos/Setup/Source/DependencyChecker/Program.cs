//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using DependencyChecker.Common.Services;
    using Microsoft.Web.PlatformInstaller;

    public static class Program
    {
        /// <summary>
        ///   The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IErrorService errorService = new ErrorService();
            var manager = new ProductManager();
            try
            {
                using (var splash = new WorkingSplash())
                {
                    splash.DisableProgressBar();
                    splash.Show();
                    var t = new Task(()=>manager.Load(manager.DefaultFeed, true));
                    t.Start();
                    t.Wait();
                }
            }
            catch (Exception e)
            {
                errorService.ShowError("An unrecoverable error occured. Please check the Event Log to see the details.", e);
                Application.Exit();
            }

            Application.Run(new DependencyCheckerForm(errorService, manager));
        }
    }
}