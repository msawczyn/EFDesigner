using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;

// ReSharper disable RedundantNameQualifier

namespace Sawczyn.EFDesigner.EFModel.DslPackage.TextTemplates.EditingOnly
{
   [SuppressMessage("ReSharper", "UnusedMember.Local")]
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   partial class EditOnly
   {

      #region Template

      // EFDesigner v2.1.0.4
      // Copyright (c) 2017-2020 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      void GenerateEF6(Manager manager, ModelRoot modelRoot)
      {
         EF6ModelGenerator generator = new EF6ModelGenerator(manager, modelRoot);
         generator.Generate();
      }
      void GenerateEFCore(Manager manager, ModelRoot modelRoot)
      {
         EFCoreModelGenerator generator = new EFCoreModelGenerator(manager, modelRoot);
         generator.Generate();
      }

      #endregion Template      
   }
}

