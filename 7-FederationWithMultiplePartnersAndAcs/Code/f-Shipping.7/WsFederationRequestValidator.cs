//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping
{
    using System;
    using System.Web;
    using System.Web.Util;
    using Microsoft.IdentityModel.Protocols.WSFederation;

    public class WsFederationRequestValidator : RequestValidator
    {
        protected override bool IsValidRequestString(HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex)
        {
            validationFailureIndex = 0;
            if (requestValidationSource == RequestValidationSource.Form &&
                collectionKey.Equals(WSFederationConstants.Parameters.Result, StringComparison.Ordinal))
            {
                if (WSFederationMessage.CreateFromFormPost(context.Request) as SignInResponseMessage != null)
                {
                    return true;
                }
            }

            return base.IsValidRequestString(context, 
                                             value, 
                                             requestValidationSource, 
                                             collectionKey, 
                                             out validationFailureIndex);
        }
    }
}