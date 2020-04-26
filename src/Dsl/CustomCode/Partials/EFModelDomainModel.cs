using System.Diagnostics;
using System.Resources;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class EFModelDomainModel
   {
      private static ResourceManager auxResourceManager;

      /// <summary>
      ///    Gets the Singleton ResourceManager for this domain model.
      /// </summary>
      public static ResourceManager AuxResourceManager
      {
         [DebuggerStepThrough]
         get
         {
            return auxResourceManager 
                ?? (auxResourceManager = new ResourceManager("Sawczyn.EFDesigner.EFModel.Resources", typeof(EFModelDomainModel).Assembly));
         }
      }
   }
}