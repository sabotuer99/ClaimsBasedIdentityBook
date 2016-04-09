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
using System.Collections.ObjectModel;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSFederation;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

namespace WFE
{
    public partial class _Default : System.Web.UI.Page
    {
        public const string CachedToken = "WFE_CachedToken";

        /// <summary>
        /// Checks whether or not user is authenticated.
        /// </summary>
        private bool IsAuthenticatedUser
        {
            get { return Page.User != null && Page.User.Identity != null && Page.User.Identity.IsAuthenticated; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                return;
            }

            string value = Request["value"];
            if (String.IsNullOrEmpty(value))
            {
                value = "Default Input";
            }

            // Get the Bootstrap Token
            SecurityToken callerToken = null;

            IClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as IClaimsPrincipal;

            // We expect only one identity, which will contain the bootstrap token.            
            if ( claimsPrincipal != null && claimsPrincipal.Identities.Count == 1 )
            {
                callerToken = claimsPrincipal.Identities[0].BootstrapToken;
            }

            if ( callerToken == null )
            {
                // We lost the session state but the user still has the federated ticket
                // Let's sign the user off and start again               
                FederatedAuthentication.SessionAuthenticationModule.DeleteSessionTokenCookie();
                Response.Redirect( Request.Url.AbsoluteUri );
                return;
            }

            // Get the channel factory to the backend service from the application state
            ChannelFactory<IService2Channel> factory = (ChannelFactory<IService2Channel>)Application[Global.CachedChannelFactory];

            // Create and setup channel to talk to the backend service
            IService2Channel channel;
 
            // Setup the ActAs to point to the caller's token so that we perform a delegated call to the backend service
            // on behalf of the original caller.
            //
            // Note: A new channel must be created for each call.
            channel = factory.CreateChannelActingAs<IService2Channel>(callerToken);

            string retval = null;

            // Call the backend service and handle the possible exceptions
            try
            {
                retval = channel.ComputeResponse(value);
                channel.Close();
            }
            catch (CommunicationException exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(exception.Message);
                sb.AppendLine(exception.StackTrace);
                Exception ex = exception.InnerException;
                while (ex != null)
                {
                    sb.AppendLine("===========================");
                    sb.AppendLine(ex.Message);
                    sb.AppendLine(ex.StackTrace);
                    ex = ex.InnerException;
                }
                channel.Abort();
                retval = sb.ToString(); ;
            }
            catch (TimeoutException)
            {
                channel.Abort();
                retval = "Timed out...";
            }
            catch (Exception exception)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("An unexpected exception occured.");
                sb.AppendLine(exception.StackTrace);
                channel.Abort();
                retval = sb.ToString();
            }

            // Make the result visible on the page
            lblResult.Text = Server.HtmlEncode(retval);
        }

        static string GetPathAndQuery(WSFederationMessage request)
        {
            StringBuilder strb = new StringBuilder(128);
            using (StringWriter writer = new StringWriter(strb, CultureInfo.InvariantCulture))
            {
                request.Write(writer);
                return strb.ToString();
            }
        }
    }
}
