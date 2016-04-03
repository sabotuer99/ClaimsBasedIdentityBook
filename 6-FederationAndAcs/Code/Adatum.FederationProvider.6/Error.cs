//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adatum.FederationProvider
{
    public class Error
    {
        public string errorCode { get; set; }

        public string errorMessage { get; set; }
    }

    public class ErrorDetails
    {

        public string context { get; set; }

        public int httpReturnCode { get; set; }

        public string identityProvider { get; set; }

        public Error[] errors { get; set; }

    }
}