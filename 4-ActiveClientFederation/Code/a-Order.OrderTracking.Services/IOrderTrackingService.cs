//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


// Microsoft patterns & practices: Guide to Claims Based Authentication Copyright � Microsoft Corporation.  All Rights Reserved. This code released under the terms of the Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)


namespace AOrder.OrderTracking.Services
{
    using System.ServiceModel;
    using AOrder.OrderTracking.Contracts.Data;

    [ServiceContract]
    public interface IOrderTrackingService
    {
        [OperationContract]
        Order[] GetOrdersFromMyOrganization();

        [OperationContract]
        Order[] GetAllOrders();
    }
}