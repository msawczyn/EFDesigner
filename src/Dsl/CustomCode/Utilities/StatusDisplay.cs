namespace Sawczyn.EFDesigner
{
   public static class StatusDisplay
   {
      public delegate void StatusVisualizer(string message);
      private static StatusVisualizer StatusVisualizerMethod;

      public static void Show(string message)
      {
         if (StatusVisualizerMethod != null)
         {
            try
            {
               StatusVisualizerMethod(message);
            }
            catch
            {
               // swallow the exception
            }
         }
      }

      public static void RegisterDisplayHandler(StatusVisualizer method)
      {
         StatusVisualizerMethod = method;
      }

   }
}