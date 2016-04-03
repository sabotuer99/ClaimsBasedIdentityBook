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
    using System.Diagnostics;
    using System.Windows.Forms;

    public class ErrorService : IErrorService
    {
        private const string EventSourceName = "DependencyChecker";
        private const string LogName = "Application";
        private readonly EventLog log;

        public ErrorService()
        {
            this.log = new EventLog(LogName)
                           {
                               Source = EventSourceName
                           };

            if (!EventLog.SourceExists(EventSourceName))
            {
                EventLog.CreateEventSource(EventSourceName, LogName);
            }
        }

        public void LogError(string message, Exception exception)
        {
            this.log.WriteEntry(string.Format("An exception has occured in the DependencyChecker. \r\nMessage:{0}\r\n{1}", message, exception.TraceInformation()));
        }

        public void ShowError(string message, Exception exception)
        {
            MessageBox.Show(message, @"Dependency checker", MessageBoxButtons.OK, MessageBoxIcon.Error);

            this.LogError(message, exception);
        }
    }
}