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
using System.IO;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;

using Microsoft.IdentityModel.Claims;

namespace Microsoft.IdentityModel.Samples.FederationScenario
{
    [ServiceBehavior( IncludeExceptionDetailInFaults = true )]
    public class BookStoreService : IBrowseBooks, IBuyBook
    {
        #region BookStoreService Constructor

        /// <summary>
        /// Sets up the BookStoreService by loading relevant Application Settings
        /// </summary>
        public BookStoreService()
        {
        }

        #endregion

        #region BrowseBooks() Implementation

        /// <summary>
        /// browseBooks() service call Implementation
        /// </summary>
        /// <returns>List of books available for purchase in the bookstore</returns>
        public List<string> BrowseBooks()
        {
            // Create an empty list of strings.
            List<string> books = new List<string>();
            try
            {
                // Create a StreamReader over the text file specified in app.config
                using ( StreamReader myStreamReader = new StreamReader( BookStoreServiceConfig.BookDB ) )
                {
                    string line = "";
                    // For each line in the text file...
                    while ( ( line = myStreamReader.ReadLine() ) != null )
                    {
                        // ...split the text from the text file...
                        string[] splitEntry = line.Split( '#' );
                        // ...format a string to return...
                        string formattedEntry = String.Format( "{0}.  {1},  {2},  ${3}",
                                                                splitEntry[0], // Book ID 
                                                                splitEntry[1], // Book Name
                                                                splitEntry[2], // Author
                                                                splitEntry[3] // Price
                                                              );
                        // ...and add it to the list 
                        books.Add( formattedEntry );
                    }
                    // Once we've finished reading the file, return the list of strings
                    return books;
                }
            }
            catch ( Exception e )
            {
                if ( ( e is System.IO.IOException ) ||
                     ( e is System.ArgumentNullException ) ||
                     ( e is System.ArgumentException ) )
                {
                    throw new FaultException<string>( String.Format( "BookStoreService: Error while loading books from DB ", e ) );
                }

                throw;
            }
        }

        #endregion

        #region BuyBook() Implementation

        /// <summary>
        /// This function extracts a Name claim from the provided ClaimsPrincipal and 
        /// returns the associated resource value.
        /// </summary>
        /// <param name="claimsPrincipal">The claims principal in which the Name claim should be found</param>
        /// <returns>The resource value associated </returns>
        static string GetNameIdentity( IClaimsPrincipal claimsPrincipal )
        {
            foreach ( IClaimsIdentity claimsId in claimsPrincipal.Identities )
            {
                if ( claimsId.Name != null )
                    return claimsId.Name;
            }
            // If there are no Name claims, return the Name of the Anonymous Windows Identity.
            return WindowsIdentity.GetAnonymous().Name;
        }

        /// <summary>
        /// Helper method to get book price from the Books Database.
        /// </summary>
        /// <param name="bookName">Name of the book intended for purchase</param>
        /// <returns>Price of the book with the given ID</returns>
        private static double GetBookPrice( string bookName )
        {
            using ( StreamReader myStreamReader = new StreamReader( BookStoreServiceConfig.BookDB ) )
            {
                string line = "";
                while ( ( line = myStreamReader.ReadLine() ) != null )
                {
                    string[] splitEntry = line.Split( '#' );
                    if ( splitEntry[1].Trim().Equals( bookName.Trim(), StringComparison.OrdinalIgnoreCase ) )
                    {
                        return Double.Parse( splitEntry[3] );
                    }
                }
                // invalid bookName - throw
                throw new FaultException( "Invalid Book Name " + bookName );
            }
        }

        /// <summary>
        /// Application level check for purchase limit claim value.
        /// </summary>
        /// <param name="bookName">Name of the book intended for purchase</param>
        /// <returns>True on success. False on failure.</returns>
        private static bool IsPurchaseLimitMet( ClaimsIdentity claimsId, string bookName )
        {
            // Get the price of the book being purchased...
            double bookPrice = GetBookPrice( bookName );

            // Iterate through the PurchaseLimit claims and verify that the Resource value is 
            // greater than or equal to the price of the book being purchased.
            foreach ( Claim claim in claimsId.Claims )
            {
                if ( claim.ClaimType == ScenarioConstants.PurchaseLimitClaim )
                {
                    double purchaseLimit = Double.Parse( claim.Value );
                    if ( purchaseLimit >= bookPrice )
                    {
                        // Book price is within the purchase limit, hence return true.
                        return true;
                    }
                }
            }

            // If no PurchaseLimit claim had a resource value that was greater than or equal
            // to the price of the book being purchased, return false.
            return false;
        }

        /// <summary>
        /// This function will purchase a book.
        /// </summary>
        /// <param name="bookName">Name of the book intended for purchase</param>
        /// <param name="emailAddress">Email of purchaser</param>
        /// <param name="shipAddress">Address of purchaser</param>
        /// <returns>status string</returns>
        /// <remarks>
        /// This function will not be called if the purchase is not authorized.
        /// Authorization is performed by CustomClaimsAuthorizationManager, which is invoked by WIF.
        /// </remarks>
        public string BuyBook( string bookName, string emailAddress, string shipAddress )
        {
            //
            // Check whether purchase limit is sufficient to buy the specified book
            //

            IClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as IClaimsPrincipal;

            if ( claimsPrincipal != null && claimsPrincipal.Identities != null )
            {
                // Need to iterate through claimsidentity collection and find a purchase limit claim that
                // meets the requirements.
                foreach ( ClaimsIdentity claimsId in claimsPrincipal.Identities )
                {
                    if ( IsPurchaseLimitMet( claimsId, bookName ) )
                    {
                        // Get the caller's name
                        string caller = GetNameIdentity( Thread.CurrentPrincipal as IClaimsPrincipal );
                        return String.Format( "{0}, the purchase of book {1} has been approved. The details of shipping date and confirmation receipt will be mailed to {2} shortly",
                                              caller, bookName, emailAddress );
                    }
                }
            }

            throw new FaultException( String.Format( "Purchase limit not sufficient to purchase '{0}'", bookName ) );
        }

        #endregion
    }
}
