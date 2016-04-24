//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace FShipping.Models
{
    public class EnrollmentViewModel
    {
        public string OrganizationName { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsValidModel 
        {
            get
            {
                return string.IsNullOrEmpty(this.ErrorMessage);
            }
        }
    }
}