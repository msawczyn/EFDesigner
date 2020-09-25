$xml = (get-content src\Dsl\DslDefinition.dsl -Raw)

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
}

$t4 = 
   'src\DslPackage\TextTemplates\EF6Designer.ttinclude', 
   'src\DslPackage\TextTemplates\EFCoreDesigner.ttinclude', 
   'src\DslPackage\TextTemplates\EFDesigner.ttinclude',
   'src\DslPackage\TextTemplates\MultipleOutputHelper.ttinclude', 
   'src\DslPackage\TextTemplates\VSIntegration.ttinclude',
   'src\DslPackage\TextTemplates\EditingOnly\EF6Designer.cs',
   'src\DslPackage\TextTemplates\EditingOnly\EFCoreDesigner.cs',
   'src\DslPackage\TextTemplates\EditingOnly\EFDesigner.cs',
   'src\DslPackage\TextTemplates\EditingOnly\MultipleOutputHelper.cs',
   'src\DslPackage\TextTemplates\EditingOnly\VSIntegration.cs'

foreach ($f in $t4) {
   [regex]::Replace((get-content $f -Raw), '(\s*)// EFDesigner v[\d\.]+', '$1// EFDesigner v'+$version) | set-content $f
}

