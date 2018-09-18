# Inheritance

<img align="right" hspace="10" src="images/Inheritance.jpg">

Modeling inheritance is pretty straightforward. Select the Inheritance line and, starting at the derived class, click/hold/drag to the base class.

Note that, when creating an inheritance link, **the designer will remove** any properties from the child class that are present in the parent. 
Also, you won't be allowed to create a multiple inheritance scenario or to create circular inheritance patterns (e.g., A is derived from B, B is derived from A).

There aren't any properties on the inheritance line - selecting it in the designer will show an empty
property window. The only property related to inheritance is the context's <a href="Using-the-designer.html#design-surface-properties">Inheritance Strategy</a> on the design surface.

### Deleting an inheritance relationship

When deleting an inheritance relationship, you'll be asked if you want to push the base entity's
attributes and associations down to the child entities prior to its removal. If you decline, the attributes
and associations of the (formerly) derived entity won't change, so if it doesn't currently have
an identity (key) attribute, it won't have one when you're done. You'll have to add that manually if you need it.

### Next Step 
[Enumerations](Enumerations)
