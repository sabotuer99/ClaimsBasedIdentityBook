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

using System.Collections.Generic;

using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Web;

/// <summary>
/// This class encrypts the session security token using the ASP.NET configured machine key.
/// </summary>
public class MachineKeySessionSecurityTokenHandler : SessionSecurityTokenHandler
{
    static List<CookieTransform> _transforms;

    static MachineKeySessionSecurityTokenHandler()
    {        
        _transforms = new List<CookieTransform>() 
                        { 
                            new DeflateCookieTransform(), 
                            new MachineKeyProtectionTransform()
                        };
    }

    public MachineKeySessionSecurityTokenHandler()
        : base( _transforms.AsReadOnly() )
    {
    }
}
