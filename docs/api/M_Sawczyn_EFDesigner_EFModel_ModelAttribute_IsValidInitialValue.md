# ModelAttribute.IsValidInitialValue Method 
 

Tests if the InitialValue property is valid for the type indicated

**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.2.7.0

## Syntax

**C#**<br />
``` C#
public <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">bool</a> IsValidInitialValue(
	<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">string</a> typeName = null,
	<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">string</a> initialValue = null
)
```

**VB**<br />
``` VB
Public Function IsValidInitialValue ( 
	Optional typeName As <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a> = Nothing,
	Optional initialValue As <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a> = Nothing
) As <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">Boolean</a>
```

<a href="https://github.com/msawczyn/EFDesigner/tree/master/src/Dsl/CustomCode/Partials/ModelAttribute.cs#L105" title="View the source code">View Source</a><br />

#### Parameters
&nbsp;<dl><dt>typeName (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Name of type to test. If typeName is null, Type property will be used. If initialValue is null, InitialValue property will be used</dd><dt>initialValue (Optional)</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />Initial value to test</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/a28wyd50" target="_blank">Boolean</a><br />true if InitialValue is a valid value for the type, or if initialValue is null or empty

## See Also


#### Reference
<a href="T_Sawczyn_EFDesigner_EFModel_ModelAttribute">ModelAttribute Class</a><br /><a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />