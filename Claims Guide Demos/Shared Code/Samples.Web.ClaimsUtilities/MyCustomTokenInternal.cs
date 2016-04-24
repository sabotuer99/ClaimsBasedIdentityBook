namespace MyCustomToken
{
    using System;
    using System.Collections.Generic;
    using Microsoft.IdentityModel.Claims;

    /// <summary>
    /// Summary description for MyCustomTokenInternal
    /// </summary>
    public class MyCustomTokenInternal
    {
        public string Id { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }

        public string Audience { get; set; }

        public string Issuer { get; set; }

        public IEnumerable<Claim> Claims { get; set; }
    }
}
