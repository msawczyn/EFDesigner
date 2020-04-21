# Diagrams (Views) 

The designer supports having multiple views of your model as of v2.0.

You'll always have one view available, and it will be named the same as your DbContext name. You won't be able to delete that view or change its
name (except indirectly, if you change the DbContext name), and it will be the one that appears when you open your model file.

You can add additional views by right-clicking the root node of the Model Explorer and selecting `Add Diagram`. You'll get a default
name for the diagram (e.g., Diagram1, Diagram2, etc.) but can change that in Visual Studio's Property window. Diagrams appear
empty at first, but you can add new elements to them or choose existing elements from the Model Explorer.

To add a class or enum from the Model Explorer, double click its tree node; the active design window will get that
element added to it. Note that if the element is already on that diagram, it'll be selected and centered on the first
click but it won't be added again.

When you double click the element in the Model Explorer, that element will appear in the top left corner of your diagram. Alternatively,
you can drag/drop the class or enum from the Model Explorer to a location on the diagram of your choosing. If it's already on the diagram,
you won't be able to drop it.

Any associations (unidirectional, bidirectional, generalization) to other elements already on that diagram will 
also automatically be added. But if the other end of that association isn't on the diagram at that moment,
the connector won't appear; you'll only know that the association is there by looking at the Property window
(to see the base class) or the element node's `Source` and `Target` compartments (to see unidirectional and bidirectional associations).

These extra diagrams can also be deleted. Do that through the Model Explorer by right-clicking on the diagram
node and selecting `Delete`.

# Hiding, Removing from the Diagram and Deleting Elements

You can take class and enumeration elements off the diagram as well, in three different ways.

- **Hiding:** Model elements on any diagram can be hidden by selecting them, right-clicking and choosing `Hide Selected Elements`. They'll
still be on the diagram, but invisible.
- **Removing from diagram:** Right-clicking one or more elements and choosing `Remove from diagram` (default hot key: `Ctrl-Del`) will remove those elements 
from that diagram but not from the model. They will still be on other diagrams that were showing them, as well as in the Model Explorer.
- **Deleting:** To delete an element from the model, select one or more, right-click and choose `Delete` (default hot key: `Del`). They'll be completely removed
from your model and all diagrams without confirmation. If you make a mistake, you can undo the operation in the normal way (`Edit/Undo` or `Ctrl-Z`).

### Next Step 
[Persistent entities](Entities)


