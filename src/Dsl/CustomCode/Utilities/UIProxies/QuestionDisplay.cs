namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// This helps keep UI interaction out of our DSL project proper. DslPackage calls RegisterDisplayHandler with a method that shows the MessageBox
   /// (or other UI-related method) properly using the Visual Studio service provider.
   /// </summary>
   public static class QuestionDisplay
   {
      public delegate bool QuestionVisualizer(string message);
      private static QuestionVisualizer QuestionVisualizerMethod;

      public static bool? Show(string message)
      {
         if (QuestionVisualizerMethod != null)
         {
            try
            {
               return QuestionVisualizerMethod(message);
            }
            catch
            {
               return null;
            }
         }
         return null;
      }

      public static void RegisterDisplayHandler(QuestionVisualizer method)
      {
         QuestionVisualizerMethod = method;
      }
   }
}