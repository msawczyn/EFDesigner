using System.Collections.Generic;

using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   public interface IFileProcessor
   {
      bool Process(string inputFile, out List<ModelElement> newElements);
   }
}