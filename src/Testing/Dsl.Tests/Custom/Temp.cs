using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dsl.Tests.Custom
{
   [TestClass]
   public class Temp
   {
      [TestMethod]
      public void DoTest()
      {
         using (MemoryStream stream = new MemoryStream(File.ReadAllBytes(@"C:\Code\ClientProjects\Improving\TuyaTech\CoreServices.v2\TuyaTech.vNext\TuyaTech.vNext.Data\TuyaTechModel.efmodel")))
         {
            XDocument doc = XDocument.Load(stream);
            IEnumerable<XElement> elements = doc.Root.Elements();

            XElement root = doc.Root;
            XElement types = root.Element("types");

            if (types == null)
               return;

            types.Name = "classes";

            XElement enums = doc.Root.Element("enums");
            List<XElement> modelRootHasEnums = enums.Elements("modelRootHasEnums").ToList();
            IEnumerable<XElement> modelEnum = modelRootHasEnums.Elements();
            enums.Add(modelEnum);
            modelRootHasEnums.Remove();

            File.WriteAllBytes(@"c:\temp\TuyaTechModel.efmodel", stream.ToArray());
         }
      }
   }
}
