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

    public partial class InstallingDependency : Form
    {
        private int counter;

        public InstallingDependency()
        {
            this.InitializeComponent();
            this.counter = 0;
        }

        public void ShowProgress()
        {
            this.counter++;
            this.progressLabel.Text += @".";
            if (this.counter > 5)
            {
                this.counter = 0;
                this.progressLabel.Text = @".";
            }
        }
    }
}