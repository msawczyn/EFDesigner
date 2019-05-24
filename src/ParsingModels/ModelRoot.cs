using System.Collections.Generic;

namespace ParsingModels
{
   public class ModelRoot
   {
      public ModelRoot()
      {
         Classes = new List<ModelClass>();
         Enumerations = new List<ModelEnum>();
      }

      public string EntityContainerName { get; set; }
      public string Namespace { get; set; }
      public string FullName => string.IsNullOrWhiteSpace(Namespace) ? $"global::{EntityContainerName}" : $"global::{Namespace}.{EntityContainerName}";
      public List<ModelClass> Classes { get; set; }
      public List<ModelEnum> Enumerations { get; set; }
   }
}