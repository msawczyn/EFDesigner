namespace ParsingModels
{
   public class ModelProperty
   {
      public string TypeName { get; set; }
      public string Name { get; set; }
      public string Summary { get; set; }
      public string Description { get; set; }
      public string CustomAttributes { get; set; }
      public string InitialValue { get; set; }
      public bool Indexed { get; set; }
      public bool IndexedUnique { get; set; }
      public Visibility SetterVisibility { get; set; }
      public bool Virtual { get; set; }
      public bool ReadOnly { get; set; }
      public bool AutoProperty { get; set; }
      public bool Required { get; set; }
      public int MaxStringLength { get; set; }
      public int MinStringLength { get; set; }
      public bool IsIdentity { get; set; }
   }
}