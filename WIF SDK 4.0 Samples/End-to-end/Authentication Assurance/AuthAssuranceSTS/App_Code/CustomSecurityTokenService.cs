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
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Xml;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Configuration;
using Microsoft.IdentityModel.Protocols.WSTrust;
using Microsoft.IdentityModel.SecurityTokenService;
using Microsoft.IdentityModel.Tokens;

using ClaimTypes = Microsoft.IdentityModel.Claims.ClaimTypes;

namespace AuthAssuranceSTS
{
    /// <summary>
    /// Extends the Microsoft.IdentityModel.Services.SecurityTokenService class to provide
    /// necessary information required to issue a security token. Such as encryption credentials to 
    /// encrypt the issued token, signing credentials to sign the issued token, claims that STS  
    /// wants to issue for the token request.
    /// </summary>
    public class CustomSecurityTokenService : SecurityTokenService
    {
        //
        // For this localhost demo app we use the same certificate for both signing and encrypting.
        // In production code you would want to use separate certificates.
        //
        static readonly string SigningCertificateName = "CN=localhost";
        static readonly string EncryptingCertificateName = "CN=localhost";

        //
        // The expected AppliesTo address is to be identical to the Realm value that is configured in the RP's config.
        // The STS CardSignIn page is also added to process information card sign-in requests.
        //
        static readonly string[] _appliesToAddresses = new string[] { "https://localhost/AuthAssuranceRP/",
                                                                      "https://localhost/AuthAssuranceSTS/CardSignIn.aspx" };

        SigningCredentials _signingCreds;
        EncryptingCredentials _encryptingCreds;

        public CustomSecurityTokenService( SecurityTokenServiceConfiguration configuration )
            : base( configuration )
        {
            //
            // Setup the certificate our STS is going to use to sign the issued tokens
            //
            _signingCreds = new X509SigningCredentials(
                                   CertificateUtil.GetCertificate(
                                      StoreName.My, StoreLocation.LocalMachine, SigningCertificateName ) );

            //
            // In this sample, we only support a single RP identity represented by the _encryptingCreds.
            // However, this encrypting credential may differ based on the RP identity requesting a token in
            // a production STS. See the comments in GetScope() for more details.
            //
            _encryptingCreds = new X509EncryptingCredentials(
                                      CertificateUtil.GetCertificate(
                                          StoreName.My, StoreLocation.LocalMachine, EncryptingCertificateName ) );
        }

        /// <summary>
        /// This method returns the configuration for the token issuance request. The configuration
        /// is represented by the Scope class. In our case, we are only capable of issuing a token to a
        /// single RP identity represented by the _encryptingCreds field.
        /// </summary>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST</param>
        /// <returns></returns>
        protected override Scope GetScope( IClaimsPrincipal principal, RequestSecurityToken request )
        {
            //
            // Get the appliesTo address
            //
            ValidateAppliesTo( request.AppliesTo );

            //
            // Create the scope using the request AppliesTo address and the RP identity
            //
            Scope scope = new Scope( request.AppliesTo.Uri.AbsoluteUri, _signingCreds );

            //
            // Setting the encrypting credentials
            // Note: In this sample app only a single RP identity is accepted: localhost.
            // If you have multiple RPs for an STS you would select a certificate that is specific to 
            // the RP that requests the token.
            //
            scope.EncryptingCredentials = _encryptingCreds;

            //
            // Set the ReplyTo address as the address where the issued token can be presented. 
            // In WS-Federation passive protocol this is the address to which the
            // client will be redirected to by the SecurityTokenSerivce.
            //
            scope.ReplyToAddress = scope.AppliesToAddress;

            return scope;
        }

        /// <summary>
        /// Validates the appliesTo and throws an exception if the appliesTo is null or appliesTo contains some unexpected address.  
        /// <param name="appliesTo">EndpointAddress that indicates the appliesTo parameter.</param> 
        /// </summary>
        void ValidateAppliesTo( EndpointAddress appliesTo )
        {
            if ( appliesTo == null )
            {
                throw new InvalidRequestException( "The appliesTo is null." );
            }

            foreach ( string addressExpected in _appliesToAddresses )
            {
                if ( appliesTo.Uri.ToString().StartsWith( ( new Uri( addressExpected ) ).ToString() ) )
                {
                    return;
                }
            }

            throw new InvalidRequestException( String.Format( "The relying party address {0} is not valid.", appliesTo.Uri.AbsoluteUri ) );
        }

        static string GetAuthStrengthClaim( IClaimsIdentity claimsIdentity )
        {
            //
            // Search for an AuthenticationMethod Claim.
            //
            IEnumerable<Claim> claimCollection = ( from c in claimsIdentity.Claims
                                                   where c.ClaimType == Microsoft.IdentityModel.Claims.ClaimTypes.AuthenticationMethod
                                                   select c );
            if ( claimCollection.Count<Claim>() > 0 )
            {
                return claimCollection.First<Claim>().Value;
            }

            return String.Empty;
        }

        /// <summary>
        /// Override this method to return the content of the issued token. The content is represented as a set of
        /// IClaimIdentity instances. Each instance corresponds to a single issued token.
        /// </summary>
        /// <param name="scope">The scope that was previously returned by GetScope</param>
        /// <param name="principal">The caller's principal</param>
        /// <param name="request">The incoming RST, actual token request</param> 
        /// <returns>A collection of ClaimsIdentity that contain claims so that they can be sent back inside the issued token</returns>
        protected override IClaimsIdentity GetOutputClaimsIdentity( IClaimsPrincipal principal, RequestSecurityToken request, Scope scope )
        {
            IClaimsIdentity callerIdentity = principal.Identity as IClaimsIdentity;

            List<Claim> claims = new List<Claim>();

            Claim nameValue = new Claim( ClaimTypes.Name, callerIdentity.Name );
            claims.Add( nameValue );

            //
            // Find the authentication method claim.
            //
            string authenticationMethod = GetAuthStrengthClaim( callerIdentity );

            //
            // If no authentication method claim was found, use the AuthenticationType that was set on the caller's identity.
            //
            if ( String.IsNullOrEmpty( authenticationMethod ) )
            {
                authenticationMethod = callerIdentity.AuthenticationType;
            }

            //
            // Add an authentication method claim if found.
            //
            if ( !String.IsNullOrEmpty( authenticationMethod ) )
            {
                //
                // Propagate the original authentication method claim. This may be used at the RP to verify authentication strength.
                //
                claims.Add( new Claim( ClaimTypes.AuthenticationMethod, authenticationMethod ) );
            }

            //
            // Add an authentication instant claim.
            //
            claims.Add( new Claim( ClaimTypes.AuthenticationInstant, XmlConvert.ToString( DateTime.Now.ToUniversalTime(), "yyyy-MM-ddTHH:mm:ss.fffZ" ) ) );

            //
            // We send back high assurance claims only when the authentication method is X509.
            // The authenticationMethod value dictates whether high assurance claims need to be sent or not.
            //
            if ( authenticationMethod == AuthenticationMethods.X509 )
            {
                //
                // Date of Birth claim
                //
                Claim dobClaim = new Claim( ClaimTypes.DateOfBirth, "Jan 01, 1980" );
                claims.Add( dobClaim );

                //
                // Resident zip code claim
                //
                Claim zipClaim = new Claim( ClaimTypes.PostalCode, "98052" );
                claims.Add( zipClaim );

                //
                // Phone number claim
                //
                Claim phoneClaim = new Claim( ClaimTypes.MobilePhone, "425-123-0000" );
                claims.Add( phoneClaim );
            }

            ClaimsIdentityCollection collection = new ClaimsIdentityCollection();
            ClaimsIdentity outputIdentity = new ClaimsIdentity( claims );
            return outputIdentity;
        }
    }
}
