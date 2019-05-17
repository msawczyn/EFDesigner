namespace ParsingModels
{
   public class ModelUnidirectionalAssociation
   {
      public ModelClass Source { get; set; }
      public Multiplicity SourceMultiplicity { get; set; }
      public AssociationRole SourceRole { get; set; }
      public ModelClass Target { get; set; }
      public Multiplicity TargetMultiplicity { get; set; }
      public AssociationRole TargetRole { get; set; }
      public string TargetPropertyName { get; set; }
      public string TargetSummary { get; set; }
      public string TargetDescription { get; set; }
   }
}