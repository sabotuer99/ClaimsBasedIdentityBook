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
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.IdentityModel.Claims;

// NOTE: If you change the class name "QuoteService" here, you must also update the reference to "QuoteService" in Web.config.
namespace ClaimsAwareService
{
    public class QuoteService : IQuote
    {
        const string QuotationClassClaimType = "http://schemas.microsoft.com/WindowsIdentityFramework/Samples/ClaimTypes/QuotationClass";
        static string[] _quotes = new string[]
            {
                "Placeholder for quote 1.",
                "Placeholder for quote 2.",
                "Placeholder for quote 3.",
                "Placeholder for quote 4.",
            };

        /// <summary>
        /// Select a quote based on the QuoteClass claim if present, else display authentication status.
        /// </summary>
        public string GetQuote()
        {
            IClaimsPrincipal principal = Thread.CurrentPrincipal as IClaimsPrincipal;
            if ( principal == null )
            {
                return "Not Federated. See this sample's readme documentation for more details.";
            }
            else
            {
                // check for a token from the STS
                IEnumerable<Claim> claimCollection = ( from c in principal.Identities[0].Claims
                                                       where c.ClaimType == QuotationClassClaimType
                                                       select c );
                if ( claimCollection.Count<Claim>() == 0 )
                {
                    return "You have no quote claims. See the readme documentation for more details on running this sample. If you are performing the steps in order and are on step 4, then this response is expected.";
                }
                else
                {
                    claimCollection = ( from c in principal.Identities[0].Claims
                                        where c.ClaimType == QuotationClassClaimType
                                        select c );
                    if ( claimCollection.Count<Claim>() == 0 )
                    {
                        return "A quote claim was not issued to you. See Step 6 in this sample's readme documentation for more details. NOTE: After you uncomment the proper line in the web.config file, you will need to perform Steps 7 and 8 again.";
                    }
                    else
                    {
                        int quoteClass = 0;
                        Int32.TryParse( claimCollection.First<Claim>().Value, out quoteClass );

                        quoteClass = Math.Max( 0, Math.Min( quoteClass, _quotes.Length-1 ) );
                        return _quotes[quoteClass];
                    }
                }
            }
        }
    }
}
