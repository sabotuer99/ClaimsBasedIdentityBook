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


namespace PassiveFlowSTS
{
    public class MySTSConfiguration : SecurityTokenServiceConfiguration
    {
        static readonly object syncRoot = new object();
        static string MySTSConfigurationKey = "MySTSConfigurationKey";

        public static MySTSConfiguration Current
        {
            get
            {
                HttpApplicationState httpAppState = HttpContext.Current.Application;

                MySTSConfiguration myConfiguration = httpAppState.Get( MySTSConfigurationKey ) as MySTSConfiguration;

                if ( myConfiguration != null )
                {
                    return myConfiguration;
                }

                lock ( syncRoot )
                {
                    myConfiguration = httpAppState.Get( MySTSConfigurationKey ) as MySTSConfiguration;

                    if ( myConfiguration == null )
                    {
                        myConfiguration = new MySTSConfiguration();
                        httpAppState.Add( MySTSConfigurationKey, myConfiguration );
                    }

                    return myConfiguration;
                }
            }
        }

        public MySTSConfiguration()
            : base( "STSForms" )
        {
            SecurityTokenService = typeof(MySecurityTokenService);
        }
    }
}
