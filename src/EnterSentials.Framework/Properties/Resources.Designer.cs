﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnterSentials.Framework.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EnterSentials.Framework.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Not authorized..
        /// </summary>
        internal static string AuthorizationErrorMessage {
            get {
                return ResourceManager.GetString("AuthorizationErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not authorized for action &apos;{0}&apos;..
        /// </summary>
        internal static string AuthorizationErrorMessageFormat {
            get {
                return ResourceManager.GetString("AuthorizationErrorMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while executing business logic..
        /// </summary>
        internal static string BusinessLogicErrorMessage {
            get {
                return ResourceManager.GetString("BusinessLogicErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not remove because it is currently depended upon by other parts of the system..
        /// </summary>
        internal static string CannotRemoveEntityDueToRelationshipConstraintsMessage {
            get {
                return ResourceManager.GetString("CannotRemoveEntityDueToRelationshipConstraintsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Setting &apos;ComponentsProviderAssemblyQualifiedTypeName&apos; must be defined for usage of components if no components provider has been explicitly set..
        /// </summary>
        internal static string ComponentsProviderSettingMissing {
            get {
                return ResourceManager.GetString("ComponentsProviderSettingMissing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not send email &apos;{0}&apos; to address &apos;{1}&apos;.
        /// </summary>
        internal static string CouldNotSendEmailErrorMessageFormat {
            get {
                return ResourceManager.GetString("CouldNotSendEmailErrorMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An error occurred while executing data access logic..
        /// </summary>
        internal static string DataAccessErrorMessage {
            get {
                return ResourceManager.GetString("DataAccessErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A serialization/deserialization failure occurred while dispatching a service operation..
        /// </summary>
        internal static string ServiceOperationDispatchSerializationErrorMessage {
            get {
                return ResourceManager.GetString("ServiceOperationDispatchSerializationErrorMessage", resourceCulture);
            }
        }
    }
}
