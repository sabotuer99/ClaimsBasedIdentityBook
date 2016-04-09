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

using System;
using System.Web;
using System.Web.Util;

using Microsoft.IdentityModel.Protocols.WSFederation;

/// <summary>
/// Validator checks for a POST containing a form that looks like a potential RequestSecurityTokenResponse from CardSpace.
/// </summary>

public class SampleCardSpaceRequestValidator : RequestValidator
{
    // Note: This matches the name of the form input in Default.aspx
    const string TokenXmlInputName = "tokenXml";

    protected override bool IsValidRequestString( HttpContext context, string value, RequestValidationSource requestValidationSource, string collectionKey, out int validationFailureIndex )
    {
        validationFailureIndex = 0;

        if ( requestValidationSource == RequestValidationSource.Form && collectionKey.Equals( TokenXmlInputName, StringComparison.Ordinal ) )
        {
            return true;
        }

        return base.IsValidRequestString( context, value, requestValidationSource, collectionKey, out validationFailureIndex );
    }

}
