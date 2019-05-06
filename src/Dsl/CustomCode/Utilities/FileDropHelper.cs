using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.Modeling;
using Sawczyn.EFDesigner.EFModel.Annotations;

namespace Sawczyn.EFDesigner.EFModel
{
   public class FileDropHelper
   {
      public static void HandleDrop(Store store, string filename)
      {
         if (store == null || filename == null) return;

         using (Transaction tx = store.TransactionManager.BeginTransaction("Process dropped class"))
         {
            if (DoHandleDrop(store, filename))
               tx.Commit();
         }
      }

      public static void HandleMultiDrop(Store store, IEnumerable<string> filenames)
      {
         if (store == null || filenames == null) return;

         List<string> filenameList = filenames.ToList();

         foreach (string filename in filenameList)
         {
            using (Transaction tx = store.TransactionManager.BeginTransaction("Process dropped classes"))
            {
               if (DoHandleDrop(store, filename))
                  tx.Commit();
            }
         }
      }

      private static bool DoHandleDrop([NotNull] Store store, [NotNull] string filename)
      {
         if (store == null)
            throw new ArgumentNullException(nameof(store));

         if (string.IsNullOrEmpty(filename))
            throw new ArgumentNullException(nameof(filename));

         // did we drop an assembly
         DroppedAssemblyProcessor droppedAssemblyProcessor = new DroppedAssemblyProcessor(store, filename);

         if (!droppedAssemblyProcessor.ProcessAssembly())
         {
            // or a code file?
            DroppedFileProcessor droppedFileProcessor = new DroppedFileProcessor(store, filename);

            if (!droppedFileProcessor.ProcessFile())
               ErrorDisplay.Show("Error interpreting " + filename);
         }

         return true;
      }
   }
}