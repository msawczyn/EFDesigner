namespace Sawczyn.EFDesigner.EFModel
{
   public class NavigationProperty
   {
      public ModelClass ClassType { get; set; }
      public Association AssociationObject { get; set; }
      public string PropertyName { get; set; }
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
   }
}
