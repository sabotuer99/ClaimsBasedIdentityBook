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
        ///   Looks up a localized string similar to Hint: This link can only be seen by the role &apos;Accountant&apos;. User roles are stored in the database and authorization to this link is done in the master page (Site.master)..
        /// </summary>
        internal static string ApproveLinkHint {
            get {
                return ResourceManager.GetString("ApproveLinkHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: This value is user profile information provided as a claim by the issuer when authenticating because it is company-wide information..
        /// </summary>
        internal static string CostCenterHint {
            get {
                return ResourceManager.GetString("CostCenterHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: This is user profile information that is stored on the User table and it is specific for this application..
        /// </summary>
        internal static string PreferedReimbursementHint {
            get {
                return ResourceManager.GetString("PreferedReimbursementHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: When signing out from this application, you will also be signed out from the issuer who authenticated you. The issuer will also sign you out from any other applications that you&apos;ve signed in to..
        /// </summary>
        internal static string SingleSignOutHint {
            get {
                return ResourceManager.GetString("SingleSignOutHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hint: Now that you are signed-in by a trusted issuer, you can try the Single Sign On (SSO) experience between all the sites that trust the same issuer..
        /// </summary>
        internal static string SSOExperienceHint {
            get {
                return ResourceManager.GetString("SSOExperienceHint", resourceCulture);
            }
        }
    }
}
