﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Condominium2000.Resources {
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
    internal class ErrorCodes {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorCodes() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    var temp = new global::System.Resources.ResourceManager("Condominium2000.Resources.ErrorCodes", typeof(ErrorCodes).Assembly);
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
        ///   Looks up a localized string similar to Filnamn med samma namn existerar redan. Var vänlig försök med ett annat filnamn..
        /// </summary>
        internal static string ERROR_FILENAME_DUPLICATE {
            get {
                return ResourceManager.GetString("ERROR_FILENAME_DUPLICATE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Filnamnet är felformaterat ex. får in filnamnet innehålla två &quot;.&quot;). Se över filnamnet och försök igen!.
        /// </summary>
        internal static string ERROR_FILENAME_INVALID_FORMAT {
            get {
                return ResourceManager.GetString("ERROR_FILENAME_INVALID_FORMAT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Okänt fel. Verifiera dina uppgifter och försök igen. Om problemet kvarstår, var vänlig kontakta system administratören..
        /// </summary>
        internal static string ERROR_GENERAL_DEFAULT {
            get {
                return ResourceManager.GetString("ERROR_GENERAL_DEFAULT", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Objekt med samma namn existerar redan. Var vänlig försök med ett annat namn..
        /// </summary>
        internal static string ERROR_GENERAL_DUPLICATE_OBJECT_NAME {
            get {
                return ResourceManager.GetString("ERROR_GENERAL_DUPLICATE_OBJECT_NAME", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Något oförväntat inträffade. Verifiera dina uppgifter och försök igen. Om problemet kvarstår, var vänlig kontakta system administratören..
        /// </summary>
        internal static string ERROR_GENERAL_PROVIDER_ERROR {
            get {
                return ResourceManager.GetString("ERROR_GENERAL_PROVIDER_ERROR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Kunde inte logga in på FTP!.
        /// </summary>
        internal static string ERROR_LOGIN_FTP {
            get {
                return ResourceManager.GetString("ERROR_LOGIN_FTP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Säkerhetsorden var ej korrekt, var vänlig försök igen..
        /// </summary>
        internal static string ERROR_RECAPTCHA_INVALID {
            get {
                return ResourceManager.GetString("ERROR_RECAPTCHA_INVALID", resourceCulture);
            }
        }
    }
}
