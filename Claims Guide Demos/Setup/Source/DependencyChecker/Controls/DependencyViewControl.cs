//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Controls
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;
    using DependencyChecker.Common;
    using DependencyChecker.Common.Services;
    using DependencyChecker.Properties;
    using Microsoft.Web.PlatformInstaller;

    public partial class DependencyViewControl : UserControl
    {
        private readonly IErrorService errorService;
        private readonly ProductManager productManager;
        private readonly Color titleSavedColor;
        private Dependency dependency;
        private InstallingDependency installingDependency;

        public DependencyViewControl(IErrorService errorService, ProductManager productManager)
        {
            this.errorService = errorService;
            this.productManager = productManager;
            this.InitializeComponent();
            this.llDownloadUrl.Text = string.Empty;
            this.titleSavedColor = this.lblTitle.ForeColor;
        }

        public bool DependencyStatus { get; set; }

        public void SetDependency(Dependency dependencyToEvalulate, IEvaluationContext context)
        {
            this.dependency = dependencyToEvalulate;

            this.lblTitle.Text = dependencyToEvalulate.Title;
            this.lblExplanation.Text = dependencyToEvalulate.Explanation;
            this.toolTip1.SetToolTip(this.lblExplanation, dependencyToEvalulate.Explanation);
            if (!string.IsNullOrEmpty(dependencyToEvalulate.DownloadUrl))
            {
                this.llDownloadUrl.Links[0].Description = dependencyToEvalulate.DownloadUrl;
                this.llDownloadUrl.Text = @"Download";
            }
            else if (!string.IsNullOrEmpty(dependencyToEvalulate.ScriptName))
            {
                this.llDownloadUrl.Links[0].Description = dependencyToEvalulate.ScriptName;
                this.llDownloadUrl.Text = @"Install now";
            }
            else if (!string.IsNullOrEmpty(dependencyToEvalulate.InfoUrl))
            {
                this.llDownloadUrl.Links[0].Description = dependencyToEvalulate.InfoUrl;
                this.llDownloadUrl.Text = @"More information";
            }
            if (context != null)
            {
                this.EvaluateDependency(dependencyToEvalulate, context);
            }
        }

        private void EvaluateDependency(Dependency dependencyToEvalulate, IEvaluationContext context)
        {
            try
            {
                if (context.Evaluate(dependencyToEvalulate.Check))
                {
                    this.pictureBox1.Image = Resources.Checked;
                    this.llDownloadUrl.Visible = false;
                    this.lblExplanation.Visible = false;
                    this.Height = 25;
                    this.lblTitle.ForeColor = this.titleSavedColor;
                    this.lblTitle.Top = 4;
                    this.DependencyStatus = true;
                }
                else
                {
                    this.pictureBox1.Image = Resources.Unchecked;
                    this.pictureBox1.Top = 10;
                    this.lblTitle.ForeColor = Color.Red;
                    this.DependencyStatus = false;
                    this.lblExplanation.Visible = true;
                    this.llDownloadUrl.Visible = true;
                }
            }
            catch (Exception exception)
            {
                this.pictureBox1.Image = Resources.Unchecked;
                this.pictureBox1.Top = 10;
                this.lblTitle.ForeColor = Color.Red;
                this.lblExplanation.Visible = true;
                this.DependencyStatus = false;
                this.llDownloadUrl.Visible = false;
                string errorMessage =
                    string.Format(
                        "'{0}' dependency could not be verified. Install components above this one first.", 
                        dependencyToEvalulate.Title);
                this.lblExplanation.Text = errorMessage;
                this.toolTip1.SetToolTip(this.lblExplanation, errorMessage);
                this.errorService.LogError(errorMessage, exception);
            }
        }

        private void OnDownloadUrlLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.dependency.DownloadUrl))
            {
                Process.Start(this.dependency.DownloadUrl);
            }
            else if (!string.IsNullOrEmpty(this.dependency.ScriptName))
            {
                if (this.dependency.ScriptName.StartsWith("cmd:"))
                {
                    var worker = new BackgroundWorker { WorkerReportsProgress = true };
                    worker.DoWork += this.OnWorkerDoWork;
                    worker.ProgressChanged += this.OnWorkerProgressChanged;
                    worker.RunWorkerCompleted += this.OnWorkerRunWorkerCompleted;

                    this.installingDependency = new InstallingDependency();
                    try
                    {
                        this.TopLevelControl.Cursor = Cursors.WaitCursor;
                        this.installingDependency.Show();
                        Application.DoEvents();
                        worker.RunWorkerAsync();
                    }
                    catch (Exception ex)
                    {
                        this.errorService.ShowError("This dependency could not be verified. Install components above this one first.", ex);
                    }
                }
                else
                {
                    Process.Start(this.dependency.ScriptName);
                }
            }
            else if (!string.IsNullOrEmpty(this.dependency.InfoUrl))
            {
                Process.Start(this.dependency.InfoUrl);
            }
        }

        private void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            var cmdType = this.dependency.ScriptName.Split(':')[1];
            var command = Activator.CreateInstance(Type.GetType(cmdType)) as IDependencySetupCommand;
            var rpm = command as IRequiresProductManager;
            if (rpm != null)
            {
                rpm.ProductManager = this.productManager;
            }
            command.Execute(this.dependency);
            while (!command.Completed)
            {
                Thread.Sleep(1500);
            }
            var disposable = command as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        private void OnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.installingDependency.ShowProgress();
        }

        private void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.installingDependency.Close();
            this.TopLevelControl.Cursor = Cursors.Default;
        }
    }
}