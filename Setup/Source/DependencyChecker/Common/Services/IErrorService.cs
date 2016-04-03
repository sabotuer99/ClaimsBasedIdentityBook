//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common.Services
{
    using System;

    public interface IErrorService
    {
        void LogError(string message, Exception exception);
        void ShowError(string message, Exception exception);
    }
}