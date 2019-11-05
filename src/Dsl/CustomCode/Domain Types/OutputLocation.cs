using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace Sawczyn.EFDesigner.EFModel
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OutputLocation
    {
       private readonly ModelRoot modelRoot;

       public OutputLocation(ModelRoot modelRoot)
       {
          this.modelRoot = modelRoot;
       }

       [System.ComponentModel.TypeConverter(typeof(ProjectDirectoryTypeConverter))]
       [DisplayNameResource("Sawczyn.EFDesigner.EFModel.ModelRoot/ContextOutputDirectory.DisplayName", typeof(global::Sawczyn.EFDesigner.EFModel.EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
       [CategoryResource("Sawczyn.EFDesigner.EFModel.ModelRoot/ContextOutputDirectory.Category", typeof(global::Sawczyn.EFDesigner.EFModel.EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
       [DescriptionResource("Sawczyn.EFDesigner.EFModel.ModelRoot/ContextOutputDirectory.Description", typeof(global::Sawczyn.EFDesigner.EFModel.EFModelDomainModel), "Sawczyn.EFDesigner.EFModel.GeneratedCode.DomainModelResx")]
       public string DbContext
       {
          get
          {
             return modelRoot.ContextOutputDirectory;
          }
          set
          {
             using (Transaction t = modelRoot.Store.TransactionManager.BeginTransaction())
             {
                modelRoot.ContextOutputDirectory = value;
                t.Commit();

             }
          }
       }

       [System.ComponentModel.TypeConverter(typeof(ProjectDirectoryTypeConverter))]
       public string Entity { get; set; }
       [System.ComponentModel.TypeConverter(typeof(ProjectDirectoryTypeConverter))]
       public string Enum { get; set; }
       [System.ComponentModel.TypeConverter(typeof(ProjectDirectoryTypeConverter))]
       public string Struct { get; set; }

       //public OutputLocationDetail Entity { get; set; }
       //public OutputLocationDetail Enum { get; set; }
       //public OutputLocationDetail Struct { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OutputLocationDetail
    {
       public string Project { get; set; }
       public string Folder { get; set; }
    }
}
