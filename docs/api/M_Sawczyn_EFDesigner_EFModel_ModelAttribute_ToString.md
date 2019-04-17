# ModelAttribute.ToString Method 
 

Returns a string that represents the current object.

**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.2.7.0

## Syntax

**C#**<br />
``` C#
public override <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">string</a> ToString()
```

**VB**<br />
``` VB
Public Overrides Function ToString As <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>
```

<a href="https://github.com/msawczyn/EFDesigner/tree/master/src/Dsl/CustomCode/Partials/ModelAttribute.cs#L538" title="View the source code">View Source</a><br />

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a><br />A string that represents the current object.

## Remarks
Output is, in order: <ul><li>Visibility</li><li>Type (with optional '?' if not a required field</li><li>Max length in brackets, if a string field and length is specified</li><li>Name (with optional '!' if an identity field</li><li>an equal sign (=) followed by an initializer, if an initializer is specified</li></ul>

## See Also


#### Reference
<a href="T_Sawczyn_EFDesigner_EFModel_ModelAttribute">ModelAttribute Class</a><br /><a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />