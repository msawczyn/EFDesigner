using Microsoft.VisualStudio.Modeling;
// ReSharper disable UnusedParameter.Local
// ReSharper disable ConvertIfStatementToReturnStatement

namespace Sawczyn.EFDesigner.EFModel
{
   partial class UnidirectionalAssociationBuilder
   {
      private static bool CanAcceptModelClassAsSource(ModelClass candidate)
      {
         // valid unidirectional associations:
         // EF6 - entity to entity, entity to dependent
         // EFCore - entity to entity, entity to dependent
         // EFCore5Plus - entity to entity, entity to dependent, dependent to dependent, keyless to entity

         ModelRoot modelRoot = candidate.ModelRoot;
         EFVersion entityFrameworkVersion = modelRoot.EntityFrameworkVersion;

         if (entityFrameworkVersion == EFVersion.EF6)
         {
            if (candidate.IsEntity())
               return true;
         }
         else if (entityFrameworkVersion == EFVersion.EFCore && !modelRoot.IsEFCore5Plus)
         {
            if (candidate.IsEntity())
               return true;
         }
         else if (entityFrameworkVersion == EFVersion.EFCore && modelRoot.IsEFCore5Plus)
         {
            if (candidate.IsEntity())
               return true;

            if (candidate.IsDependent())
               return true;

            if (candidate.IsKeyless())
               return true;
         }

         return false;
      }

      private static bool CanAcceptModelClassAsTarget(ModelClass candidate)
      {
         // valid unidirectional associations:
         // EF6 - entity to entity, entity to dependent
         // EFCore - entity to entity, entity to dependent
         // EFCore5Plus - entity to entity, entity to dependent, dependent to dependent, keyless to entity

         ModelRoot modelRoot = candidate.ModelRoot;
         EFVersion entityFrameworkVersion = modelRoot.EntityFrameworkVersion;

         if (entityFrameworkVersion == EFVersion.EF6)
         {
            if (candidate.IsEntity())
               return true;

            if (candidate.IsDependent())
               return true;
         }
         else if (entityFrameworkVersion == EFVersion.EFCore && !modelRoot.IsEFCore5Plus)
         {
            if (candidate.IsEntity())
               return true;

            if (candidate.IsDependent())
               return true;
         }
         else if (entityFrameworkVersion == EFVersion.EFCore && modelRoot.IsEFCore5Plus)
         {
            if (candidate.IsEntity())
               return true;

            if (candidate.IsDependent())
               return true;
         }

         return false;
      }

      private static bool CanAcceptModelClassAndModelClassAsSourceAndTarget(ModelClass sourceModelClass, ModelClass targetModelClass)
      {
         // keyless types may not have navigations to owned entities
         if (sourceModelClass.IsKeyless() && targetModelClass.IsDependent())
            return false;

         // navigation properties can't point to keyless entity types
         if (targetModelClass.IsKeyless())
            return false;

         // valid unidirectional associations:
         // EF6 - entity to entity, entity to dependent
         // EFCore - entity to entity, entity to dependent
         // EFCore5Plus - entity to entity, entity to dependent, dependent to dependent, keyless to entity

         ModelRoot modelRoot = sourceModelClass.ModelRoot;

         if (modelRoot.EntityFrameworkVersion == EFVersion.EF6)
         {
            if (sourceModelClass.IsEntity() && targetModelClass.IsEntity())
               return true;
            if (sourceModelClass.IsEntity() && targetModelClass.IsDependent())
               return true;
         }
         else if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore && !modelRoot.IsEFCore5Plus)
         {
            if (sourceModelClass.IsEntity() && targetModelClass.IsEntity())
               return true;
            if (sourceModelClass.IsEntity() && targetModelClass.IsDependent())
               return true;
         }
         else if (modelRoot.EntityFrameworkVersion == EFVersion.EFCore && modelRoot.IsEFCore5Plus)
         {
            if (sourceModelClass.IsEntity() && targetModelClass.IsEntity())
               return true;
            if (sourceModelClass.IsEntity() && targetModelClass.IsDependent())
               return true;
            if (sourceModelClass.IsDependent() && targetModelClass.IsDependent())
               return true;
            if (sourceModelClass.IsKeyless() && targetModelClass.IsEntity())
               return true;
         }

         return false;
      }

      private static bool CanAcceptModelAttributeAndModelAttributeAsSourceAndTarget(ModelAttribute sourceModelAttribute, ModelAttribute targetModelAttribute)
      {
         return CanAcceptModelClassAndModelClassAsSourceAndTarget(sourceModelAttribute.ModelClass, targetModelAttribute.ModelClass);
      }

      private static bool CanAcceptModelAttributeAndModelClassAsSourceAndTarget(ModelAttribute sourceModelAttribute, ModelClass targetModelClass)
      {
         return CanAcceptModelClassAndModelClassAsSourceAndTarget(sourceModelAttribute.ModelClass, targetModelClass);
      }

      private static bool CanAcceptModelAttributeAsSource(ModelAttribute candidate)
      {
         return CanAcceptModelClassAsSource(candidate.ModelClass);
      }

      private static bool CanAcceptModelAttributeAsTarget(ModelAttribute candidate)
      {
         return CanAcceptModelClassAsTarget(candidate.ModelClass);
      }

      private static bool CanAcceptModelClassAndModelAttributeAsSourceAndTarget(ModelClass sourceModelClass, ModelAttribute targetModelAttribute)
      {
         return CanAcceptModelClassAndModelClassAsSourceAndTarget(sourceModelClass, targetModelAttribute.ModelClass);
      }

      private static ElementLink ConnectModelAttributeToModelAttribute(ModelAttribute sourceAccepted, ModelAttribute targetAccepted)
      {
         return ConnectModelClassToModelClass(sourceAccepted.ModelClass, targetAccepted.ModelClass);
      }

      private static ElementLink ConnectModelAttributeToModelClass(ModelAttribute sourceAccepted, ModelClass targetAccepted)
      {
         return ConnectModelClassToModelClass(sourceAccepted.ModelClass, targetAccepted);
      }

      private static ElementLink ConnectModelClassToModelAttribute(ModelClass sourceAccepted, ModelAttribute targetAccepted)
      {
         return ConnectModelClassToModelClass(sourceAccepted, targetAccepted.ModelClass);
      }

      private static ElementLink ConnectModelClassToModelClass(ModelClass sourceAccepted, ModelClass targetAccepted)
      {
         ElementLink result = new UnidirectionalAssociation(sourceAccepted, targetAccepted);

         if (DomainClassInfo.HasNameProperty(result))
            DomainClassInfo.SetUniqueName(result);

         return result;
      }

      private static ElementLink ConnectSourceToTarget(ModelElement source, ModelElement target)
      {
         if (source is ModelAttribute sourceAttribute)
         {
            if (target is ModelAttribute targetAttribute)
               return ConnectModelAttributeToModelAttribute(sourceAttribute, targetAttribute);

            if (target is ModelClass targetClass)
               return ConnectModelAttributeToModelClass(sourceAttribute, targetClass);
         }

         if (source is ModelClass sourceClass)
         {
            if (target is ModelAttribute targetAttribute)
               return ConnectModelClassToModelAttribute(sourceClass, targetAttribute);

            if (target is ModelClass targetClass)
               return ConnectModelClassToModelClass(sourceClass, targetClass);
         }

         return null;
      }
   }
}
