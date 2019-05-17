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

      public string Name { get; set; }
      public string Namespace { get; set; }
      public string Summary { get; set; }
      public string Description { get; set; }
      public List<ModelClass> Classes { get; set; }
      public List<ModelEnum> Enumerations { get; set; }
   }
}