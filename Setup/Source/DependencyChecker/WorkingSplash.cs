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
    using System.Windows.Forms;

    public partial class WorkingSplash : Form
    {
        public WorkingSplash()
        {
            this.InitializeComponent();
        }

        public int MaxDependencies
        {
            get { return this.progressBar.Maximum; }
            set { this.progressBar.Maximum = value; }
        }

        public string ScanningPrompt
        {
            set { this.scanningLabel.Text = value; }
        }

        public void DisableProgressBar()
        {
            this.progressBar.Visible = false;
        }

        public void ShowCurrentProgress(int current)
        {
            this.progressBar.Value = current;
        }
    }
}