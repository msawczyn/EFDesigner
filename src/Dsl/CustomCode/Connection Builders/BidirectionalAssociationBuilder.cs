namespace Sawczyn.EFDesigner.EFModel
{
   partial class BidirectionalAssociationBuilder
   {
      private static bool CanAcceptModelClassAsSource(ModelClass candidate)
      {
         return !candidate.IsDependentType && (candidate.ModelRoot.BypassReadOnlyChecks || !candidate.IsReadOnly);
      }

      private static bool CanAcceptModelClassAsTarget(ModelClass candidate)
      {
         return !candidate.IsDependentType && (candidate.ModelRoot.BypassReadOnlyChecks || !candidate.IsReadOnly);
      }

      private static bool CanAcceptModelClassAndModelClassAsSourceAndTarget(ModelClass sourceModelClass, ModelClass targetModelClass)
      {
         return !sourceModelClass.IsDependentType && 
                !targetModelClass.IsDependentType && 
                (sourceModelClass.ModelRoot.BypassReadOnlyChecks || 
                 (!sourceModelClass.IsReadOnly && !targetModelClass.IsReadOnly));
      }
   }
}
