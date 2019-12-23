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
      /// Owning class
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

      public bool IsCollection => /*Cardinality == Multiplicity.OneMany || */Cardinality == Multiplicity.ZeroMany;
      public bool Required => Cardinality == Multiplicity.One /*|| Cardinality == Multiplicity.OneMany*/;
      public bool ConstructorParameterOnly { get; set; }
      public string Summary { get; set; }
      public string Description { get; set; }
      public string CustomAttributes { get; set; }
      public string DisplayText { get; set; }
      public bool IsAutoProperty { get; set; }
      public bool ImplementNotify { get; set; }
      public string FKPropertyName { get; set; }
   }
}
