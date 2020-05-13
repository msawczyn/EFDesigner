using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   public static class FileDropHelper
   {
      public const int BAD_ARGUMENT_COUNT = 1;
      public const int CANNOT_LOAD_ASSEMBLY = 2;
      public const int CANNOT_WRITE_OUTPUTFILE = 3;
      public const int CANNOT_CREATE_DBCONTEXT = 4;
      public const int CANNOT_FIND_APPROPRIATE_CONSTRUCTOR = 5;
      public const int AMBIGUOUS_REQUEST = 6;

      public static IEnumerable<ModelElement> HandleMultiDrop(Store store, IEnumerable<string> filenames)
      {
         List<ModelElement> newElements = new List<ModelElement>();
         List<string> filenameList = filenames?.ToList();

         if (store == null || filenameList == null)
            return newElements;

         try
         {
            StatusDisplay.Show($"Processing {filenameList.Count} files");

            AssemblyProcessor assemblyProcessor = new AssemblyProcessor(store);
            TextFileProcessor textFileProcessor = new TextFileProcessor(store);

            try
            {
               // may not work. Might not be a text file
               textFileProcessor.LoadCache(filenameList);
            }
            catch
            {
               // if not, no big deal. Either it's not a text file, or we'll just process suboptimally
            }

            foreach (string filename in filenameList)
               newElements.AddRange(Process(store, filename, assemblyProcessor, textFileProcessor));
         }
         catch (Exception e)
         {
            ErrorDisplay.Show(store, e.Message);
         }
         finally
         {
            StatusDisplay.Show(string.Empty);
         }

         return newElements;
      }

      private static bool IsAssembly(string filename)
      {
         try
         {
            AssemblyName.GetAssemblyName(filename);
         }
         catch (BadImageFormatException)
         {
            return false;
         }

         return true;
      }

      private static IEnumerable<ModelElement> Process(Store store, string filename, AssemblyProcessor assemblyProcessor, TextFileProcessor textFileProcessor)
      {
         List<ModelElement> newElements;
         Cursor prev = Cursor.Current;

         try
         {
            Cursor.Current = Cursors.WaitCursor;
            ModelRoot.BatchUpdating = true;

            using (Transaction tx = store.TransactionManager.BeginTransaction("Process drop"))
            {
               bool processingResult = IsAssembly(filename)
                                          ? assemblyProcessor.Process(filename, out newElements)
                                          : textFileProcessor.Process(filename, out newElements);

               if (processingResult)
               {
                  StatusDisplay.Show("Creating diagram elements. This might take a while...");
                  tx.Commit();
               }
               else
                  newElements = new List<ModelElement>();
            }
         }
         finally
         {
            Cursor.Current = prev;
            ModelRoot.BatchUpdating = false;

            StatusDisplay.Show("");
         }

         return newElements;
      }
   }
}