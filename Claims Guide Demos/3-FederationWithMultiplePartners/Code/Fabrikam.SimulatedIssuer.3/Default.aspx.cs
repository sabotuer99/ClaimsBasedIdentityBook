//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Litware.SimulatedIssuer
{
    using System;
    using System.Globalization;
    using System.Web;
    using System.Web.UI;
    using Fabrikam.SimulatedIssuer;
    using Microsoft.IdentityModel.Protocols.WSFederation;
    using Microsoft.IdentityModel.SecurityTokenService;
    using Microsoft.IdentityModel.Web;

    public partial class Default : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            string action = this.Request.QueryString[WSFederationConstants.Parameters.Action];

            if (action == WSFederationConstants.Actions.SignIn)
            {
                // Process signin request.
                var requestMessage = (SignInRequestMessage)WSFederationMessage.CreateFromUri(this.Request.Url);
                if (this.User != null && this.User.Identity.IsAuthenticated)
                {
                    SecurityTokenService sts = new IdentityProviderSecurityTokenService(IdentityProviderSecurityTokenServiceConfiguration.Current);
                    SignInResponseMessage responseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, this.User, sts);
                    FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, this.Response);
                }
                else
                {
                    throw new UnauthorizedAccessException();
                }
            }
            else if (action == WSFederationConstants.Actions.SignOut || action == WSFederationConstants.Actions.SignOutCleanup)
            {
                // Process signout request.
                WSFederationMessage requestMessage = WSFederationMessage.CreateFromUri(this.Request.Url);
                FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(requestMessage, this.User, null, this.Response);
                
                // Simulate what happens when you sign out of WIF to send a response that everything was Ok
                var signOutImage = new byte[]
                                       {
                                           71, 73, 70, 56, 57, 97, 17, 0, 13, 0, 162, 0, 0, 255, 255, 255, 
                                           169, 240, 169, 125, 232, 125, 82, 224, 82, 38, 216, 38, 0, 0, 0, 0, 
                                           0, 0, 0, 0, 0, 33, 249, 4, 5, 0, 0, 5, 0, 44, 0, 0, 
                                           0, 0, 17, 0, 13, 0, 0, 8, 84, 0, 11, 8, 28, 72, 112, 32, 
                                           128, 131, 5, 19, 22, 56, 24, 128, 64, 0, 0, 10, 13, 54, 116, 8, 
                                           49, 226, 193, 1, 4, 6, 32, 36, 88, 113, 97, 0, 140, 26, 11, 30, 
                                           68, 8, 64, 0, 129, 140, 29, 5, 2, 56, 73, 209, 36, 202, 132, 37, 
                                           79, 14, 112, 73, 81, 97, 76, 150, 53, 109, 210, 36, 32, 32, 37, 76, 
                                           151, 33, 35, 26, 20, 16, 84, 168, 65, 159, 9, 3, 2, 0, 59
                                       };
                
                this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                this.Response.ClearContent();
                this.Response.ContentType = "image/gif";
                this.Response.BinaryWrite(signOutImage);
            }
            else
            {
                throw new InvalidOperationException(
                    String.Format(
                        CultureInfo.InvariantCulture, 
                        "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}' or '{3}'.", 
                        String.IsNullOrEmpty(action) ? "<EMPTY>" : action, 
                        WSFederationConstants.Parameters.Action, 
                        WSFederationConstants.Actions.SignIn, 
                        WSFederationConstants.Actions.SignOut));
            }

            base.OnLoad(e);
        }
    }
}