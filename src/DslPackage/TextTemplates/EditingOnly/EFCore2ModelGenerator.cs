using System.Collections.Generic;

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   public partial class HostingEnvironment
   {
      #region Template
      // EFDesigner v3.0.0.1
      // Copyright (c) 2017-2020 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class EFCore2ModelGenerator : EFCoreModelGenerator
      {
         public EFCore2ModelGenerator(ModelRoot modelRoot) : base(modelRoot) { }

         public override void Generate(EFModelFileManager efModelFileManager) { }

         protected override List<string> GetAdditionalUsingStatements()
         {
            return null;
         }
      }
      #endregion Template
   }

}
