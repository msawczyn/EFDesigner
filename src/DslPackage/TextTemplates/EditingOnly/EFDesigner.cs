using System;

using Microsoft.VisualStudio.TextTemplating;

// ReSharper disable UnusedMember.Global

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   // ReSharper disable once UnusedMember.Global
   public partial class GeneratedTextTransformation
   {
      // stubs for methods provided by the EFModelDirectiveProcessor

      public ModelRoot ModelRoot { get; set; }

      public ITextTemplatingEngineHost Host { get; set; }

      public string PopIndent() { return string.Empty; }

      public void PushIndent(string indent) { }

      public void ClearIndent() { }

      public void WriteLine(string textToAppend) { }

      #region Template

      // EFDesigner v3.0.4
      // Copyright (c) 2017-2021 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public void GenerateEF6(Manager manager, ModelRoot modelRoot)
      {
         if (modelRoot.EntityFrameworkVersion != EFVersion.EF6)
            throw new InvalidOperationException("Wrong generator selected");

         EFModelGenerator generator = new EF6ModelGenerator(this);
         generator.Generate(manager);
      }

      public void GenerateEFCore(Manager manager, ModelRoot modelRoot)
      {
         if (modelRoot.EntityFrameworkVersion != EFVersion.EFCore)
            throw new InvalidOperationException("Wrong generator selected");

         EFModelGenerator generator;

         switch ((int)modelRoot.GetEntityFrameworkPackageVersionNum())
         {
            case 2:
               generator = new EFCore2ModelGenerator(this); break;

            case 3:
               generator = new EFCore3ModelGenerator(this); break;

            default:
               generator = new EFCore5ModelGenerator(this); break;
         }

         generator.Generate(manager);
      }
      #endregion Template
   }
}



