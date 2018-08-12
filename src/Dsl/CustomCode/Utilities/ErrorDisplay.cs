namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// This helps keep UI interaction out of our DSL project proper. DslPackage calls RegisterDisplayHandler with a method that shows the MessageBox
   /// (or other UI-related method) properly using the Visual Studio service provider.
   /// </summary>
   public static class ErrorDisplay
   {
      public delegate void ErrorVisualizer(string message, bool asMessageBox);
      private static ErrorVisualizer ErrorVisualizerMethod;

      public static void Show(string message, bool asMessageBox = true)
      {
         if (ErrorVisualizerMethod != null)
         {
            try
            {
               ErrorVisualizerMethod(message, asMessageBox);
            }
            catch
            {
               // swallow the exception
            }
         }
      }

      public static void RegisterDisplayHandler(ErrorVisualizer method)
      {
         ErrorVisualizerMethod = method;
      }
   }
}
