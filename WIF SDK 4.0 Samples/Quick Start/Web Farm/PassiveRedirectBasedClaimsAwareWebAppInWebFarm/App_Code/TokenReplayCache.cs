//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//-----------------------------------------------------------------------------

using System;
using System.ServiceModel;
using Microsoft.IdentityModel.Tokens;

/// <summary>
/// A TokenReplayCache interface implemented by the WCF Service in the TokenRepayCacheService project.
/// </summary>
[ServiceContract()]
public interface ITokenReplayCache
{
    [OperationContract()]
    int GetCapacity();

    [OperationContract()]
    void SetCapacity( int size );

    [OperationContract()]
    TimeSpan GetPurgeInterval();

    [OperationContract()]
    void SetPurgeInterval( TimeSpan purgeInterval );

    [OperationContract()]
    void Clear();

    [OperationContract()]
    int IncreaseCapacity( int size );

    [OperationContract()]
    bool TryAdd( string key, DateTime expirationTime );

    [OperationContract()]
    bool TryFind( string key );

    [OperationContract()]
    bool TryRemove( string key );
}
