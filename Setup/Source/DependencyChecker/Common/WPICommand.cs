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
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Web.PlatformInstaller;

    public class WpiCommand : IDependencySetupCommand, IRequiresProductManager, IDisposable
    {
        private readonly InstallManager installManager;
        private bool completed;
        private bool iisComponent;
        private ProductManager productManager;

        public WpiCommand()
        {
            this.installManager = new InstallManager();
        }

        public bool Completed
        {
            get { return this.completed; }
        }

        public ProductManager ProductManager
        {
            get { return this.productManager; }
            set { this.productManager = value; }
        }

        public void Dispose()
        {
            if (this.installManager != null)
            {
                this.installManager.Dispose();
            }
        }

        public void Execute(Dependency dependency)
        {
            var settings = dependency.Settings.Split('!');
            var installers = new Dictionary<string, Installer>();
            foreach (var setting in settings)
            {
                var product = this.productManager.GetProduct(setting);
                var sets = product.DependencySets.ToList();
                foreach (var installer in product.Installers)
                {
                    if (!installers.ContainsKey(installer.Product.ProductId))
                    {
                        installers.Add(installer.Product.ProductId, installer);
                    }
                }
                //installers.AddRange(product.Installers);
                foreach (var item in sets.SelectMany(items => items))
                {
                    if (item.IsInstalled(false))
                    {
                        continue;
                    }
                    this.CheckIisComponent(item);
                    foreach (var installer in item.Installers)
                    {
                        if (!installers.ContainsKey(installer.Product.ProductId))
                        {
                            installers.Add(installer.Product.ProductId, installer);
                        }
                    }
                    //installers.AddRange(item.Installers);
                }
                this.CheckIisComponent(product);
            }
            this.installManager.Load(installers.Values);
            this.installManager.InstallCompleted += this.OnInstallCompleted;
            this.installManager.StartInstallation();
        }

        private void CheckIisComponent(Product product)
        {
            if (!this.iisComponent && product.IsIisComponent)
            {
                this.iisComponent = true;
            }
        }

        private void OnInstallCompleted(object sender, EventArgs e)
        {
            if (this.iisComponent)
            {
                var appCommand = new AppCmdWrapper();
                appCommand.RegisterAspNetInIIS();
            }
            this.completed = true;
        }
    }
}