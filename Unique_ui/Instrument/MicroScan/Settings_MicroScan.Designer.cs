//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UNIQUE.Instrument.MicroScan {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Settings_MicroScan : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings_MicroScan defaultInstance = ((Settings_MicroScan)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings_MicroScan())));
        
        public static Settings_MicroScan Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("COM1")]
        public string PortName {
            get {
                return ((string)(this["PortName"]));
            }
            set {
                this["PortName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("9600")]
        public int BaudRate {
            get {
                return ((int)(this["BaudRate"]));
            }
            set {
                this["BaudRate"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int DataBits {
            get {
                return ((int)(this["DataBits"]));
            }
            set {
                this["DataBits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("None")]
        public global::System.IO.Ports.Parity Parity {
            get {
                return ((global::System.IO.Ports.Parity)(this["Parity"]));
            }
            set {
                this["Parity"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("One")]
        public global::System.IO.Ports.StopBits StopBits {
            get {
                return ((global::System.IO.Ports.StopBits)(this["StopBits"]));
            }
            set {
                this["StopBits"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ClearOnOpen {
            get {
                return ((bool)(this["ClearOnOpen"]));
            }
            set {
                this["ClearOnOpen"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("False")]
        public bool ClearWithDTR {
            get {
                return ((bool)(this["ClearWithDTR"]));
            }
            set {
                this["ClearWithDTR"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string Pathspy {
            get {
                return ((string)(this["Pathspy"]));
            }
            set {
                this["Pathspy"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Text")]
        public global::UNIQUE.DataMode DataMode {
            get {
                return ((global::UNIQUE.DataMode)(this["DataMode"]));
            }
            set {
                this["DataMode"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("000001")]
        public string CounterM1 {
            get {
                return ((string)(this["CounterM1"]));
            }
            set {
                this["CounterM1"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0000001")]
        public string CounterM2 {
            get {
                return ((string)(this["CounterM2"]));
            }
            set {
                this["CounterM2"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("")]
        public string DBSet {
            get {
                return ((string)(this["DBSet"]));
            }
            set {
                this["DBSet"] = value;
            }
        }
    }
}
