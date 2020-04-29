using System;
using System.Windows.Input;

namespace Sawczyn.EFDesigner.EFModel
{
   public class WaitCursor : IDisposable
   {
      private readonly Cursor previousCursor;

      public WaitCursor()
      {
         previousCursor = Mouse.OverrideCursor;
         Mouse.OverrideCursor = Cursors.Wait;
      }

      public void Dispose()
      {
         Mouse.OverrideCursor = previousCursor;
      }
   }
}