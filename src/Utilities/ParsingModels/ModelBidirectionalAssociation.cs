namespace ParsingModels
{
   public class ModelBidirectionalAssociation : ModelUnidirectionalAssociation
   {
      public string SourcePropertyTypeName { get; set; }
      public string SourcePropertyName { get; set; }
      public string SourceSummary { get; set; }
      public string SourceDescription { get; set; }
   }
}