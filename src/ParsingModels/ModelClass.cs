using System.Collections.Generic;

namespace ParsingModels
{
   public class ModelClass
   {
      public ModelClass()
      {
         Properties = new List<ModelProperty>();
      }

      public string Name { get; set; }
      public string Namespace { get; set; }
      public string Summary { get; set; }
      public string Description { get; set; }
      public string CustomAttributes { get; set; }
      public bool IsAbstract { get; set; }
      public string BaseClass { get; set; }
      public string TableName { get; set; }
      public bool IsDependentType { get; set; }
      public List<ModelProperty> Properties { get; set; }
   }
}