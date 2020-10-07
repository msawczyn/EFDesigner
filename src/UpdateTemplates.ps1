$files = 
   'DslPackage\TextTemplates\EditingOnly\EF6Designer', 
   'DslPackage\TextTemplates\EditingOnly\EFCoreDesigner', 
   'DslPackage\TextTemplates\EditingOnly\EFDesigner'

foreach ($f in $files) {
[regex]::Replace((get-content "$f.cs" -Raw), '(?s)^.+#region Template[\r\n]+(.+)#endregion Template.+$', '<#@ assembly name="System.Core"
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
') | set-content "$f.ttinclude"

# Clean up the blank lines at the end of the file that, for some reason, appear there
[regex]::Replace((get-content "$f.ttinclude" -Raw), '\r?\n\r?\n\r?\n[\r\n]*', "") | set-content "$f.ttinclude"
}

