namespace Sawczyn.EFDesigner {
   public class WarningDisplay
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