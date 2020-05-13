namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// This helps keep UI interaction out of our DSL project proper. DslPackage calls RegisterDisplayHandler with a method that shows the MessageBox
   /// (or other UI-related method) properly using the Visual Studio service provider.
   /// </summary>
   public static class MessageDisplay
   {
      public delegate void MessageVisualizer(string message);
      private static MessageVisualizer MessageVisualizerMethod;

      public static void Show(string message)
      {
         if (MessageVisualizerMethod != null)
         {
            try
            {
               MessageVisualizerMethod(message);
            }
            catch
            {
               // swallow the exception
            }
         }
      }

      public static void RegisterDisplayHandler(MessageVisualizer method)
      {
         MessageVisualizerMethod = method;
      }

   }
}