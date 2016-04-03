//===============================================================================
// Microsoft patterns & practices
// Cliams Identity Guide V2
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://claimsid.codeplex.com/license)
//===============================================================================


namespace DependencyChecker.Common.CheckEvaluators.Helpers
{
    using System;
    using Microsoft.Win32;

    internal static class RegistryHelper
    {
        public enum RegistryKeyRoot
        {
            HKLM, 
            HKCU, 
            HKCR
        }

        public static bool IsInKey(string regPath, string valueName, string desiredValue)
        {
            string finalRegPath;
            using (RegistryKey root = GetRoot(regPath, out finalRegPath))
            using (RegistryKey key = root.OpenSubKey(finalRegPath))
            {
                if (key != null)
                {
                    foreach (string subkey in key.GetSubKeyNames())
                    {
                        using (RegistryKey openSubKey = key.OpenSubKey(subkey))
                        {
                            if (openSubKey != null)
                            {
                                var value = openSubKey.GetValue(valueName) as string;
                                if ((value != null) && value.StartsWith(desiredValue, StringComparison.OrdinalIgnoreCase))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
        }

        public static bool IsValueAt(string regPath, string valueName, string desiredValue)
        {
            string finalRegPath;
            using (RegistryKey root = GetRoot(regPath, out finalRegPath))
            using (RegistryKey key = root.OpenSubKey(finalRegPath))
            {
                if (key != null)
                {
                    return key.GetValue(valueName, "NOTFOUND").ToString().StartsWith(desiredValue, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            }
        }

        public static bool KeyExists(string regPath)
        {
            string finalRegPath;
            using (RegistryKey root = GetRoot(regPath, out finalRegPath))
            using (RegistryKey key = root.OpenSubKey(finalRegPath))
            {
                if (key != null)
                {
                    return true;
                }
                return false;
            }
        }

        public static bool ValueExists(string regPath, string valueName)
        {
            string finalRegPath;
            using (RegistryKey root = GetRoot(regPath, out finalRegPath))
            using (RegistryKey key = root.OpenSubKey(finalRegPath))
            {
                if (key != null && key.GetValue(valueName) != null)
                {
                    return true;
                }
                return false;
            }
        }

        internal static string GetValue(string regPath, string valueName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regPath))
            {
                if (key != null)
                {
                    var value = (string)key.GetValue(valueName);
                    return value;
                }
                return string.Empty;
            }
        }

        private static RegistryKey GetRoot(string regPath, out string finalRegPath)
        {
            RegistryKeyRoot keyRoot;

            if (regPath.Length > 4 && regPath[4] == ':')
            {
                keyRoot = (RegistryKeyRoot)Enum.Parse(typeof(RegistryKeyRoot), regPath.Substring(0, 4));
                finalRegPath = regPath.Substring(5);
            }
            else
            {
                keyRoot = RegistryKeyRoot.HKLM;
                finalRegPath = regPath;
            }

            RegistryKey root = Registry.LocalMachine;

            switch (keyRoot)
            {
                case RegistryKeyRoot.HKLM:
                    root = Registry.LocalMachine;
                    break;
                case RegistryKeyRoot.HKCU:
                    root = Registry.CurrentUser;
                    break;
                case RegistryKeyRoot.HKCR:
                    root = Registry.ClassesRoot;
                    break;
            }
            return root;
        }
    }
}