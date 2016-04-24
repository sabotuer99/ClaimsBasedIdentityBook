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

    public class Expense
    {
        public decimal Amount { get; set; }

        public bool Approved { get; set; }

        public string CostCenter { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public Guid Id { get; set; }

        public ReimbursementMethod ReimbursementMethod { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
    }
}