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
    public enum IdentityKeyTypes
    {
        // X509Certificate service identity is not supported by ACSv2 yet, although database scheme allows it.
        X509Certificate, 

        Password, 

        // Used for supporting Wrap Profile 5.2 - SWT assertion. 
        // Instead of sending username and password, clients can send a signed SWT assertion in a request. 
        Symmetric
    }
}