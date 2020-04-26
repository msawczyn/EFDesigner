# Diagrams (Views) 

As of v2.0 the designer supports having multiple views of your model.

### Concepts

This is a good time to touch on the difference between the Model Explorer and diagrams.

While diagrams are the interface you'll most often use to edit your model, think of them as a view onto a
facet of the model. They don't necessarily show all the classes and enums available, and only show associations
and generalizations if both ends of the connection are present and visible.

The Model Explorer, on the other hand, always shows all the classes and enums you have created. It's the source
of truth of your model elements, even though it doesn't show the connections. While technically it's also a view
of your model, it's a view of the complete set of model classes and enumerations, and removing an element from
the Model Explorer removes it from the model itself.

You'll always have one diagram view available, your primary diagram, and it will be named the same as your DbContext name. 
You won't be able to delete that view or change its name (except indirectly, by changing the DbContext name), and it will be the 
one that appears when you open your model file.

You can add additional diagrams by right-clicking the root node of the Model Explorer and selecting `Add Diagram`. You'll get a default
name for the new diagram (e.g., Diagram1, Diagram2, etc.) but you can change that in Visual Studio's Property window. Diagrams appear
empty at first, but you can add new elements to them or choose existing elements from the Model Explorer.

The extra diagrams can also be deleted, but the primary diagram cannot. You can delete a diagram using the Model Explorer by 
right-clicking on the diagram node and selecting `Delete`.

### Adding elements

To add a existing class or enum from the Model Explorer, double click its tree node; the active diagram will get that
element added to it in its top left corner. Note that if the element is already present on that diagram, even if hidden, 
it won't be added again.

For more control over placement, you can left-click, hold and drag the class or enum from the Model Explorer, dropping it at a 
location on the diagram of your choosing. If it's already on the diagram, the cursor will change to indicate that you can't drop it there.

For classes, any connectors (unidirectional and bidirectional associations, as well as generalizations and comment connectors) to other 
elements already present on that particular diagram will be drawn automatically. If the element at other end of that connector isn't on 
the diagram at that moment, though, its connector won't appear; you'll only know that the association is there by looking at the 
Property window (to see the base class) or the class's `Source` and `Target` compartments (for unidirectional and bidirectional 
associations).

### Hiding vs Removing from the Diagram vs Deleting Elements

You can remove class and enumeration elements from the diagram as well, in three different ways.

- **Hiding:** Model elements on any diagram can be hidden by selecting them, right-clicking and choosing `Hide Selected Elements`. They'll
still be on the diagram, but invisible. Because they're still present, they can't be added again.
- **Removing from diagram:** Right-clicking one or more elements and choosing `Remove from diagram` (default hot key: `Ctrl-Del`) will remove those elements 
from that diagram but not from the model. They will still be on other diagrams that were showing them, if any, as well as in the Model Explorer.
- **Deleting:** To delete an element from the model, select one or more of them, right-click and choose `Delete` (default hot key: `Del`). 
They'll be completely removed from your model and all diagrams _without confirmation_. If you make a mistake, you can undo the operation in the normal 
way (`Edit/Undo` or `Ctrl-Z`).

### Moving between diagrams ###

You can **copy** elements in a diagram (default hot key: `Ctrl-C`) and **paste** them (default hot key: `Ctrl-V`) into another diagram _in the same model_. 
Relative position information won't be copied over, and the pasted elements will be selected after the paste operation.

If you **cut** elements from a diagram, you are effectively copying the element into the clipboard and then deleting it from the model. You can then paste them in
another diagram in the same model just like a copy operation.

Copying elements between diagrams that live in _different_ models isn't supported, but is a feature in the backlog that is being considered for
future implementation.

### Next Step 
[Persistent entities](Entities)


