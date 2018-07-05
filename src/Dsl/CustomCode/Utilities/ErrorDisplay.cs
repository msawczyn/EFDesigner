
namespace Sawczyn.EFDesigner
{
   /// <summary>
   /// This keeps MessageBox out of our DSL project proper. DslPackage calls RegisterDisplayHandler with a method that shows the MessageBox
   /// properly using the Visual Studio service provider.
   /// </summary>
   public  class ErrorDisplay
   {
      public delegate void ErrorVisualizer(string message);

      private static ErrorVisualizer ErrorVisualizerMethod;

      public static void Show(string message)
      {
         if (ErrorVisualizerMethod != null)
         {
            try
            {
               ErrorVisualizerMethod(message);
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
