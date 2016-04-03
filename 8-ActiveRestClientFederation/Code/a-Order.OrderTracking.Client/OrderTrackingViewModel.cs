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
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using AOrder.OrderTracking.Client.OrderTrackingService;
    using AOrder.OrderTracking.Contracts.Data;

    public class OrderTrackingViewModel
    {
        private readonly Func<IEnumerable<Order>> action;
        private readonly OrderTrackingServiceClient client;
        private readonly DelegateCommand logoutCommand;
        private readonly OrderTrackingController orderTrackingController;
        private readonly DelegateCommand refreshCommand;

        public OrderTrackingViewModel(OrderTrackingServiceClient client, OrderTrackingController orderTrackingController, RequestType requestType)
        {
            this.client = client;
            this.orderTrackingController = orderTrackingController;
            this.refreshCommand = new DelegateCommand(this.RefreshOrders);
            this.logoutCommand = new DelegateCommand(this.Logout);
            if (requestType == RequestType.OrganizationOrders)
            {
                this.action = new Func<IEnumerable<Order>>(this.client.GetOrdersFromMyOrganization);
            }
            else
            {
                this.action = new Func<IEnumerable<Order>>(this.client.GetAllOrders);
            }
            this.RefreshOrders();
        }

        public bool LitwareSamlTokenAvailable { get; private set; }

        public ICommand LogoutCommand
        {
            get { return this.logoutCommand; }
        }

        public ObservableCollection<Order> Orders { get; private set; }

        public ICommand RefreshCommand
        {
            get { return this.refreshCommand; }
        }

        public void Logout()
        {
            this.orderTrackingController.LogoutUser();
        }

        public void RefreshOrders()
        {
            this.refreshCommand.CanExecute = false;
            this.Orders = new ObservableCollection<Order>(this.action());
            this.refreshCommand.CanExecute = true;
        }
    }
}