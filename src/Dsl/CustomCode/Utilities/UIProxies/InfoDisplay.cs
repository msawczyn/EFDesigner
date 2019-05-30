namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// This helps keep UI interaction out of our DSL project proper. DslPackage calls RegisterDisplayHandler with a method that shows the MessageBox
   /// (or other UI-related method) properly using the Visual Studio service provider.
   /// </summary>
   public static class InfoDisplay
   {
      public delegate void InfoVisualizer(string message);
      private static InfoVisualizer InfoVisualizerMethod;

      public static void Show(string message)
      {
         if (InfoVisualizerMethod != null)
         {
            try
            {
               InfoVisualizerMethod(message);
            }
            catch
            {
               // swallow the exception
            }
         }
      }

      public static void RegisterDisplayHandler(InfoVisualizer method)
      {
         InfoVisualizerMethod = method;
      }

   }
}