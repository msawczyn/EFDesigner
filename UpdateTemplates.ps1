$search = '(?s)^.+#region Template[\r\n]+(.+)#endregion Template.+$'

$replace = '<#@ assembly name="System.Core"
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
#>'

$files = 
   'EFDesigner',
   'EF6Designer',
   'EFCoreDesigner',
   'EF6ModelGenerator',
   'EFCore2ModelGenerator',
   'EFCore3ModelGenerator',
   'EFCore5ModelGenerator',
   'EFCoreModelGenerator',
   'EFModelFileManager',
   'EFModelGenerator',
   'VSIntegrations'
   
foreach ($f in $files) {
   [regex]::Replace((get-content src\DslPackage\TextTemplates\EditingOnly\$f.cs -Raw), $search, $replace) | set-content src\DslPackage\TextTemplates\$f.ttinclude
}

