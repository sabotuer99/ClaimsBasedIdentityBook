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
    using System.ServiceModel.Security;
    using AOrder.OrderTracking.Contracts.Data;

    [ServiceContract]
    public interface IOrderTrackingService
    {
        [OperationContract]
        [FaultContract(typeof(SecurityAccessDeniedException))]
        Order[] GetAllOrders();

        [OperationContract]
        [FaultContract(typeof(SecurityAccessDeniedException))]
        Order[] GetOrdersFromMyOrganization();
    }
}