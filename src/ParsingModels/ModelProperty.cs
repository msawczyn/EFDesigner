namespace ParsingModels
{
   public class ModelProperty
   {
      public string TypeName { get; set; }
      public string Name { get; set; }
      public string CustomAttributes { get; set; }
      public bool Indexed { get; set; }
      public bool Required { get; set; }
      public int MaxStringLength { get; set; }
      public int MinStringLength { get; set; }
      public bool IsIdentity { get; set; }
   }
}