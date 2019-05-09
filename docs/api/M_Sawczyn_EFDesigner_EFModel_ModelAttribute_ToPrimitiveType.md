# ModelAttribute.ToPrimitiveType Method 
 

From internal class System.Data.Metadata.Edm.PrimitiveType in System.Data.Entity. Converts a CLR type to a C# primitive type.

**Namespace:**&nbsp;<a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel</a><br />**Assembly:**&nbsp;Sawczyn.EFDesigner.EFModel.Dsl (in Sawczyn.EFDesigner.EFModel.Dsl.dll) Version: 1.2.7.0

## Syntax

**C#**<br />
``` C#
public static <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">string</a> ToPrimitiveType(
	<a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">string</a> typeName
)
```

**VB**<br />
``` VB
Public Shared Function ToPrimitiveType ( 
	typeName As <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>
) As <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a>
```

<a href="https://github.com/msawczyn/EFDesigner/tree/master/src/Dsl/CustomCode/Partials/ModelAttribute.cs#L252" title="View the source code">View Source</a><br />

#### Parameters
&nbsp;<dl><dt>typeName</dt><dd>Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">System.String</a><br />CLR type</dd></dl>

#### Return Value
Type: <a href="http://msdn2.microsoft.com/en-us/library/s1wwdcbf" target="_blank">String</a><br />Name of primitive type given

## See Also


#### Reference
<a href="T_Sawczyn_EFDesigner_EFModel_ModelAttribute">ModelAttribute Class</a><br /><a href="N_Sawczyn_EFDesigner_EFModel">Sawczyn.EFDesigner.EFModel Namespace</a><br />