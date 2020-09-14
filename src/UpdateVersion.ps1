# [Reflection.Assembly]::Load("EnvDTE")                                               
# $DTE.ExecuteCommand("TextTransformation.TransformAllTemplates")

$xml = (get-content Dsl\DslDefinition.dsl -Raw)

$major = 0
$minor = 0
$build = 0
$revision = 0

$m = $xml | Select-String -Pattern 'MajorVersion="(\d+)"'
if ($m.Matches.Success) { $major = $m.Matches.Groups[1].Value }
$m = $xml | Select-String -Pattern 'MinorVersion="(\d+)"'
if ($m.Matches.Success) { $minor = $m.Matches.Groups[1].Value }
$m = $xml | Select-String -Pattern 'Build="(\d+)"'
if ($m.Matches.Success) { $build = $m.Matches.Groups[1].Value }
$m = $xml | Select-String -Pattern 'Revision="(\d+)"'
if ($m.Matches.Success) { $revision = $m.Matches.Groups[1].Value }

$version = $major+'.'+$minor+'.'+$build+'.'+$revision

$assemblyInfo = 
   'Dsl\Properties\AssemblyInfo.cs', 
   'DslPackage\Properties\AssemblyInfo.cs', 
   'Utilities\ParsingModels\AssemblyInfo.cs'
   
foreach ($f in $assemblyInfo) {
   [regex]::Replace((get-content $f -Raw), '\[assembly:\s*AssemblyVersion\("[\d\.]+"\)\]', '[assembly: AssemblyVersion("'+$version+'")]') | set-content $f
   [regex]::Replace((get-content $f -Raw), '\[assembly:\s*AssemblyFileVersion\("[\d\.]+"\)\]', '[assembly: AssemblyFileVersion("'+$version+'")]') | set-content $f
}

$t4 = 
   'DslPackage\TextTemplates\EF6Designer.ttinclude', 
   'DslPackage\TextTemplates\EFCoreDesigner.ttinclude', 
   'DslPackage\TextTemplates\EFDesigner.ttinclude',
   'DslPackage\TextTemplates\MultipleOutputHelper.ttinclude', 
   'DslPackage\TextTemplates\VSIntegration.ttinclude',
   'DslPackage\TextTemplates\EditingOnly\EF6Designer.cs',
   'DslPackage\TextTemplates\EditingOnly\EFCoreDesigner.cs',
   'DslPackage\TextTemplates\EditingOnly\EFDesigner.cs',
   'DslPackage\TextTemplates\EditingOnly\MultipleOutputHelper.cs',
   'DslPackage\TextTemplates\EditingOnly\VSIntegration.cs'

foreach ($f in $t4) {
   [regex]::Replace((get-content $f -Raw), '(\s*)// EFDesigner v[\d\.]+', '$1// EFDesigner v'+$version) | set-content $f
}
