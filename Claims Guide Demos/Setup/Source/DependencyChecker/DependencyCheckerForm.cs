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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using DependencyChecker.Common;
    using DependencyChecker.Common.Services;
    using DependencyChecker.Configuration;
    using DependencyChecker.Controls;
    using Microsoft.Web.PlatformInstaller;

    public partial class DependencyCheckerForm : Form
    {
        private readonly IErrorService errorService;
        private readonly ProductManager productManager;
        private DependenciesSection configSection;
        private DependenciesInfo dependenciesInfo;
        private Dictionary<string, DependencyGroupView> dependencyGroupViews;
        private System.Configuration.Configuration fileConfig;


        public DependencyCheckerForm(IErrorService errorService, ProductManager productManager)
        {
            this.productManager = productManager;
            this.errorService = errorService;


            try
            {
                this.InitializeComponent();
                this.InitializeConfigurationSection();
                this.titleLabel.Text = this.configSection.Title;
                this.Text = this.configSection.Title;
                this.descriptionLabel.Text = this.configSection.Description;
                this.InitializedDependencyGroupViews();
            }
            catch (Exception ex)
            {
                this.errorService.ShowError("Could not load DependencyChecker Form", ex);
                Application.Exit();
            }
        }

        protected virtual DependenciesInfoBuilder GetDependenciesInfoBuilder()
        {
            return new DependenciesInfoBuilder(this.productManager);
        }

        private static void OnButtonExitClicked(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AddDependencyControl(Dependency dependency, IEvaluationContext context)
        {
            this.dependencyGroupViews[dependency.Category].AddDependency(dependency, context);
        }

        protected override void OnLoad(EventArgs e)
        {
            foreach (DependencyGroupView dependencyGroupView in this.dependencyGroupViews.Values)
            {
                dependencyGroupView.Reset();
            }

            foreach (Dependency d in this.dependenciesInfo.Dependencies)
            {
                this.AddDependencyControl(d, null);
            }
            base.OnLoad(e);
        }

        private void InitializeConfigurationSection()
        {
            const string configurationFileName = "App.config";
            string configFilePath = Path.Combine(Path.GetTempPath(), configurationFileName);
            TextReader configReader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream(this.GetType().Namespace + "." + configurationFileName));
            File.WriteAllText(configFilePath, configReader.ReadToEnd());

            var map = new ExeConfigurationFileMap { ExeConfigFilename = configFilePath };
            this.fileConfig = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            this.configSection = (DependenciesSection)this.fileConfig.GetSection(DependenciesSection.SectionName);

            try
            {
                this.dependenciesInfo = this.GetDependenciesInfoBuilder().BuildDependenciesInfo(this.configSection);
            }
            catch (NotSupportedException e)
            {
                MessageBox.Show(e.Message, @"OS Not Supported", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }

        private void InitializedDependencyGroupViews()
        {
            this.SuspendLayout();

            this.dependencyGroupViews = new Dictionary<string, DependencyGroupView>();
            foreach (Dependency dependency in this.dependenciesInfo.Dependencies)
            {
                if (!this.dependencyGroupViews.ContainsKey(dependency.Category))
                {
                    var newGroupView = new DependencyGroupView(this.errorService, this.productManager)
                                           {
                                               Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                                               AutoSize = true,
                                               AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                               Dependencies = null,
                                               Heading = dependency.Category,
                                               Size = new Size(this.flowLayoutPanel.Width - 6, 18),
                                               MinimumSize = new Size(this.flowLayoutPanel.Width - 6, 18)
                                           };

                    this.flowLayoutPanel.Controls.Add(newGroupView);
                    this.dependencyGroupViews.Add(dependency.Category, newGroupView);
                }
            }
            this.ResumeLayout(true);
        }

        private void OnButtonScanClicked(object sender, EventArgs e)
        {
            try
            {
                this.scanButton.Enabled = false;
                this.Scan();
                this.scanButton.Text = @"&Rescan";
            }
            finally
            {

                this.scanButton.Enabled = true;
            }
        }

        private void Scan()
        {
            foreach (DependencyGroupView dependencyGroupView in this.dependencyGroupViews.Values)
            {
                dependencyGroupView.Reset();
            }

            var splash = new WorkingSplash();
            splash.Show();
            splash.MaxDependencies = this.dependenciesInfo.Dependencies.Count;
            Application.DoEvents();

            int current = 0;
            foreach (Dependency d in this.dependenciesInfo.Dependencies)
            {
                splash.ScanningPrompt = "Scanning for " + d.Title;
                splash.ShowCurrentProgress(current++);
                this.AddDependencyControl(d, this.dependenciesInfo.EvaluationContext);
            }

            splash.Close();
        }
    }
}