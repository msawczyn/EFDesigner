namespace ParsingModels
{
   public class ModelBidirectionalAssociation
   {
      public string TargetClassName { get; set; }
      public string TargetClassNamespace { get; set; }
      public string SourcePropertyTypeName { get; set; }
      public Multiplicity SourceMultiplicity { get; set; }
      public string SourcePropertyName { get; set; }
      public string SourceSummary { get; set; }
      public string SourceDescription { get; set; }

      public string SourceClassName { get; set; }
      public string SourceClassNamespace { get; set; }
      public string TargetPropertyTypeName { get; set; }
      public Multiplicity TargetMultiplicity { get; set; }
      public string TargetPropertyName { get; set; }
      public string TargetSummary { get; set; }
      public string TargetDescription { get; set; }
   }
}