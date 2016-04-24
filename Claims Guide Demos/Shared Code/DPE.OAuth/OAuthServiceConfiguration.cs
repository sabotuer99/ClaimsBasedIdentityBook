//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Microsoft.Samples.DPE.OAuth
{
    using System;
    using System.Configuration;
    using Microsoft.IdentityModel.Configuration;
    using Microsoft.Samples.DPE.OAuth.Tokens;

    public class OAuthServiceConfiguration : ServiceConfiguration
    {
        public OAuthServiceConfiguration()
            : this("OAuth")
        {
        }

        public OAuthServiceConfiguration(string serviceName)
            : base(serviceName)
        {            
        }               
    }
}