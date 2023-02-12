using System.Collections.Generic;

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   public partial class GeneratedTextTransformation
   {
      #region Template

      // EFDesigner v4.1.2.0
      // Copyright (c) 2017-2022 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class EFCore3ModelGenerator : EFCore2ModelGenerator
      {
         public EFCore3ModelGenerator(GeneratedTextTransformation host) : base(host) { }

         protected override void WriteTargetDeleteBehavior(Association association, List<string> segments)
         {
            if (!association.Source.IsDependentType
             && !association.Target.IsDependentType
             && (association.TargetRole == EndpointRole.Principal || association.SourceRole == EndpointRole.Principal))
            {
               DeleteAction deleteAction = association.SourceRole == EndpointRole.Principal
                                              ? association.SourceDeleteAction
                                              : association.TargetDeleteAction;

               switch (deleteAction)
               {
                  case DeleteAction.None:
                     segments.Add("OnDelete(DeleteBehavior.NoAction)");

                     break;

                  case DeleteAction.Cascade:
                     segments.Add("OnDelete(DeleteBehavior.Cascade)");

                     break;
               }
            }
         }

         protected override void WriteSourceDeleteBehavior(BidirectionalAssociation association, List<string> segments)
         {
            if (!association.Source.IsDependentType
             && !association.Target.IsDependentType
             && (association.TargetRole == EndpointRole.Principal || association.SourceRole == EndpointRole.Principal))
            {
               DeleteAction deleteAction = association.SourceRole == EndpointRole.Principal
                                              ? association.SourceDeleteAction
                                              : association.TargetDeleteAction;

               switch (deleteAction)
               {
                  case DeleteAction.None:
                     segments.Add("OnDelete(DeleteBehavior.NoAction)");

                     break;

                  case DeleteAction.Cascade:
                     segments.Add("OnDelete(DeleteBehavior.Cascade)");

                     break;
               }
            }
         }
      }

      #endregion Template
   }
}


