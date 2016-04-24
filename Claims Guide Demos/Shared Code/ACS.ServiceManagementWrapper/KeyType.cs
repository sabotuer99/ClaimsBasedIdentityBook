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
    public enum KeyType
    {
        // Recommend not to share symmetric signing key across RPs but configure it on RP instead. 
        Symmetric, 

        X509Certificate, 

        ApplicationKey
    }
}