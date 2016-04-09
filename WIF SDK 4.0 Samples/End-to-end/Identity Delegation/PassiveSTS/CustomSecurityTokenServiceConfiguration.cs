//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

using System.Web;
using Microsoft.IdentityModel.Configuration;
using STS;

namespace PassiveSTS
{
    /// <summary>
    /// Custom SecurityTokenServiceConfiguration of the PassiveSTS. 
    /// </summary>
    public class CustomSecurityTokenServiceConfiguration : SecurityTokenServiceConfiguration
    {
        static readonly object syncRoot = new object();
        static string CustomSecurityTokenServiceConfigurationKey = "CustomSecurityTokenServiceConfigurationKey";

        public static CustomSecurityTokenServiceConfiguration Current
        {
            get
            {
                HttpApplicationState httpAppState = HttpContext.Current.Application;

                CustomSecurityTokenServiceConfiguration myConfiguration = httpAppState.Get( CustomSecurityTokenServiceConfigurationKey ) as CustomSecurityTokenServiceConfiguration;

                if ( myConfiguration != null )
                {
                    return myConfiguration;
                }

                lock ( syncRoot )
                {
                    myConfiguration = httpAppState.Get( CustomSecurityTokenServiceConfigurationKey ) as CustomSecurityTokenServiceConfiguration;

                    if ( myConfiguration == null )
                    {
                        myConfiguration = new CustomSecurityTokenServiceConfiguration();
                        httpAppState.Add( CustomSecurityTokenServiceConfigurationKey, myConfiguration );
                    }

                    return myConfiguration;
                }
            }
        }

        /// <summary>
        /// Creates an instance of CustomSecurityTokenServiceConfiguration.
        /// </summary>
        public CustomSecurityTokenServiceConfiguration()
            : base( "http://PassiveSTS" )
        {
            SecurityTokenService = typeof( CustomSecurityTokenService );
        }
    }
}
