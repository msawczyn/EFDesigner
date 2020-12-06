namespace Sawczyn.EFDesigner.EFModel
{
   /// <summary>
   /// Describes a class property that associates to another persistent entity
   /// </summary>
   public class NavigationProperty
   {

      /// <summary>
      /// Association this is based on
      /// </summary>
      public Association AssociationObject { get; set; }

      /// <summary>
      /// Owning class; the class that has the property
      /// </summary>
      public ModelClass ClassType { get; set; }

      /// <summary>
      /// Name of peoperty in owning class
      /// </summary>
      public string PropertyName { get; set; }

      /// <summary>
      /// Multiplicity of the association
      /// </summary>
      public Multiplicity Cardinality { get; set; }

      /// <summary>
      /// True if the property is a collection, false otherwise
      /// </summary>
      public bool IsCollection => /*Cardinality == Multiplicity.OneMany || */Cardinality == Multiplicity.ZeroMany;

      /// <summary>
      /// True if the property is required, false otherwise
      /// </summary>
      public bool Required => Cardinality == Multiplicity.One /*|| Cardinality == Multiplicity.OneMany*/;

      /// <summary>
      /// True if the property is only used as a constructor parameter (i.e., not usable in the owning class), false otherwise
      /// </summary>
      public bool ConstructorParameterOnly { get; set; }

      /// <summary>
      /// Summary comments
      /// </summary>
      public string Summary { get; set; }

      /// <summary>
      /// Detailed comments
      /// </summary>
      public string Description { get; set; }

      /// <summary>
      /// Any custom attributes to apply to the property
      /// </summary>
      public string CustomAttributes { get; set; }

      /// <summary>
      /// Human-readable text to be used when displaying the property in the designer
      /// </summary>
      public string DisplayText { get; set; }

      /// <summary>
      /// True if this is an autoproperty, false otherwise
      /// </summary>
      public bool IsAutoProperty { get; set; }

      /// <summary>
      /// If not an autoproperty, the name of the field holding the property's value
      /// </summary>
      public string BackingFieldName { get; set; }

      /// <summary>
      /// If not an autoproperty, the visibility of the field holding the property's value
      /// </summary>
      public string BackingFieldPropertyAccessMode { get; set; }

      /// <summary>
      /// If true, an IPropertyChanged notification will be generated
      /// </summary>
      public bool ImplementNotify { get; set; }

      /// <summary>
      /// Exposed foreign key name for this association
      /// </summary>
      public string FKPropertyName { get; set; }
   }
}
