using System.Reflection;
using System.Runtime.InteropServices;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("CitizenMatt.DotPeek.AssemblyLists")]
[assembly: AssemblyDescription("Adds assembly list management to JetBrains' dotPeek")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("@citizenmatt")]
[assembly: AssemblyProduct("CitizenMatt.DotPeek.AssemblyLists")]
[assembly: AssemblyCopyright("Copyright © Matt Ellis 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3fa84ed4-848c-43d6-ace0-486ed9493cc3")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.1219.*")]
[assembly: AssemblyFileVersion("1.0.1219.1")]

[assembly: PluginDescription("Adds assembly list management")]
[assembly: PluginTitle("CitizenMatt.DotPeek.AssemblyLists")]
[assembly: PluginVendor("@citizenmatt")]

[assembly: ActionsXml("CitizenMatt.DotPeek.AssemblyLists.Actions.xml")]

