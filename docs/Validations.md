# Validations

The designer validates the model both while you're editing it and when it's saved.
There are also a number of automatic corrections that silently occur when model 
properties conflict.

Validations show up as warnings or error messages in the output window or,
in some circumstances, error popups that stop you from doing a bad action.

## Errors

The following errors can appear in the output window. They must be corrected or bad code will be generated.

**Entity container needs a name**: The `Name` property of the designer is empty

**Enum has no values**: An enum on the model doesn't have any value elements.

**Should have X concurrency properties but has Y**:

**Class has no identity property in inheritance chain**:

**Properties can't be named the same as the enclosing class**:

**Principal/dependent designations must be manually set for 1..1 and 0-1..0-1 associations**:

**Association endpoints can only be to most-derived classes in TPC inheritance strategy**:

The following display popups that prevent the action causing the error




## Warnings

**Association end should be documented**:

**String length not specified**:

**Property should be documented**:

**Class has no properties**: 

**Identity attribute in derived class X becomes a composite key**: 

**Class X should be documented**:

**Enum has some, but not all, values initialized. Please ensure this is what was intended**: 

**Enum should be documented**: 

**Enum value should be documented**:

**Default connection string missing**:



