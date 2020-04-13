# Diagrams (Views) 

The designer supports having multiple views of your model as of v2.0.

You'll always have one view available, and it will be named the same as your DbContext name. You won't be able to delete that view or change its
name (except indirectly, if you change the DbContext name), and it will be the one that opens up when you open your model file.

You can add additional views by right-clicking the root node of the Model Explorer and selecting `Add Diagram`. You'll get a default
name for the diagram (e.g., Diagram1, Diagram2, etc.) but can change that in Visual Studio's Property window. Diagrams appear
empty at first, but you can add new elements to them or choose existing elements from the Model Explorer.

To add a class or enum from the Model Explorer, double click its tree node; the active design window will get that
element added to it. Note that if the element is already on that diagram, it'll be selected and centered on the first
click but it won't be added again.

Any associations (unidirectional, bidirectional, generalization) to other elements on that diagram will 
also automatically be added. But if the other end of that association isn't on the diagram at that moment,
the connector won't appear; you'll only know that the association is there by looking at the Property window
(for the base class) or the element node's `Source` and `Target` compartments (for unidirectional and bidirectional associations).

These extra diagrams can also be deleted. Do that throug the Model Explorer by right-clicking on the diagram
node and selecting `Delete`.

# Hiding, Removing from the Diagram and Deleting Elements

You can take class and enumeration elements off the diagram as well, in three different ways.

- **Hiding:** 

### Next Step 
[Persistent entities](Entities)


