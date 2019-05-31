namespace Sawczyn.EFDesigner.EFModel
{
   partial class UnidirectionalAssociationBuilder
   {
      private static bool CanAcceptModelClassAsSource(ModelClass candidate)
      {
         return !candidate.IsDependentType && !candidate.ReadOnly;
      }

      private static bool CanAcceptModelClassAsTarget(ModelClass candidate)
      {
         return !candidate.ReadOnly;
      }

      // ReSharper disable once UnusedParameter.Local
      private static bool CanAcceptModelClassAndModelClassAsSourceAndTarget(ModelClass sourceModelClass, ModelClass targetModelClass)
      {
         return !sourceModelClass.IsDependentType && !sourceModelClass.ReadOnly && !targetModelClass.ReadOnly;
      }
   }
}
