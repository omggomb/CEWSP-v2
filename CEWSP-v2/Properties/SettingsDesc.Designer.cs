﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CEWSP_v2.Properties {
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
    public class SettingsDesc {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SettingsDesc() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CEWSP_v2.Properties.SettingsDesc", typeof(SettingsDesc).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The project that was active when CEWSP was closed.
        /// </summary>
        public static string DescLastUsedProject {
            get {
                return ResourceManager.GetString("DescLastUsedProject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Should the welcome window be shown on startup.
        /// </summary>
        public static string DescShowWelcomeWindow {
            get {
                return ResourceManager.GetString("DescShowWelcomeWindow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Last active project.
        /// </summary>
        public static string HumLastUsedProject {
            get {
                return ResourceManager.GetString("HumLastUsedProject", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Show welcome window on startup.
        /// </summary>
        public static string HumShowWelcomeWindow {
            get {
                return ResourceManager.GetString("HumShowWelcomeWindow", resourceCulture);
            }
        }
    }
}
