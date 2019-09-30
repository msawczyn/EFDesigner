using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public abstract partial class EFModelSerializationHelperBase
   {
      internal static void ResetTrackingProperties(Store store)
      {
         // Two passes required - one to set all elements to storage-based then another to set 
         // some back to being tracking.  
         /************************************************/

         foreach (ModelClass element in store.ElementDirectory.FindElements<ModelClass>())
            element.PreResetIsTrackingProperties();

         foreach (ModelClass element in store.ElementDirectory.FindElements<ModelClass>())
            element.ResetIsTrackingProperties();

         /************************************************/

         foreach (ModelEnum element in store.ElementDirectory.FindElements<ModelEnum>())
            element.PreResetIsTrackingProperties();

         foreach (ModelEnum element in store.ElementDirectory.FindElements<ModelEnum>())
            element.ResetIsTrackingProperties();

         /************************************************/

         foreach (ModelAttribute element in store.ElementDirectory.FindElements<ModelAttribute>())
            element.PreResetIsTrackingProperties();

         foreach (ModelAttribute element in store.ElementDirectory.FindElements<ModelAttribute>())
            element.ResetIsTrackingProperties();

         /************************************************/
         // doubles as reset/prereset for both Association and BidirectionalAssociation

         foreach (Association element in store.ElementDirectory.FindElements<Association>())
            element.PreResetIsTrackingProperties();

         foreach (Association element in store.ElementDirectory.FindElements<Association>())
            element.ResetIsTrackingProperties();

      }

      /// <summary>Customize model loading.</summary>
      /// <param name="serializationResult">The serialization result from the load operation.</param>
      /// <param name="partition">The partition in which the new instance was created.</param>
      /// <param name="fileName">The name of the file from which the instance was deserialized.</param>
      /// <param name="modelRoot">The root of the file that was loaded.</param>
      [SuppressMessage("ReSharper", "UnusedParameter.Local")]
      private void OnPostLoadModel(SerializationResult serializationResult, Partition partition, string fileName, ModelRoot modelRoot) { }

      /// <summary>Customize model and diagram loading.</summary>
      /// <param name="serializationResult">Stores serialization result from the load operation. </param>
      /// <param name="modelPartition">Partition in which the new instance will be created. </param>
      /// <param name="modelFileName">Name of the file from which the instance will be deserialized. </param>
      /// <param name="diagramPartition">Partition in which the new diagram instance will be created. </param>
      /// <param name="diagramFileName">Name of the file from which thediagram instance will be deserialized. </param>
      /// <param name="modelRoot">The root of the file that was loaded.</param>
      /// <param name="diagram">The diagram matching the modelRoot.</param>
      [SuppressMessage("ReSharper", "UnusedParameter.Local")]
      private void OnPostLoadModelAndDiagram(SerializationResult serializationResult, Partition modelPartition, string modelFileName, Partition diagramPartition, string diagramFileName, ModelRoot modelRoot, EFModelDiagram diagram)
      {
         Debug.Assert(modelPartition != null);
         Debug.Assert(modelPartition.Store != null);

         // Tracking properties need to be set up according to whether the serialization matches the calculated values.  
         ResetTrackingProperties(modelPartition.Store);
      }
   }
}
