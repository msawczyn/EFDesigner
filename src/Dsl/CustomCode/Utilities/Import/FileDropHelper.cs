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

      public static void HandleDrop(Store store, string filename)
      {
         if (store == null || filename == null)
            return;

         try
         {
            StatusDisplay.Show($"Reading {filename}");

            AssemblyProcessor assemblyProcessor = new AssemblyProcessor(store);
            TextFileProcessor textFileProcessor = new TextFileProcessor(store);
            textFileProcessor.LoadCache(filename);

            Process(store, filename, assemblyProcessor, textFileProcessor);
         }
         catch (Exception e)
         {
            ErrorDisplay.Show(e.Message);
         }
         finally
         {
            StatusDisplay.Show(string.Empty);
         }
      }

      public static void HandleMultiDrop(Store store, IEnumerable<string> filenames)
      {
         List<string> filenameList = filenames?.ToList();

         if (store == null || filenameList == null)
            return;

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
               Process(store, filename, assemblyProcessor, textFileProcessor);
         }
         catch (Exception e)
         {
            ErrorDisplay.Show(e.Message);
         }
         finally
         {
            StatusDisplay.Show(string.Empty);
         }
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

      private static void Process(Store store, string filename, AssemblyProcessor assemblyProcessor, TextFileProcessor textFileProcessor)
      {
         try
         {
            Cursor.Current = Cursors.WaitCursor;

            if (IsAssembly(filename))
            {
               using (Transaction tx = store.TransactionManager.BeginTransaction("Process dropped assembly"))
               {
                  if (assemblyProcessor.Process(filename))
                  {
                     StatusDisplay.Show("Creating diagram elements. This might take a while...");
                     tx.Commit();

                     ModelDisplay.LayoutDiagram(store.ModelRoot().GetActiveDiagram() as EFModelDiagram);
                  }
               }
            }
            else
            {
               using (Transaction tx = store.TransactionManager.BeginTransaction("Process dropped class"))
               {
                  if (textFileProcessor.Process(filename))
                  {
                     StatusDisplay.Show("Creating diagram elements. This might take a while...");
                     tx.Commit();
                  }
               }
            }
         }
         finally
         {
            Cursor.Current = Cursors.Default;
            StatusDisplay.Show("");
         }
      }
   }
}