namespace ParsingModels
{
   public class ModelBidirectionalAssociation : ModelUnidirectionalAssociation
   {
      public string SourcePropertyName { get; set; }
      public string SourceSummary { get; set; }
      public string SourceDescription { get; set; }
   }
}