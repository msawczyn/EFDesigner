namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// This helps keep UI interaction out of our DSL project proper. DslPackage calls RegisterDisplayHandler with a method that shows the MessageBox
   /// (or other UI-related method) properly using the Visual Studio service provider.
   /// </summary>
   public static class WarningDisplay
   {
      public delegate void WarningVisualizer(string message);
      private static WarningVisualizer WarningVisualizerMethod;

      public static void Show(string message)
      {
         if (WarningVisualizerMethod != null)
         {
            try
            {
               WarningVisualizerMethod(message);
            }
            catch
            {
               // swallow the exception
            }
         }
      }

      public static void RegisterDisplayHandler(WarningVisualizer method)
      {
         WarningVisualizerMethod = method;
      }

   }
}