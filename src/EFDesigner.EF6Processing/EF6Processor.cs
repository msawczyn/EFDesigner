using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace EFDesigner.EF6Processing
{
   [Serializable]
   public class EF6Processor: MarshalByRefObject
   {
      public string ProcessContext(string contextClassName)
      {
         List<Dictionary<string,object>> resultSet = new List<Dictionary<string, object>>();

         DbContext dbContext = Assembly.GetExecutingAssembly()
                                       .CreateInstance(contextClassName,
                                                       false,
                                                       BindingFlags.Default,
                                                       null,
                                                       new object[] {contextClassName},
                                                       null,
                                                       null)
                                  as DbContext;
         ObjectContext objContext = ((IObjectContextAdapter)dbContext).ObjectContext;
         List<EntityType> entityTypes = objContext.MetadataWorkspace.GetItems(DataSpace.CSpace).OfType<EntityType>().ToList();

         foreach (EntityType entityType in entityTypes)
         {
            resultSet.Add(ProcessEntity(entityType));
         }

         return JsonConvert.SerializeObject(resultSet);
      }

      private Dictionary<string, object> ProcessEntity(EntityType entityType)
      {
         Dictionary<string, object> result = new Dictionary<string, object>();

         result.Add("Abstract", entityType.Abstract);
         result.Add("BaseType", entityType.BaseType?.FullName);
         result.Add("Name", entityType.Name);
         result.Add("Namespace", entityType.NamespaceName);

         return result;
      }
   }
}
