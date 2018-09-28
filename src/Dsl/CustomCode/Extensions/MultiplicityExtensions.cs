namespace Sawczyn.EFDesigner.EFModel
{
   public static class MultiplicityExtensions
   {
      public static string Display(this Multiplicity value)
      {
         switch (value)
         {
            case Multiplicity.One:
               return "1";

            case Multiplicity.ZeroOne:
               return "0..1";

            case Multiplicity.ZeroMany:
               return "0..*";

            default:
               return string.Empty;
         }
      }
   }
}
