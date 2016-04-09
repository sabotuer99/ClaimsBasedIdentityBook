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
using System.IdentityModel.Tokens;

using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// A custom UsernamPasswordSecurityTokenHandler that validates the username and password.
/// </summary>
class CustomUserNameSecurityTokenHandler : UserNameSecurityTokenHandler
{
    public CustomUserNameSecurityTokenHandler( )
        : base()
    {
    }

    /// <summary>
    /// Returns true to indicate that the token handler can Validate
    /// UserNameSecurityToken.
    /// </summary>
    public override bool CanValidateToken
    {
        get
        {
            return true;
        }
    }

    /// <summary>
    /// Overrides the abstract base class function to validate the username and the password.
    /// </summary>
    /// <param name="token">The SecurityToken from which to validate the user.</param>
    /// <exception cref="ArgumentNullException">If the SecurityToken is null.</exception>
    /// <exception cref="ArgumentException">If the SecurityToken is not a UserNameSecurityToken.</exception>
    /// <exception cref="InvalidOperationException">If the username/password pair is incorrect.</exception>
    /// <returns>A ClaimsIdentityCollection identifying the user.</returns>
    public override ClaimsIdentityCollection ValidateToken( SecurityToken token )
    {
        if ( token == null )
        {
            throw new ArgumentNullException( "token" );
        }

        UserNameSecurityToken usernameToken = token as UserNameSecurityToken;
        if ( usernameToken == null )
        {
            throw new ArgumentException( "usernameToken", "The security token is not a valid username security token." );
        }

        // The username and password verification logic shown below is for illustrative purpose only. Do not use this
        // in production environment.
        if ( StringComparer.Ordinal.Equals( usernameToken.UserName, "terry" ) && StringComparer.Ordinal.Equals( usernameToken.Password, "123" ) )
        {
            return new ClaimsIdentityCollection( 
                            new ClaimsIdentity[]{
                                new ClaimsIdentity(
                                    new Claim[]{    
                                        new Claim( ClaimTypes.GivenName, "Terry" ),
                                        new Claim( ClaimTypes.Surname, "Earls" ),
                                        new Claim( ClaimTypes.Email, "terry.earls@localhost.com" ),
                                        new Claim( ClaimTypes.Locality, "Coolsville" ),
                                        } ) } );
        }

        throw new InvalidOperationException( "The username/password is incorrect." );
    }
}
