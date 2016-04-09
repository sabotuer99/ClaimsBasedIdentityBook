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

public partial class _Default : System.Web.UI.Page 
{
    protected string CurrentQuote { get { return _quote; } }
    string _quote = "**Service failure**";

    protected void Page_Load(object sender, EventArgs e)
    {
        QuoteService.QuoteClient client = new QuoteService.QuoteClient();
        try
        {
            _quote = client.GetQuote();
            client.Close();
        }
        finally
        {
            if ( client.State == System.ServiceModel.CommunicationState.Faulted )
            {
                client.Abort();
            }
        }
    }
}
