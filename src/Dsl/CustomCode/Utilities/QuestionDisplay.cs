namespace Sawczyn.EFDesigner {
   public class QuestionDisplay
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