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
    using System.Configuration;
    using System.Windows.Controls;
    using AOrder.OrderTracking.Client.OrderTrackingService;

    public class OrderTrackingController
    {
        private readonly Shell shell;

        public OrderTrackingController(Shell shell)
        {
            this.shell = shell;
        }

        public bool LoginUser(string userName, string password, RequestType requestType)
        {
            try
            {
                this.ShowOrders(userName, password, requestType);
                this.shell.HideError();
                return true;
            }
            catch (Exception ex)
            {
                this.DisplayError(ex.Message);
                return false;
            }
        }

        public void LogoutUser()
        {
            this.Run();
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

        private void ShowOrders(string userName, string password, RequestType requestType)
        {
            var stsEndpoint = ConfigurationManager.AppSettings["stsEndpoint"];
            var acsNamespace = ConfigurationManager.AppSettings["acsNamespace"];
            var acsRelyingParty = ConfigurationManager.AppSettings["acsRelyingParty"];

            var client = new OrderTrackingServiceClient(acsNamespace, acsRelyingParty, stsEndpoint);
            client.ClientCredentials.UserName.UserName = userName;
            client.ClientCredentials.UserName.Password = password;

            this.DisplayView(new OrderTrackingView
                                 {
                                     DataContext = new OrderTrackingViewModel(client, this, requestType)
                                 });
        }
    }
}