// https://stackoverflow.com/questions/54968182/ef-core-get-navigation-properties-of-an-entity-from-model-with-multiplicity-zero

namespace EFCoreParser
{
   public enum RelationshipMultiplicity
   {
      ZeroOrOne = 0,
      One = 1,
      Many = 2
   }
}