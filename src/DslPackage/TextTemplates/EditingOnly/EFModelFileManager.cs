using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;

using EnvDTE;

using Microsoft.VisualStudio.TextTemplating;

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   [SuppressMessage("ReSharper", "UnusedMember.Global")]
   public partial class GeneratedTextTransformation
   {
      #region Template

      // EFDesigner v4.1.2.0
      // Copyright (c) 2017-2022 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      public class Manager
      {
         private readonly List<Block> files = new List<Block>();
         private readonly Block footer = new Block();
         private readonly List<string> generatedFileNames = new List<string>();
         private readonly Block header = new Block();
         private readonly ITextTemplatingEngineHost host;
         private readonly StringBuilder template;

         private Block currentBlock;

         private string fileNameMarker;

         protected Manager(ITextTemplatingEngineHost host, StringBuilder template)
         {
            this.host = host;
            this.template = template;
            fileNameMarker = ".generated";
         }

         public string FileNameMarker
         {
            get
            {
               return fileNameMarker;
            }
            set
            {
               if (string.IsNullOrEmpty(value))
                  fileNameMarker = string.Empty;
               else if (!value.StartsWith("."))
                  fileNameMarker = $".{value}";
               else
                  fileNameMarker = value;
            }
         }

         public virtual string OutputPath
         {
            get { return Path.GetDirectoryName(host.TemplateFile); }
         }

         public virtual string DefaultProjectNamespace
         {
            get { return null; }
         }

         private Block CurrentBlock
         {
            get { return currentBlock; }
            set
            {
               if (CurrentBlock != null)
                  EndBlock();

               if (value != null)
                  value.Start = template.Length;

               currentBlock = value;
            }
         }

         public static Manager Create(ITextTemplatingEngineHost host, StringBuilder template)
         {
            return host is IServiceProvider
                      ? new VSManager(host, template)
                      : new Manager(host, template);
         }

         protected virtual void CreateFile(string fileName, string content)
         {
            if (IsFileContentDifferent(fileName, content))
               File.WriteAllText(fileName, content);
         }

         private void EndBlock()
         {
            if (CurrentBlock == null)
               return;

            CurrentBlock.Length = template.Length - CurrentBlock.Start;

            if (CurrentBlock != header && CurrentBlock != footer)
               files.Add(CurrentBlock);

            currentBlock = null;
         }

         public virtual string GetCustomToolNamespace(string fileName)
         {
            return null;
         }

         private bool IsFileContentDifferent(string fileName, string newContent)
         {
            return !(File.Exists(fileName) && File.ReadAllText(fileName) == newContent);
         }

         public virtual void Process(bool split)
         {
            if (split)
            {
               EndBlock();
               string headerText = template.ToString(header.Start, header.Length);
               string footerText = template.ToString(footer.Start, footer.Length);
               files.Reverse();

               if (!footer.IncludeInDefault)
                  template.Remove(footer.Start, footer.Length);

               foreach (Block block in files)
               {
                  string fileName = Path.Combine(OutputPath, block.Name);
                  string content = headerText + template.ToString(block.Start, block.Length) + footerText;
                  generatedFileNames.Add(fileName);
                  CreateFile(fileName, content);
                  template.Remove(block.Start, block.Length);
               }

               if (!header.IncludeInDefault)
                  template.Remove(header.Start, header.Length);
            }
         }

         public void StartFooter(bool includeInDefault = true)
         {
            CurrentBlock = footer;
            footer.IncludeInDefault = includeInDefault;
         }

         public void StartHeader(bool includeInDefault = true)
         {
            CurrentBlock = header;
            header.IncludeInDefault = includeInDefault;
         }

         public void StartNewFile(string name)
         {
            if (name == null)
               throw new ArgumentNullException(nameof(name));

            CurrentBlock = new Block { Name = name };
         }

         private class Block
         {
            public bool IncludeInDefault;
            public string Name;

            public int Start
                     , Length;
         }

         private class VSManager : Manager
         {
            private readonly DTE dte;
            private readonly ProjectItem templateProjectItem;

            internal VSManager(ITextTemplatingEngineHost host, StringBuilder template) : base(host, template)
            {
               IServiceProvider hostServiceProvider = (IServiceProvider)host;

               if (hostServiceProvider == null)
                  throw new ArgumentNullException(nameof(host));

               dte = (DTE)hostServiceProvider.GetCOMService(typeof(DTE));
               templateProjectItem = dte.Solution.FindProjectItem(host.TemplateFile);
            }

            public override string DefaultProjectNamespace
            {
               get { return templateProjectItem.ContainingProject.Properties.Item("DefaultNamespace").Value.ToString(); }
            }

            public override string OutputPath
            {
               get { return Path.GetDirectoryName(templateProjectItem.ContainingProject.FullName); }
            }

            private void CheckoutFileIfRequired(string fileName)
            {
               EnvDTE.SourceControl sc = dte.SourceControl;

               if (sc != null && sc.IsItemUnderSCC(fileName) && !sc.IsItemCheckedOut(fileName))
                  sc.CheckOutItem(fileName);
            }

            protected override void CreateFile(string fileName, string content)
            {
               string directory = Path.GetDirectoryName(fileName);

               if (!Directory.Exists(directory))
                  Directory.CreateDirectory(directory);

               if (IsFileContentDifferent(fileName, content))
               {
                  CheckoutFileIfRequired(fileName);
                  File.WriteAllText(fileName, content);
               }
            }

            private Dictionary<ProjectItem, List<string>> GetCurrentState()
            {
               Dictionary<ProjectItem, List<string>> result = new Dictionary<ProjectItem, List<string>>();
               Project currentProject = templateProjectItem.ContainingProject;
               string projectDirectory = Path.GetDirectoryName(currentProject.FullName);
               string[] existingGeneratedFiles = Directory.GetFiles(projectDirectory, $"*{FileNameMarker}.cs", SearchOption.AllDirectories);

               foreach (string fileName in existingGeneratedFiles)
               {
                  ProjectItem fileItem = dte.Solution.FindProjectItem(fileName);

                  if (fileItem != null)
                  {
                     try
                     {
                        if (fileItem.Collection.Parent is ProjectItem parentItem)
                        {
                           if (!result.ContainsKey(parentItem))
                              result.Add(parentItem, new List<string>());

                           result[parentItem].Add(fileName);
                        }
                     }
                     catch (InvalidCastException) { }
                  }
               }

               return result;
            }

            private ProjectItem GetOrCreateParentItem(string filePath)
            {
               if (string.IsNullOrEmpty(filePath))
                  return templateProjectItem;

               string projectDirectory = Path.GetDirectoryName(templateProjectItem.ContainingProject.FullName);
               string fileDirectory = Path.GetDirectoryName(filePath);

               if (fileDirectory.ToLower() == projectDirectory.ToLower())
                  return templateProjectItem;

               ProjectItem result = templateProjectItem;

               string relativeFilePath = fileDirectory.Substring(projectDirectory.Length + 1);
               Queue<string> pathParts = new Queue<string>(relativeFilePath.Split('\\'));
               ProjectItems currentItemList = templateProjectItem.ContainingProject.ProjectItems;

               while (pathParts.Any())
               {
                  bool found = false;
                  string pathPart = pathParts.Dequeue();

                  for (int index = 1; index <= currentItemList.Count; ++index)
                  {
                     ProjectItem item = currentItemList.Item(index);

                     if (item.Kind == EnvDTE.Constants.vsProjectItemKindPhysicalFolder && item.Name == pathPart)
                     {
                        if (!pathParts.Any())
                           result = item;
                        else
                           currentItemList = item.ProjectItems;

                        found = true;

                        break;
                     }
                  }

                  if (!found)
                  {
                     ProjectItem newItem = currentItemList.AddFolder(pathPart);

                     if (!pathParts.Any())
                        result = newItem;
                     else
                        currentItemList = newItem.ProjectItems;
                  }
               }

               return result;
            }

            private Dictionary<ProjectItem, List<string>> GetTargetState(string[] fileNames)
            {
               Dictionary<ProjectItem, List<string>> result = new Dictionary<ProjectItem, List<string>> { { templateProjectItem, new List<string>() } };

               foreach (string fileName in fileNames)
               {
                  ProjectItem parentItem = GetOrCreateParentItem(fileName);

                  if (!result.ContainsKey(parentItem))
                     result.Add(parentItem, new List<string>());

                  result[parentItem].Add(fileName);
               }

               return result;
            }

            public override void Process(bool split)
            {
               if (templateProjectItem.ProjectItems == null)
                  return;

               base.Process(split);
               ProjectSync(generatedFileNames);
            }

            private void ProjectSync(IEnumerable<string> keepFileNames)
            {
               Dictionary<ProjectItem, List<string>> current = GetCurrentState();

               string[] fileNames = keepFileNames as string[] ?? keepFileNames.ToArray();
               Dictionary<ProjectItem, List<string>> target = GetTargetState(fileNames);
               List<string> allTargetFiles = target.Keys.SelectMany(k => target[k]).ToList();

               List<string> existingFiles = new List<string>();

               foreach (ProjectItem parentItem in current.Keys.ToList())
               {
                  foreach (string filename in current[parentItem])
                  {
                     if (!allTargetFiles.Contains(filename) && !fileNames.Contains(filename))
                        dte.Solution.FindProjectItem(filename)?.Delete();
                     else
                        existingFiles.Add(filename);
                  }
               }

               // just to be safe
               existingFiles = existingFiles.Distinct().ToList();

               foreach (ProjectItem parentItem in target.Keys.ToList())
               {
                  foreach (string filename in target[parentItem].Except(existingFiles).ToList())
                     parentItem.ProjectItems.AddFromFile(filename);
               }
            }
         }
      }

      #endregion Template
   }
}
