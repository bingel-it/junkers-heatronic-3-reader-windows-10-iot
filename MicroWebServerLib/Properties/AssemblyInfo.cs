using System.Resources;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über folgende 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die einer Assembly zugeordnet sind.
[assembly: AssemblyTitle("MicroWebServerLib")]
[assembly: AssemblyDescription("A small HTTP WebServer to recieve HTTP Requests (GET/PUT/POST/DELETE)")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Bingel-IT")]
[assembly: AssemblyProduct("MicroWebServerLib")]
[assembly: AssemblyCopyright("Copyright ©  2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("de")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("BingelIT.MicroWebServerLib.Tests")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Build- und Revisionsnummern 
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("%Version.Full%")]
[assembly: AssemblyFileVersion("%Version.Full%")]
[assembly: AssemblyInformationalVersion("%Version.Full%-%env.vcs.short.hash%")]
