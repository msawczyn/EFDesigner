# Associations

Associations are relationships beteween two entities, termed "navigation properties" in Entity Framework documentation.

The designer supports creation of both unidirectional and bidirectional associations. In unidirectional associations, the source
entity contains a property of the destination type, but not the other way around. In bidirectional associations,
each class contains a property pointing to the other entity.

As an example, consider the following:

<img src="https://raw.githubusercontent.com/wiki/msawczyn/EFDesigner/images/Unidirectional.jpg">

This tells us that Entity1 has a collection of Entity2 objects. The type of collection will typically be set as the default value
set in the design surface, but can be overridden (see Association Properties, below).

In short, this means that Entity1 would contain

```
public virtual ICollection<Entity2> TheEntity2s { get; set; } 
```
   
but Entity2 doesn't have a matching property.

In contrast, a bidirectional association like

<img src="https://raw.githubusercontent.com/wiki/msawczyn/EFDesigner/images/Bidirectional.jpg">

would have, in the Entity2 class,

```
public virtual Entity1 TheEntity1 { get; set; }  // Required
```

## Cardinalities

Associations have cardinalities (the number of elements in a set) at each end. Supported cardinalities are:
- Zero or One (0..1)
- One (1)
- Zero or More (*)

While cardinalities include One or More (1..*), Entity Framework currently doesn't support that. That doesn't mean
that it can't be done, just that it has to be done with custom code (and, in fact, is on the to-do list of things
to support with custom code generation in the designer).

Regardless of whether the association is unidirectional or bidirectional, it's important to correctly specify the
cardinality at each end. It has an effect on the kind of database structure Entity Framework creates, as well as 
being critical to properly modelling the problem space. 

It can't be emphasized enough - **getting an association's cardinality correct is *extremely* important**. Lots of application code 
will have to change if you wind up changing the cardinalities after the entities are in use.
 
## Unidirectional Associations
Unidirectional associations have the following properties:

<table>
<thead>
<tr><td valign="top"><b>Property</b></td><td valign="top"><b>Description</b></td></tr>
</thead>
<tbody>
<tr><td colspan="2" style="background-color: gainsboro"><b>Code Generation</b></td></tr>
<tr><td valign="top">Collection Class</td><td valign="top"><i>String</i>. The concrete class used to implement Zero or More cardinality ends. Defaults to class set at the designer level, but can be overridden here. Must implement ICollection&lt;T&gt;.</td></tr>
<tr><td valign="top">Persistent</td><td valign="top"><i>Boolean</i>. If false, the association will not be persisted in the database, requiring custom code to implement.</td></tr>
<tr><td colspan="2" style="background-color: gainsboro"><b>Documentation</b></td></tr>
<tr><td valign="top">Comment Summary</td><td valign="top"><i>String</i>. XML comment &lt;Summary&gt; section</td></tr>
<tr><td colspan="2" style="background-color: gainsboro"><b>End1</b></td></tr>
<tr><td valign="top">End1 Multiplicity</td><td valign="top"><i>String</i>. Cardinality of this end</td></tr>
<tr><td valign="top">End1 On Delete</td><td valign="top"><i>String</i>. How to handle objects on this end if the object on the other end is deleted. Can be programmatically fixed (if the other end has a One cardinality, therefore is required) or manually set.</td></tr>
<tr><td valign="top">End1 Role</td><td valign="top"><i>String</i>. Whether this end is the Principal or Dependent end of the association. For more information, see <a href="https://msdn.microsoft.com/en-us/library/jj713564(v=vs.113).aspx">Entity Framework Relationships and Navigation Properties</a></td></tr>
<tr><td colspan="2" style="background-color: gainsboro"><b>End2</b></td></tr>
<tr><td valign="top">End1 Navigation Property</td><td valign="top"><i>String</i>. Name of the property that will be generated in the class on the *other* side of the association.</td></tr>
<tr><td valign="top">End2 Comment Detail</td><td valign="top"><i>String</i>. XML comment &lt;Remarks&gt; section for this end</td></tr>
<tr><td valign="top">End2 Comment Summary</td><td valign="top"><i>String</i>. XML comment &lt;Summary&gt; section for this end</td></tr>
<tr><td valign="top">End2 Multiplicity</td><td valign="top"><i>String</i>. Cardinality of this end</td></tr>
<tr><td valign="top">End2 On Delete</td><td valign="top"><i>String</i>. How to handle objects on this end if the object on the other end is deleted. Can be programmatically fixed (if the other end has a One cardinality, therefore is required) or manually set.</td></tr>
<tr><td valign="top">End2 Role</td><td valign="top"><i>String</i>. Whether this end is the Principal or Dependent end of the association. For more information, see <a href="https://msdn.microsoft.com/en-us/library/jj713564(v=vs.113).aspx">Entity Framework Relationships and Navigation Properties</a></td></tr>
</tbody>
</table>


## Bidirectional Associations

Bidirectional associations have the following properties:

<table>
<thead>
<tr><td valign="top"><b>Property</b></td><td valign="top"><b>Description</b></td></tr>
</thead>
<tbody>
<tr><td colspan="2" style="background-color: gainsboro"><b>Code Generation</b></td></tr>
<tr><td valign="top">Collection Class</td><td valign="top"><i>String</i>. The concrete class used to implement Zero or More cardinality ends. Defaults to class set at the designer level, but can be overridden here. Must implement ICollection&lt;T&gt;.</td></tr>
<tr><td valign="top">Persistent</td><td valign="top"><i>Boolean</i>. If false, the association will not be persisted in the database, requiring custom code to implement.</td></tr>
<tr><td colspan="2" style="background-color: gainsboro"><b>Documentation</b></td></tr>
<tr><td valign="top">Comment Summary</td><td valign="top"><i>String</i>. XML comment &lt;Summary&gt; section</td></tr>
<tr><td colspan="2" style="background-color: gainsboro"><b>End1</b></td></tr>
<tr><td valign="top">End1 Comment Detail</td><td valign="top"><i>String</i>. XML comment &lt;Remarks&gt; section for this end</td></tr>
<tr><td valign="top">End1 Comment Summary</td><td valign="top"><i>String</i>. XML comment &lt;Summary&gt; section for this end</td></tr>
<tr><td valign="top">End1 Multiplicity</td><td valign="top"><i>String</i>. Cardinality of this end</td></tr>
<tr><td valign="top">End1 On Delete</td><td valign="top"><i>String</i>. How to handle objects on this end if the object on the other end is deleted. Can be programmatically fixed (if the other end has a One cardinality, therefore is required) or manually set.</td></tr>
<tr><td valign="top">End1 Role</td><td valign="top"><i>String</i>. Whether this end is the Principal or Dependent end of the association. For more information, see <a href="https://msdn.microsoft.com/en-us/library/jj713564(v=vs.113).aspx">Entity Framework Relationships and Navigation Properties</a></td></tr>
<tr><td valign="top">End2 Navigation Property</td><td valign="top"><i>String</i>. Name of the property that will be generated in the class on the *other* side of the association.</td></tr>
<tr><td colspan="2" style="background-color: gainsboro"><b>End2</b></td></tr>
<tr><td valign="top">End1 Navigation Property</td><td valign="top"><i>String</i>. Name of the property that will be generated in the class on the *other* side of the association.</td></tr>
<tr><td valign="top">End2 Comment Detail</td><td valign="top"><i>String</i>. XML comment &lt;Remarks&gt; section for this end</td></tr>
<tr><td valign="top">End2 Comment Summary</td><td valign="top"><i>String</i>. XML comment &lt;Summary&gt; section for this end</td></tr>
<tr><td valign="top">End2 Multiplicity</td><td valign="top"><i>String</i>. Cardinality of this end</td></tr>
<tr><td valign="top">End2 On Delete</td><td valign="top"><i>String</i>. How to handle objects on this end if the object on the other end is deleted. Can be programmatically fixed (if the other end has a One cardinality, therefore is required) or manually set.</td></tr>
<tr><td valign="top">End2 Role</td><td valign="top"><i>String</i>. Whether this end is the Principal or Dependent end of the association. For more information, see <a href="https://msdn.microsoft.com/en-us/library/jj713564(v=vs.113).aspx">Entity Framework Relationships and Navigation Properties</a></td></tr>
</tbody>
</table>

### Next Step 
[Inheritance](Inheritance)
