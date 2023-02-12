// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project. Project-level
// suppressions either have no target or are given a specific target
// and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the
// Error List, point to "Suppress Message(s)", and click "In Project
// Suppression File". You do not need to add suppressions to this
// file manually.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Scope = "type", Target = "~T:Sawczyn.EFDesigner.EFModel.ModelAttribute", Justification="Attribute in this sense is a model concept")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods", Scope = "member", Target = "~P:Sawczyn.EFDesigner.EFModel.ModelAttribute.Type", Justification="Type here refers to the model concept")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1703:ResourceStringsShouldBeSpelledCorrectly", MessageId = "efmodel", Scope = "resource", Target = "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx.resources", Justification = "'efmodel' is the file extension")]
[assembly: SuppressMessage("Style", "IDE0090:Use 'new(...)'", Justification = "Code clarity")]
