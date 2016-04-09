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

namespace TokenReplayCacheService
{
    /// <summary>
    /// A simple TokenReplayCache interface implemented by the WCF Service defined below.
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


    /// <summary>
    /// This WCF service exposes a TokenReplayCache that can be accessed by multple relying parties.  
    /// The WIF DefaultTokenReplayCache is used as the internal cache.  A durable cache which would withstand
    /// recycles, could be backed by a database.
    /// </summary>
    [ServiceBehavior]
    public class TokenReplayCacheService : ITokenReplayCache
    {
        DefaultTokenReplayCache _internalCache;

        public TokenReplayCacheService()
        {
            // these parameters specify 100,000 entries in cache AND a purge interval of 5 minutes.
            // 100,000 is reasonable since only a Hash is stored.
            _internalCache = new DefaultTokenReplayCache( 100000, TimeSpan.FromMinutes( 5 ) );
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// </summary>
        [OperationBehavior]
        public int GetCapacity()
        {
            return _internalCache.Capacity;
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// </summary>
        [OperationBehavior]
        public void SetCapacity( int size )
        {
            _internalCache.Capacity = size;
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// </summary>
        [OperationBehavior]
        public TimeSpan GetPurgeInterval()
        {
            return _internalCache.PurgeInterval;
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// </summary>
        [OperationBehavior]
        public void SetPurgeInterval( TimeSpan purgeInterval )
        {
            _internalCache.PurgeInterval = purgeInterval;
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// </summary>
        [OperationBehavior]
        public void Clear()
        {
            _internalCache.Clear();
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// </summary>
        [OperationBehavior]
        public int IncreaseCapacity( int size )
        {
            return _internalCache.IncreaseCapacity( size );
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// Differs slightly from the definition as the SecurityToken parameter has been removed.
        /// </summary>
        [OperationBehavior]
        public bool TryAdd( string key, DateTime expirationTime )
        {
            return _internalCache.TryAdd( key, null, expirationTime );
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// </summary>
        [OperationBehavior]
        public bool TryFind( string key )
        {
            return _internalCache.TryFind( key );
        }

        /// <summary>
        /// Delegates to internalCache (DefaultTokenReplayCache).
        /// </summary>
        [OperationBehavior]
        public bool TryRemove( string key )
        {
            return _internalCache.TryRemove( key );
        }
    }
}
