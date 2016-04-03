//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.Client
{
    using System.Windows;

    public partial class Shell
    {
        public Shell()
        {
            this.InitializeComponent();
        }

        public void HideError()
        {
            this.ErrorSection.Visibility = Visibility.Hidden;
        }

        public void SetContent(object content)
        {
            this.contentPlaceholder.Content = content;
        }

        public void ShowError(string errorMessage)
        {
            this.ErrorSection.Visibility = Visibility.Visible;
            this.ErrorText.Text = errorMessage;
        }
    }
}