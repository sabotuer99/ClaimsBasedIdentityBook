//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.PhoneClient
{
    using System;
    using System.Net;
    using System.Windows;
    using AOrder.OrderTracking.Contracts.Data;
    using Microsoft.Phone.Controls;
    using Microsoft.Phone.Reactive;
    using SL.Phone.Federation.Controls;

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            this.InitializeComponent();
        }

        public IObservable<Order[]> GetOrders()
        {
            var stsEndpoint = "https://localhost/Litware.SimulatedIssuer.9/Issue.svc";
            var acsEndpoint = "https://aorderphone-dev.accesscontrol.windows.net/v2/OAuth2-13";

            var serviceEnpoint = "https://localhost/a-Order.OrderTracking.Services.9";
            var ordersServiceUri = new Uri(serviceEnpoint + "/orders/frommyorganization");

            return
                HttpClient.RequestTo(ordersServiceUri)
                    .AddAuthorizationHeader(stsEndpoint, acsEndpoint, serviceEnpoint)
                    .SelectMany(request => { return request.Get<Order[]>(); },
                                (request, orders) => { return orders; })
                    .ObserveOnDispatcher()
                    .Catch((WebException ex) =>
                               {
                                   var message = GetMessageForException(ex);
                                   MessageBox.Show(message);
                                   return Observable.Return(default(Order[]));
                               });
        }

        public IObservable<Order[]> GetOrdersWithToken(string swtToken)
        {
            var serviceEnpoint = "https://localhost/a-Order.OrderTracking.Services.9";
            var ordersServiceUri = new Uri(serviceEnpoint + "/orders/frommyorganization");

            return
                HttpClient.RequestTo(ordersServiceUri)
                    .AddAuthorizationHeader(swtToken)
                    .SelectMany(request => { return request.Get<Order[]>(); },
                                (request, orders) => { return orders; });
        }

        private static string GetMessageForException(WebException ex)
        {
            var response = ex.Response as HttpWebResponse;
            if (response != null)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return
                        string.Format(
                            "{0}: Can't connect to the server. Try Installing the samples SSL root certificate by browsing from the phone's IE the page 'http://localhost/Litware.SimulatedIssuer.9/RootCert/' and check internet connectivity.",
                            response.StatusCode);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return string.Format("{0}: {1}.", response.StatusCode, response.StatusDescription);
                }
            }

            if (ex.InnerException != null)
            {
                return ex.InnerException.Message;
            }

            return ex.Message;
        }

        private void OnGetOrdersActiveButtonClicked(object sender, RoutedEventArgs e)
        {
            this.UpdateOrders(default(Order[]));

            try
            {
                this.GetOrders()
                    .ObserveOnDispatcher()
                    .Subscribe(this.UpdateOrders);
            }
            catch (WebException ex)
            {
                var message = GetMessageForException(ex);
                MessageBox.Show(message);
            }
        }

        private void OnGetMyOrdersPassiveButtonClicked(object sender, RoutedEventArgs e)
        {
            this.UpdateOrders(default(Order[]));

            this.SignInControl.Visibility = Visibility.Visible;
            this.GetMyOrdersPassiveButton.Visibility = Visibility.Collapsed;

            var acsJsonEndpoint = "https://aorderphone-dev.accesscontrol.windows.net/v2/metadata/IdentityProviders.js?protocol=wsfederation&realm=https%3A%2F%2Flocalhost%2Fa-Order.OrderTracking.Services.9&context=&version=1.0";
            this.SignInControl.RequestSecurityTokenResponseCompleted += this.SignInControl_RequestSecurityTokenResponseCompleted;
            this.SignInControl.GetSecurityToken(new Uri(acsJsonEndpoint));
        }

        private void SignInControl_RequestSecurityTokenResponseCompleted(object sender, RequestSecurityTokenResponseCompletedEventArgs e)
        {
            this.GetOrdersWithToken(e.RequestSecurityTokenResponse.TokenString)
                .ObserveOnDispatcher()
                .Catch((WebException ex) =>
                           {
                               var message = GetMessageForException(ex);
                               MessageBox.Show(message);
                               return Observable.Return(default(Order[]));
                           })
                .Subscribe(orders =>
                               {
                                   this.SignInControl.Visibility = Visibility.Collapsed;
                                   this.GetMyOrdersPassiveButton.Visibility = Visibility.Visible;
                                   this.UpdateOrders(orders);
                               });
        }

        private void UpdateOrders(Order[] orders)
        {
            this.OrderItems.ItemsSource = orders;
        }
    }
}