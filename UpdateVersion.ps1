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
   'src\Dsl\Properties\AssemblyInfo.cs', 
   'src\DslPackage\Properties\AssemblyInfo.cs', 
   'src\Utilities\ParsingModels\AssemblyInfo.cs'
   
foreach ($f in $assemblyInfo) {
   [regex]::Replace((get-content $f -Raw), '\[assembly:\s*AssemblyVersion\("[\d\.]+"\)\]', '[assembly: AssemblyVersion("'+$version+'")]') | set-content $f
   [regex]::Replace((get-content $f -Raw), '\[assembly:\s*AssemblyFileVersion\("[\d\.]+"\)\]', '[assembly: AssemblyFileVersion("'+$version+'")]') | set-content $f
   [regex]::Replace((get-content $f -Raw), '\r?\n\r?\n\r?\n[\r\n]*', "") | set-content $f # Clean up the blank lines at the end of the file that, for some reason, appear there
}

$files = 
'src\DslPackage\TextTemplates\EditingOnly\EF6ModelGenerator.cs',
'src\DslPackage\TextTemplates\EditingOnly\EFCore2ModelGenerator.cs',
'src\DslPackage\TextTemplates\EditingOnly\EFCore3ModelGenerator.cs',
'src\DslPackage\TextTemplates\EditingOnly\EFCore5ModelGenerator.cs',
'src\DslPackage\TextTemplates\EditingOnly\EFCoreDesigner.cs',
'src\DslPackage\TextTemplates\EditingOnly\EFCoreModelGenerator.cs',
'src\DslPackage\TextTemplates\EditingOnly\EFDesigner.cs',
'src\DslPackage\TextTemplates\EditingOnly\EFModelFileManager.cs',
'src\DslPackage\TextTemplates\EditingOnly\EFModelGenerator.cs',
'src\DslPackage\TextTemplates\EditingOnly\VSIntegration.cs'

foreach ($f in $t4) {
   [regex]::Replace((get-content $f -Raw), '(\s*)// EFDesigner v[\d\.]+', '$1// EFDesigner v'+$version) | set-content $f
   [regex]::Replace((get-content $f -Raw), '\r?\n\r?\n\r?\n[\r\n]*', "") | set-content $f
}

