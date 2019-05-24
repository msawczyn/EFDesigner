namespace ParsingModels
{
   public class ModelUnidirectionalAssociation
   {
      public string SourceClassName { get; set; }
      public string SourceClassNamespace { get; set; }
      public string SourceClassFullName => string.IsNullOrWhiteSpace(SourceClassNamespace) ? $"global::{SourceClassName}" : $"global::{SourceClassNamespace}.{SourceClassName}";
      public Multiplicity SourceMultiplicity { get; set; }

      public string TargetClassName { get; set; }
      public string TargetClassNamespace { get; set; }
      public string TargetClassFullName => string.IsNullOrWhiteSpace(TargetClassNamespace) ? $"global::{TargetClassName}" : $"global::{TargetClassNamespace}.{TargetClassName}";
      public string TargetPropertyTypeName { get; set; }
      public Multiplicity TargetMultiplicity { get; set; }
      public string TargetPropertyName { get; set; }
      public string TargetSummary { get; set; }
      public string TargetDescription { get; set; }
   }
}