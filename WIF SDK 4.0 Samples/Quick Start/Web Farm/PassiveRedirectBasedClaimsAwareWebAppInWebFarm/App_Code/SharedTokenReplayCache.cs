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
using System.IdentityModel.Tokens;
using System.ServiceModel;

using Microsoft.IdentityModel.Tokens;

/// <summary>
/// This class acts as a proxy to call a WCF service that implements a TokenReplayCache. 
/// </summary>
/// 
public class SharedTokenReplayCache : TokenReplayCache
{
    // change this address to point to the deployed service
    const string _serviceAddress = "http://localhost/TokenReplayCacheService/TokenReplayCacheService.svc";
    ITokenReplayCache _tokenReplayCache;

    public SharedTokenReplayCache()
    {
        // One could choose to lazy initialize when the first check for a 
        // replayed token occurs, this would decrease the latency on bootup.
        Initialize();
    }
    
    /// <summary>
    /// Creates a client channel to call the service host.
    /// </summary>
    protected void Initialize()
    {
        ChannelFactory<ITokenReplayCache> cf = new ChannelFactory<ITokenReplayCache>( new WS2007HttpBinding( SecurityMode.None ), new EndpointAddress( _serviceAddress ) );
        _tokenReplayCache = cf.CreateChannel();
    }

    /// <summary>
    /// Delegates to service.
    /// </summary>
    public override int Capacity
    {
        get
        {
            return _tokenReplayCache.GetCapacity();
        }
        set
        {
            _tokenReplayCache.SetCapacity( value );
        }
    }

    /// <summary>
    /// Delegates to service.
    /// </summary>
    public override void Clear()
    {
        _tokenReplayCache.Clear();
    }

    /// <summary>
    /// Delegates to service.
    /// </summary>
    public override int IncreaseCapacity( int size )
    {
        return _tokenReplayCache.IncreaseCapacity( size );
    }

    /// <summary>
    /// Delegates to service.
    /// </summary>
    public override TimeSpan PurgeInterval
    {
        get
        {
            return _tokenReplayCache.GetPurgeInterval();
        }
        set
        {
            _tokenReplayCache.SetPurgeInterval( value );
        }
    }

    /// <summary>
    /// Delegates to service.
    /// </summary>
    public override bool TryAdd( string key, SecurityToken securityToken, DateTime expirationTime )
    {
        if ( securityToken != null )
        {
            throw new NotSupportedException("SecurityToken must be null, this implementation does not support saving SecurityTokens");
        }

        return _tokenReplayCache.TryAdd( key, expirationTime );
    }

    /// <summary>
    /// Delegates to service.
    /// </summary>
    public override bool TryFind( string key )
    {
        return _tokenReplayCache.TryFind( key );
    }

    /// <summary>
    /// This method is not supported as the security token must be null when added.
    /// </summary>
    public override bool TryGet( string key, out SecurityToken securityToken )
    {
        throw new NotSupportedException("This implementation does not support saving SecurityTokens");
    }

    /// <summary>
    /// Delegates to service.
    /// </summary>
    public override bool TryRemove( string key )
    {
        return _tokenReplayCache.TryRemove( key );
    }
}
