using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   internal static class TrackingHelper
   {
      /// <summary>Notify each model element in a collection that a tracked property has changed.</summary>
      /// <param name="store">The store for the model.</param>
      /// <param name="collection">The collection of model elements that contain the tracking property.</param>
      /// <param name="propertyId">The ID of the tracking property.</param>
      /// <param name="trackingPropertyId">The ID of the property that indicates whether the tracking property is tracking.</param>
      internal static void UpdateTrackingCollectionProperty(
         Store store,
         IEnumerable collection,
         Guid propertyId,
         Guid trackingPropertyId)
      {
         DomainPropertyInfo propInfo = store.DomainDataDirectory.GetDomainProperty(propertyId);
         DomainPropertyInfo trackingPropInfo = store.DomainDataDirectory.GetDomainProperty(trackingPropertyId);

         Debug.Assert(propInfo != null);
         Debug.Assert(trackingPropInfo != null);
         Debug.Assert(trackingPropInfo.PropertyType == typeof(bool), "Tracking property not specified as a boolean");

         // If the tracking property is currently tracking, then notify  
         // it that the tracked property has changed.  
         foreach (ModelElement element in collection.Cast<ModelElement>()
                                                    .Where(element => element.GetDomainClass() == trackingPropInfo.DomainClass &&
                                                                      (bool) trackingPropInfo.GetValue(element)))
            propInfo.NotifyValueChange(element);
      }
   }

   /// <summary>
   ///    Helper class to flag critical exceptions from ones that are
   ///    safe to ignore.
   /// </summary>
   internal static class CriticalException
   {
      /// <summary>
      ///    Gets whether an exception is critical and can not be
      ///    ignored.
      /// </summary>
      /// <param name="ex">The exception to check.</param>
      /// <returns>True if the exception is critical.</returns>
      internal static bool IsCriticalException(Exception ex)
      {
         if (ex is NullReferenceException ||
             ex is StackOverflowException ||
             ex is OutOfMemoryException ||
             ex is ThreadAbortException)
            return true;

         return ex.InnerException != null && IsCriticalException(ex.InnerException);
      }
   }
}
