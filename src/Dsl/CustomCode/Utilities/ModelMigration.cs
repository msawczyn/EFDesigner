using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Sawczyn.EFDesigner.EFModel.Annotations;

namespace Sawczyn.EFDesigner.EFModel
{
   internal static class ModelMigration
   {
      public static Stream To_1_2_6_3([NotNull] Stream stream)
      {
         if (stream == null)
            throw new ArgumentNullException(nameof(stream));

         Stream result = stream;

         if (stream.Length > 5)
         {
            // Looking for:
            // modelRoot
            //    types [change to classes]
            //    enums
            //       modelRootHasEnums [remove]
            //          modelEnum [move to enums]

            stream.Seek(0, SeekOrigin.Begin);
            XDocument doc = XDocument.Load(stream);
            List<XElement> elements = doc.Root.Elements().ToList();

            XElement types = elements.FirstOrDefault(e => e.Name.LocalName == "types");

            if (types != null)
            {
               types.Name = "classes";
            }

            XElement enums = elements.FirstOrDefault(e => e.Name.LocalName == "enums");

            if (enums != null)
            {
               List<XElement> modelRootHasEnums = enums.Elements().Where(e => e.Name.LocalName == "modelRootHasEnums").ToList();

               foreach (XElement wrapper in modelRootHasEnums)
               {
                  XElement modelEnum = wrapper.Elements().FirstOrDefault(e => e.Name.LocalName == "modelEnum");
                  modelEnum.Remove();
                  enums.Add(modelEnum);
                  wrapper.Remove();
               }
            }

            if (types != null || enums != null)
            {
               MemoryStream memoryStream = new MemoryStream();
               doc.Save(memoryStream);
               memoryStream.Seek(0, SeekOrigin.Begin);

               result = memoryStream;
            }
         }

         return result;
      }
   }
}
