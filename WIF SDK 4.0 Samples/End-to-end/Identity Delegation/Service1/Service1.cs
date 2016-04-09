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
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.ServiceModel;
using System.Threading;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.Tokens;


namespace Service1
{
    /// <summary>
    /// Defines the Service Contract of the Echo service.
    /// </summary>
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string Echo( string value );
    }

    /// <summary>
    /// ServiceBehavior implementation of the Echo service.
    /// </summary>
    [ServiceBehavior]
    public class Service1 : IService1
    {
        static ChannelFactory<IService2Channel> service2CF;
        static object service2CFLock = new object();

        #region IService1 Members

        /// <summary>
        /// Echo service that calls the backend service acting as the client. A SAML token is obtained from the STS 
        /// Acting on behalf of the client and then the SAML token is presented to the backend service.
        /// </summary>
        /// <param name="value">The client supplied value in the call to this service.</param>
        /// <returns>A string that the client provided.</returns>
        public string Echo( string value )
        {
            IClaimsIdentity identity = (IClaimsIdentity)( (IClaimsPrincipal)Thread.CurrentPrincipal ).Identity;
            Console.WriteLine( "Caller's name: " + identity.Name );
            foreach ( Claim claim in identity.Claims )
            {
                Console.WriteLine( "ClaimType  : " + claim.ClaimType );
                Console.WriteLine( "ClaimValue : " + claim.Value );
                Console.WriteLine();
            }

            string retval = null;

            lock ( service2CFLock )
            {
                if ( service2CF == null )
                {
                    // Create a ChannelFactory to talk to the Service.
                    service2CF = new ChannelFactory<IService2Channel>( "CustomBinding_IService2" );
                    service2CF.ConfigureChannelFactory();
                }
            }

            // Get the client token. Here we are looking for a SAML token as we know from our bindings that the 
            // client should have authenticated using a SAML 1.1 token.
            SecurityToken clientToken = null;

            IClaimsPrincipal claimsPrincipal = Thread.CurrentPrincipal as IClaimsPrincipal;

            if ( claimsPrincipal != null )
            {
                foreach ( IClaimsIdentity claimsIdentity in claimsPrincipal.Identities )
                {
                    if ( claimsIdentity.BootstrapToken is SamlSecurityToken )
                    {
                        clientToken = claimsIdentity.BootstrapToken;
                        break;
                    }
                }
            }

            // We couldn't find the client token. This would mean that either IdentityModel's SecurityTokenHandler 
            // was not involved in validating this token or IdentityModelServiceAuthorizationManager was not
            // plugged into the ServiceHost.
            if ( clientToken == null )
            {
                throw new InvalidOperationException(
                    "Unable to find client token. Check if your ServiceHost is correctly configured." );
            }

            // Create a channel that uses the Client token as the ActAs token. The Channel will use
            // the configured client token when an ActAs token is requested.
            //
            // Note: A new channel must be created for each call.
            IService2Channel channel = service2CF.CreateChannelActingAs<IService2Channel>( clientToken );

            try
            {
                // Call the backend service.
                retval = channel.ComputeResponse( value );
                channel.Close();
            }
            catch ( CommunicationException e )
            {
                Console.WriteLine( e.Message );
                Console.WriteLine( e.StackTrace );
                Exception ex = e.InnerException;
                while ( ex != null )
                {
                    Console.WriteLine( "===========================" );
                    Console.WriteLine( ex.Message );
                    Console.WriteLine( ex.StackTrace );
                    ex = ex.InnerException;
                }
                channel.Abort();
                retval = "Unable to compute the return value";
            }
            catch ( TimeoutException )
            {
                Console.WriteLine( "Timed out..." );
                channel.Abort();
                retval = "Unable to compute the return value";
            }
            catch ( Exception e )
            {
                Console.WriteLine( "An unexpected exception occured." );
                Console.WriteLine( e.StackTrace );
                channel.Abort();
                retval = "Unable to compute the return value";
            }

            return retval;
        }

        #endregion
    }
}
