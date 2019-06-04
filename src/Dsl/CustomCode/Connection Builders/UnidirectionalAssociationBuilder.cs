namespace Sawczyn.EFDesigner.EFModel
{
   partial class UnidirectionalAssociationBuilder
   {
      private static bool CanAcceptModelClassAsSource(ModelClass candidate)
      {
         return !candidate.IsDependentType && !candidate.IsReadOnly;
      }

      private static bool CanAcceptModelClassAsTarget(ModelClass candidate)
      {
         return !candidate.IsReadOnly;
      }

      // ReSharper disable once UnusedParameter.Local
      private static bool CanAcceptModelClassAndModelClassAsSourceAndTarget(ModelClass sourceModelClass, ModelClass targetModelClass)
      {
         return !sourceModelClass.IsDependentType && !sourceModelClass.IsReadOnly && !targetModelClass.IsReadOnly;
      }
   }
}
