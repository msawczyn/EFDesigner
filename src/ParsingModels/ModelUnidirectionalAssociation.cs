namespace ParsingModels
{
   public class ModelUnidirectionalAssociation
   {
      public string SourceClassName { get; set; }
      public Multiplicity SourceMultiplicity { get; set; }
      public string TargetClassName { get; set; }
      public Multiplicity TargetMultiplicity { get; set; }
      public string TargetPropertyName { get; set; }
      public string TargetSummary { get; set; }
      public string TargetDescription { get; set; }
   }
}