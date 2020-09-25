[regex]::Replace((get-content DslPackage\TextTemplates\EditingOnly\EF6Designer.cs -Raw), '(?s)^.+#region Template[\r\n]+(.+)#endregion Template.+$', '<#@ assembly name="System.Core"
#><#@ assembly name="System.Data.Linq"
#><#@ assembly name="EnvDTE"
#><#@ assembly name="System.Xml"
#><#@ assembly name="System.Xml.Linq"
#><#@ import namespace="System.Collections.Generic"
#><#@ import namespace="System.IO"
#><#@ import namespace="System.Linq" 
#><#@ import namespace="System.Security"
#><#+
$1
#>
') | set-content DslPackage\TextTemplates\EF6Designer.ttinclude

[regex]::Replace((get-content DslPackage\TextTemplates\EditingOnly\EFCoreDesigner.cs -Raw), '(?s)^.+#region Template[\r\n]+(.+)#endregion Template.+$', '<#@ assembly name="System.Core"
#><#@ assembly name="System.Data.Linq"
#><#@ assembly name="EnvDTE"
#><#@ assembly name="System.Xml"
#><#@ assembly name="System.Xml.Linq"
#><#@ import namespace="System.Collections.Generic"
#><#@ import namespace="System.IO"
#><#@ import namespace="System.Linq" 
#><#@ import namespace="System.Security"
#><#+
$1
#>
') | set-content DslPackage\TextTemplates\EFCoreDesigner.ttinclude

[regex]::Replace((get-content DslPackage\TextTemplates\EditingOnly\EFDesigner.cs -Raw), '(?s)^.+#region Template[\r\n]+(.+)#endregion Template.+$', '<#@ assembly name="System.Core"
#><#@ assembly name="System.Data.Linq"
#><#@ assembly name="EnvDTE"
#><#@ assembly name="System.Xml"
#><#@ assembly name="System.Xml.Linq"
#><#@ import namespace="System.Collections.Generic"
#><#@ import namespace="System.IO"
#><#@ import namespace="System.Linq" 
#><#@ import namespace="System.Security"
#><#+
$1
#>
') | set-content DslPackage\TextTemplates\EFDesigner.ttinclude
