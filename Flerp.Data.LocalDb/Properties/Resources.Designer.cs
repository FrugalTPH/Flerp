﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Flerp.Data.LocalDb.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Flerp.Data.LocalDb.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to DisposedDate.
        /// </summary>
        internal static string DbField_DisposedDate {
            get {
                return ResourceManager.GetString("DbField_DisposedDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to _id.
        /// </summary>
        internal static string DbField_Id {
            get {
                return ResourceManager.GetString("DbField_Id", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  (x86).
        /// </summary>
        internal static string Filter_x86 {
            get {
                return ResourceManager.GetString("Filter_x86", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Flerp.
        /// </summary>
        internal static string FlerpDbName {
            get {
                return ResourceManager.GetString("FlerpDbName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MongoDatabaseProvider has already been configured.
        /// </summary>
        internal static string uiMsg_MongoAlreadyConfigured {
            get {
                return ResourceManager.GetString("uiMsg_MongoAlreadyConfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Mongo connection attempt {0} failed..
        /// </summary>
        internal static string uiMsg_MongoConnectionFailed {
            get {
                return ResourceManager.GetString("uiMsg_MongoConnectionFailed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to MongoDatabaseProvider has not yet been configured.
        /// </summary>
        internal static string uiMsg_MongoNotConfigured {
            get {
                return ResourceManager.GetString("uiMsg_MongoNotConfigured", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting Mongo Daemon at path={0}, port={1}, dbPath={2}.
        /// </summary>
        internal static string uiMsg_MongoStartupFeedback {
            get {
                return ResourceManager.GetString("uiMsg_MongoStartupFeedback", resourceCulture);
            }
        }
    }
}