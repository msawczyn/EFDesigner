using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sawczyn.EFDesigner.EFModel
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OutputLocation
    {
       public OutputLocationDetail Entity { get; set; }
       public OutputLocationDetail Enum { get; set; }
       public OutputLocationDetail Struct { get; set; }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class OutputLocationDetail
    {
       public string Project { get; set; }
       public string Folder { get; set; }
    }
}
