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
    using System.Windows.Input;

    public class LoginViewModel
    {
        private readonly OrderTrackingController controller;

        private readonly DelegateCommand loginCommand;

        public LoginViewModel(OrderTrackingController controller)
        {
            this.controller = controller;
            this.loginCommand = new DelegateCommand(this.ProceedWithLogin);
        }

        public ICommand LoginCommand
        {
            get { return this.loginCommand; }
        }

        public void ProceedWithLogin()
        {
            this.loginCommand.CanExecute = false;

            if (!this.controller.LoginUser())
            {
                this.loginCommand.CanExecute = true;
            }
        }
    }
}