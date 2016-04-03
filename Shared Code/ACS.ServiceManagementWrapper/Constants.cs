//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace ACS.ServiceManagementWrapper
{
    public static class Constants
    {
        public const string AcsHostName = "accesscontrol.windows.net";
        public const string ManagementServiceHead = "v2/mgmt/service/";
        public const string MetadataImportHead = "v2/mgmt/service/importFederationMetadata/importIdentityProvider";

        // 1 hour = 3600 seconds
        public const int RelyingPartyTokenLifetime = 3600;
    }
}