//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace Microsoft.Samples.DPE.OAuth.Tokens
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.IdentityModel.Tokens;

    public class SimpleWebToken : SecurityToken
    {
        private static readonly TimeSpan DefaultValidity = new TimeSpan(1, 0, 0);

        private string id;
        private NameValueCollection parameters;
        private DateTime timestamp;
        
        public SimpleWebToken(string issuer)            
        {
            this.Issuer = issuer;
            this.timestamp = DateTime.UtcNow;
            this.id = Guid.NewGuid().ToString().Remove(8);
            this.parameters = new NameValueCollection();
            this.TokenValidity = DefaultValidity;
        }               

        public override string Id
        {
            get { return this.id; }            
        }
                
        public override ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get { return EmptyReadOnlyCollection<SecurityKey>.Instance; }
        }

        public override DateTime ValidFrom
        {
            get 
            { 
                // TODO: Implement
                return DateTime.MinValue;
            }
        }

        public override DateTime ValidTo
        {
            get { return this.timestamp + this.TokenValidity; }
        }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public byte[] Signature { get; set; }

        public string SignatureAlgorithm { get; set; }

        public string RawToken { get; set; }

        public TimeSpan TokenValidity { get; set; }

        public NameValueCollection Parameters
        {
            get
            {
                return this.parameters;
            }
        }

        public void SetId(string id)
        {
            this.id = id;
        }

        public void AddClaim(string name, string value)
        {
            this.parameters.Add(name, value);
        }
    }

    internal static class EmptyReadOnlyCollection<T>
    {
        public static ReadOnlyCollection<T> Instance 
        {
            get
            {
                return new ReadOnlyCollection<T>(new List<T>());
            }
        }
    }
}