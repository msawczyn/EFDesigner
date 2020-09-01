[regex]::Replace((get-content src\DslPackage\TextTemplates\EditingOnly\EF6Designer.cs -Raw), '(?s)^.+#region Template[\r\n]+(.+)#endregion Template.+$', '<#@ assembly name="System.Core"
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
') | set-content src\DslPackage\TextTemplates\EF6Designer.ttinclude

[regex]::Replace((get-content src\DslPackage\TextTemplates\EditingOnly\EFCoreDesigner.cs -Raw), '(?s)^.+#region Template[\r\n]+(.+)#endregion Template.+$', '<#@ assembly name="System.Core"
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
') | set-content src\DslPackage\TextTemplates\EFCoreDesigner.ttinclude

[regex]::Replace((get-content src\DslPackage\TextTemplates\EditingOnly\EFDesigner.cs -Raw), '(?s)^.+#region Template[\r\n]+(.+)#endregion Template.+$', '<#@ assembly name="System.Core"
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
') | set-content src\DslPackage\TextTemplates\EFDesigner.ttinclude
