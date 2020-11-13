$search = '(?s)^.*#region Template[\r\n]+(.+)#endregion Template.*$'

$replace = '<#@ include file="EF6ModelGenerator.ttinclude" once="true"
#><#@ include file="EFCore2ModelGenerator.ttinclude" once="true"
#><#@ include file="EFCore3ModelGenerator.ttinclude" once="true"
#><#@ include file="EFCore5ModelGenerator.ttinclude" once="true"
#><#@ include file="EFCoreModelGenerator.ttinclude" once="true"
#><#@ include file="EFModelFileManager.ttinclude" once="true"
#><#@ include file="EFModelGenerator.ttinclude" once="true"
#><#@ include file="VSIntegration.ttinclude" once="true"
#><#@ assembly name="System.Core"
#><#@ assembly name="System.Data.Linq"
#><#@ assembly name="EnvDTE"
#><#@ assembly name="System.Xml"
#><#@ assembly name="System.Xml.Linq"
#><#@ import namespace="System"
#><#@ import namespace="System.IO"
#><#@ import namespace="System.Globalization"
#><#@ import namespace="System.Linq"
#><#@ import namespace="System.Security"
#><#@ import namespace="System.Text"
#><#@ import namespace="System.Collections.Generic"
#><#@ import namespace="System.Diagnostics.CodeAnalysis"
#><#@ import namespace="EnvDTE"
#><#@ import namespace="System.Data.Entity.Design.PluralizationServices"
#><#@ import namespace="Microsoft.VisualStudio.TextTemplating"
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
   'VSIntegration',
   'MultipleOutputHelper'
   
foreach ($f in $files) {
   [regex]::Replace((get-content src\DslPackage\TextTemplates\EditingOnly\$f.cs -Raw), $search, $replace) | set-content src\DslPackage\TextTemplates\$f.ttinclude
}

