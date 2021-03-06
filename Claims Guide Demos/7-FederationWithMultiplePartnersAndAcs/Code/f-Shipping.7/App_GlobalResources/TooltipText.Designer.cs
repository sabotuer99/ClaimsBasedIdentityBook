//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option or rebuild the Visual Studio project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Web.Application.StronglyTypedResourceProxyBuilder", "12.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class TooltipText {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal TooltipText() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Resources.TooltipText", global::System.Reflection.Assembly.Load("App_GlobalResources"));
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: The username is read from User.Identity.Name. In a claims-aware application, this property is of type IClaimsPrincipal. Displayed between parenthesis, you should see the issuer of the token received by the application (Fabrikams&apos;s federation provider) and the original issuer (adatum, litware, or fabrikam-simple identity provider)..
        /// </summary>
        internal static string LoggedInUserNameHint {
            get {
                return ResourceManager.GetString("LoggedInUserNameHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: Incoming claim types to be mapped to Fabrikam&apos;s claims..
        /// </summary>
        internal static string MappingHint {
            get {
                return ResourceManager.GetString("MappingHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: In a real application, you may not want to display a list of all your partners. This page exists to help you find your way around the demo. Adatum and Litware have their own issuers, Contoso uses Windows Live to authenticate its users..
        /// </summary>
        internal static string PreProvisionedHint {
            get {
                return ResourceManager.GetString("PreProvisionedHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: The sender&apos;s address is retrieved from the StreetAddress, StateOrProvince and Country Claims..
        /// </summary>
        internal static string SenderAddressHint {
            get {
                return ResourceManager.GetString("SenderAddressHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: The sender&apos;s Cost Center is retrieved from the CostCenter Claim..
        /// </summary>
        internal static string SenderCostCenterHint {
            get {
                return ResourceManager.GetString("SenderCostCenterHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: The sender&apos;s name is retrieved from the GivenName and Surname Claims..
        /// </summary>
        internal static string SenderNameHint {
            get {
                return ResourceManager.GetString("SenderNameHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: The last selected ServiceType is persisted in the mocked database to emulate ASP.NET&apos;s Profile Provider..
        /// </summary>
        internal static string SenderServiceTypeHint {
            get {
                return ResourceManager.GetString("SenderServiceTypeHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: Since the logged in user has a role claim with a value of &apos;Shipment Manager&apos;, he can cancel shipment orders..
        /// </summary>
        internal static string ShipmentManagerHint {
            get {
                return ResourceManager.GetString("ShipmentManagerHint", resourceCulture);
            }
        }
    }
}
