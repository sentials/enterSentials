﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EnterSentials.Framework.Azure.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("[DefaultEndpointsProtocol=https;AccountName=entersentialsstorage;AccountKey=KrC1I" +
            "vaaqqj9v8Pm+yNF3sNKmM/jYc30crOC1BNUTUKEbLFrUPsb0S8+DvLhr8gxP7s1xIFU+F1cDn2gHn9lj" +
            "g==][someContainer]")]
        public string DefaultAzureBlobStorageConnectionString {
            get {
                return ((string)(this["DefaultAzureBlobStorageConnectionString"]));
            }
        }
    }
}
