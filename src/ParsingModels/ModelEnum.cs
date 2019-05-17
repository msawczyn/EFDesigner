using System.Collections.Generic;

namespace ParsingModels
{
   public class ModelEnum
   {
      public ModelEnum()
      {
         Values = new List<ModelEnumValue>();
      }

      public string Name { get; set; }
      public string Namespace { get; set; }
      public string Summary { get; set; }
      public string Description { get; set; }
      public string CustomAttributes { get; set; }
      public List<ModelEnumValue> Values { get; set; }
      public string ValueType { get; set; }
      public bool IsFlags { get; set; }
   }
}