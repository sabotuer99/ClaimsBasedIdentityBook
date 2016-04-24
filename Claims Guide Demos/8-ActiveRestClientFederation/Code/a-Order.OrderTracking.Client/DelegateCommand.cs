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
    using System.Windows.Input;

    public sealed class DelegateCommand : ICommand
    {
        private readonly Action executeMethod;

        private bool canExecute;

        public DelegateCommand(Action executeMethod)
        {
            if (executeMethod == null)
            {
                throw new ArgumentNullException("executeMethod");
            }

            this.executeMethod = executeMethod;
            this.CanExecute = true;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute
        {
            get { return this.canExecute; }

            set
            {
                if (this.canExecute == value)
                {
                    return;
                }

                this.canExecute = value;
                this.OnCanExecuteChanged();
            }
        }

        public void Execute(object parameter)
        {
            this.executeMethod();
        }

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute;
        }

        private void OnCanExecuteChanged()
        {
            EventHandler handler = this.CanExecuteChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }
    }
}