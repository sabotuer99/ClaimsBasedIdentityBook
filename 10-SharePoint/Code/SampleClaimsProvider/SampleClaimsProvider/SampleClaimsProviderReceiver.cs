//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Administration.Claims;
using Microsoft.SharePoint.Diagnostics;

namespace SampleClaimsProvider
{
    public class SampleClaimsProviderReceiver : SPClaimProviderFeatureReceiver
    {
        private void ExecBaseFeatureActivated(Microsoft.SharePoint.SPFeatureReceiverProperties properties)
        {
            base.FeatureActivated(properties);
        }
 
        public override string ClaimProviderAssembly
        {
            get 
            {
                return typeof(SampleClaimsProvider).Assembly.FullName;
            }
        }
 
        public override string ClaimProviderDescription
        {
            get 
            { 
                return "Sample claims provider"; 
            }
        }
 
        public override string ClaimProviderDisplayName
        {
            get 
            {
                return SampleClaimsProvider.ProviderDisplayName; 
            }
        }
 
        public override string ClaimProviderType
        {
            get 
            {
                return typeof(SampleClaimsProvider).FullName; 
            }
        }
 
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            ExecBaseFeatureActivated(properties);
        }
    }
}
