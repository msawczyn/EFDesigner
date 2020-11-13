using System;
using System.Text;

using Microsoft.VisualStudio.TextTemplating;
// ReSharper disable UnusedMember.Global

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   // ReSharper disable once UnusedMember.Global
   public partial class HostingEnvironment
   {
      public static ModelRoot ModelRoot { get; set; }

      public static ITextTemplatingEngineHost Host { get; set; }

      public static string PopIndent() { return string.Empty; }

      public static void PushIndent(string indent) { }

      public static void WriteLine(string textToAppend) { }

      #region Template
      // EFDesigner v3.0.0.1
      // Copyright (c) 2017-2020 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public void GenerateEF6(EFModelFileManager manager, ModelRoot modelRoot)
      {
         if (modelRoot.EntityFrameworkVersion != EFVersion.EF6)
            throw new InvalidOperationException("Wrong generator selected");

         EFModelGenerator generator = new EF6ModelGenerator(modelRoot);
         generator.Generate(manager);
      }

      public void GenerateEFCore(EFModelFileManager manager, ModelRoot modelRoot)
      {
         if (modelRoot.EntityFrameworkVersion != EFVersion.EFCore)
            throw new InvalidOperationException("Wrong generator selected");

         EFModelGenerator generator;

         switch ((int)modelRoot.GetEntityFrameworkPackageVersionNum())
         {
            case 2:
               generator = new EFCore2ModelGenerator(modelRoot);

               break;

            case 3:
               generator = new EFCore3ModelGenerator(modelRoot);

               break;

            case 5:
               generator = new EFCore5ModelGenerator(modelRoot);

               break;

            default:
               throw new InvalidOperationException("Unsupported EFCore version");
         }

         generator.Generate(manager);
      }

      public class Manager : EFModelFileManager
      {
         protected Manager(ITextTemplatingEngineHost host, StringBuilder template) : base(host, template) { }
      }
      #endregion
   }
}
