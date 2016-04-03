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
    using System.ComponentModel;
    using System.Windows.Input;

    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly OrderTrackingController controller;
        private readonly DelegateCommand loginAsManagerCommand;
        private readonly DelegateCommand loginAsSalesCommand;
        private readonly DelegateCommand retrieveOrganizationOrdersCommand;
        private readonly DelegateCommand retrieveAllOrdersCommand;
        private string password;
        private string userName;

        public LoginViewModel(OrderTrackingController controller)
        {
            this.controller = controller;
            this.retrieveOrganizationOrdersCommand = new DelegateCommand(this.RetrieveOrganizationOrders);
            this.loginAsManagerCommand = new DelegateCommand(this.LoginAsManager);
            this.loginAsSalesCommand = new DelegateCommand(this.LoginAsSales);
            this.retrieveAllOrdersCommand = new DelegateCommand(this.RetrieveAllOrders);
            this.LoginAsManagerChecked = true;
            this.LoginAsManager();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool LoginAsManagerChecked { get; set; }

        public ICommand LoginAsManagerCommand
        {
            get { return this.loginAsManagerCommand; }
        }

        public bool LoginAsSalesChecked { get; set; }

        public ICommand LoginAsSalesCommand
        {
            get { return this.loginAsSalesCommand; }
        }

        public ICommand RetrieveOrganizationOrdersCommand
        {
            get { return this.retrieveOrganizationOrdersCommand; }
        }

        public ICommand RetrieveAllOrdersCommand
        {
            get { return this.retrieveAllOrdersCommand; }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                this.password = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Password"));
                }
            }
        }

        public string UserName
        {
            get { return this.userName; }
            set
            {
                this.userName = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("UserName"));
                }
            }
        }

        public void RetrieveOrganizationOrders()
        {
            this.Login(this.retrieveOrganizationOrdersCommand, RequestType.OrganizationOrders);
        }

        public void RetrieveAllOrders()
        {
            this.Login(this.retrieveAllOrdersCommand, RequestType.AllOrders);
        }

        private void Login(DelegateCommand command, RequestType requestType)
        {
            command.CanExecute = false;

            if (!this.controller.LoginUser(this.UserName, this.Password, requestType))
            {
                command.CanExecute = true;
            }
        }

        private void LoginAsManager()
        {
            this.LoginAsSalesChecked = false;
            this.UserName = "LITWARE\\fred";
            this.Password = "ThisPasswordIsNotChecked";
        }

        private void LoginAsSales()
        {
            this.LoginAsManagerChecked = false;
            this.UserName = "LITWARE\\rick";
            this.Password = "ThisPasswordIsNotChecked";
        }
    }
}