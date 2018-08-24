using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class BidirectionalAssociationBuilder
   {
      private static bool CanAcceptModelClassAsSource(ModelClass candidate)
      {
         return !candidate.IsDependentType;
      }

      private static bool CanAcceptModelClassAsTarget(ModelClass candidate)
      {
         return !candidate.IsDependentType;
      }

      private static bool CanAcceptModelClassAndModelClassAsSourceAndTarget(ModelClass sourceModelClass, ModelClass targetModelClass)
      {
         return !sourceModelClass.IsDependentType && !targetModelClass.IsDependentType;
      }
   }
}
