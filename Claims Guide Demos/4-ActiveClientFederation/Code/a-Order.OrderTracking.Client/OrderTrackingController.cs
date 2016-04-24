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
    using System;
    using System.Windows.Controls;
    using AOrder.OrderTracking.Client.OrderTrackingService;

    public class OrderTrackingController
    {
        private readonly Shell shell;

        public OrderTrackingController(Shell shell)
        {
            this.shell = shell;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Since this is just an example we only catch Exception.")]
        public bool LoginUser()
        {
            try
            {
                this.ShowOrders();
                this.shell.HideError();
                return true;
            }
            catch (Exception ex)
            {
                this.DisplayError(ex.Message);
                return false;
            }
        }

        public void Run()
        {
            this.DisplayView(new LoginView
                                 {
                                     DataContext = new LoginViewModel(this)
                                 });
        }

        private void DisplayError(string errorMessage)
        {
            this.shell.ShowError(errorMessage);
        }

        private void DisplayView(UserControl view)
        {
            this.shell.SetContent(view);
        }

        private void ShowOrders()
        {
            using (var client = new OrderTrackingServiceClient())
            {
                client.ClientCredentials.UserName.UserName = "LITWARE\\rick";
                client.ClientCredentials.UserName.Password = "thisPasswordIsNotChecked";

                var orders = client.GetOrdersFromMyOrganization();

                this.DisplayView(new OrderTrackingView
                                     {
                                         DataContext = new OrderTrackingViewModel(orders)
                                     });
            }
        }
    }
}