//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AOrder.OrderTracking.Contracts
{
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using AOrder.OrderTracking.Contracts.Data;

    [ServiceContract]
    public interface IOrderTrackingService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/all", Method = "GET")]
        Order[] GetAllOrders();

        [OperationContract]
        [WebInvoke(UriTemplate = "/frommyorganization", Method = "GET")]
        Order[] GetOrdersFromMyOrganization();
    }
}