//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace AExpense.Data
{
    using System;
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Roles = new List<Role>();
        }

        public string CostCenter { get; set; }
        public string FullName { get; set; }
        public Guid Id { get; set; }

        public string PasswordHashAndSalt { get; internal set; }
        public ReimbursementMethod PreferredReimbursementMethod { get; set; }
        public IList<Role> Roles { get; private set; }
        public string UserName { get; set; }

        public string GetSalt(int saltSize)
        {
            return this.PasswordHashAndSalt.Substring(this.PasswordHashAndSalt.Length - saltSize);
        }
    }
}