<%@ WebHandler Language="C#" Class="FederationMetadataHandler" %>
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

using Microsoft.IdentityModel.Protocols.WSFederation.Metadata;

public class FederationMetadataHandler : IHttpHandler 
{
    
    public void ProcessRequest (HttpContext context) 
    {
        MetadataBase metadata = Common.GetFederationMetadata();
        MetadataSerializer serializer = new MetadataSerializer();
        serializer.WriteMetadata( context.Response.OutputStream, metadata );
        context.Response.ContentType = "text/xml";
    }
 
    public bool IsReusable {
        get 
        {
            return true;
        }
    }

}
