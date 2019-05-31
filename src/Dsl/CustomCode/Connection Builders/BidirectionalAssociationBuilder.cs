namespace Sawczyn.EFDesigner.EFModel
{
   partial class BidirectionalAssociationBuilder
   {
      private static bool CanAcceptModelClassAsSource(ModelClass candidate)
      {
         return !candidate.IsDependentType && !candidate.ReadOnly;
      }

      private static bool CanAcceptModelClassAsTarget(ModelClass candidate)
      {
         return !candidate.IsDependentType && !candidate.ReadOnly;
      }

      private static bool CanAcceptModelClassAndModelClassAsSourceAndTarget(ModelClass sourceModelClass, ModelClass targetModelClass)
      {
         return !sourceModelClass.IsDependentType && !targetModelClass.IsDependentType && !sourceModelClass.ReadOnly && !targetModelClass.ReadOnly;
      }
   }
}
