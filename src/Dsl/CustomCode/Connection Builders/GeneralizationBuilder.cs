namespace Sawczyn.EFDesigner.EFModel
{
   partial class GeneralizationBuilder
   {
      // The Generalization connection tool specifies that source and target should be reversed. 

      private static bool CanAcceptModelClassAsSource(ModelClass candidate)
      {
         // dependent types can't participate in inheritance relationships
         return !candidate.IsDependentType;
      }

      private static bool CanAcceptModelClassAsTarget(ModelClass candidate)
      {
         // dependent types can't participate in inheritance relationships
         // classes can't have > 1 superclass
         return !candidate.IsDependentType && !candidate.ReadOnly && candidate.Superclass == null;
      }

      private static bool CanAcceptModelClassAndModelClassAsSourceAndTarget(ModelClass sourceModelClass, ModelClass targetModelClass)
      {
         // can't have cycles
         for (ModelClass candidate = sourceModelClass; candidate != null; candidate = candidate.Superclass)
         {
            if (candidate == targetModelClass)
               return false;
         }

         return true;
      }

   }
}
