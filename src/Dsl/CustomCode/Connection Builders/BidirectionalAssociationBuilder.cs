using Microsoft.VisualStudio.Modeling;
// ReSharper disable UnusedParameter.Local

namespace Sawczyn.EFDesigner.EFModel
{
   partial class BidirectionalAssociationBuilder
   {
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
         return true;
         //return CanAcceptModelClassAsSource(candidate.ModelClass);
      }

      private static bool CanAcceptModelAttributeAsTarget(ModelAttribute candidate)
      {
         return true;
         //return CanAcceptModelClassAsTarget(candidate.ModelClass);
      }

      private static bool CanAcceptModelClassAndModelAttributeAsSourceAndTarget(ModelClass sourceModelClass, ModelAttribute targetModelAttribute)
      {
         return CanAcceptModelClassAndModelClassAsSourceAndTarget(sourceModelClass, targetModelAttribute.ModelClass);
      }

      private static bool CanAcceptModelClassAndModelClassAsSourceAndTarget(ModelClass sourceModelClass, ModelClass targetModelClass)
      {
         // ReSharper disable ConvertIfStatementToReturnStatement

         // keyless types may not have navigations to owned entities
         if ((sourceModelClass.IsKeylessType() && targetModelClass.IsDependentType)
          || (targetModelClass.IsKeylessType() && sourceModelClass.IsDependentType))
            return false;

         // keyless types can only contain reference navigation properties pointing to regular entities
         if (sourceModelClass.IsKeylessType() && targetModelClass.IsKeylessType())
            return false;

         // Entities cannot contain navigation properties to keyless entity types
         if ((!sourceModelClass.IsKeylessType() && targetModelClass.IsKeylessType())
          || (!targetModelClass.IsKeylessType() && sourceModelClass.IsKeylessType()))
            return false;

         // nested dependent types aren't allowed prior to EFCore5
         if (!sourceModelClass.ModelRoot.IsEFCore5Plus && sourceModelClass.IsDependentType && targetModelClass.IsDependentType)
            return false;

         return true;

         // ReSharper restore ConvertIfStatementToReturnStatement
      }

      private static bool CanAcceptModelClassAsSource(ModelClass candidate)
      {
         return true;
         //return candidate.ModelRoot.IsEFCore5Plus || !candidate.IsDependentType;
      }

      private static bool CanAcceptModelClassAsTarget(ModelClass candidate)
      {
         return true;
         //return candidate.ModelRoot.IsEFCore5Plus || !candidate.IsDependentType;
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
         ElementLink result = new BidirectionalAssociation(sourceAccepted, targetAccepted);

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