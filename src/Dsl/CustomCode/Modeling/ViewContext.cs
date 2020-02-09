using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

using Microsoft.VisualStudio.Modeling;

namespace Mexedge.VisualStudio.Modeling
{
   public sealed class ViewContext : IXmlSerializable
   {
      public ViewContext(string diagramName)
      {
         if (string.IsNullOrEmpty(diagramName))
            throw new ArgumentNullException(nameof(diagramName));

         DiagramName = diagramName;
      }

      public ViewContext(string diagramName, Type diagramType)
            : this(diagramName)
      {
         if (null == diagramType)
            throw new ArgumentNullException(nameof(diagramType));

         DiagramType = diagramType;
      }

      public ViewContext(string diagramName, Type diagramType, ModelElement rootElement)
            : this(diagramName)
      {
         DiagramType = diagramType;
         RootElement = rootElement;
      }

      private ViewContext() { }

      public string DiagramName { get; private set; }

      public Type DiagramType { get; private set; }

      public ModelElement RootElement { get; }

      public XmlSchema GetSchema()
      {
         throw new NotImplementedException();
      }

      public void ReadXml(XmlReader reader)
      {
         reader.MoveToContent();
         string l_diagramNameValue = reader.GetAttribute("diagramName");
         DiagramName = l_diagramNameValue;
         string l_diagramTypeValue = reader.GetAttribute("diagramType");
         DiagramType = Type.GetType(l_diagramTypeValue);
      }

      public void WriteXml(XmlWriter writer)
      {
         writer.WriteStartElement("ViewContext");
         writer.WriteAttributeString("diagramName", DiagramName);

         if (DiagramType != null)
         {
            writer.WriteAttributeString("diagramType", DiagramType.AssemblyQualifiedName);

            //writer.WriteAttributeString("ui", m_diagramType.IsSubclassOf());
         }

         writer.WriteEndElement();
      }

      public override string ToString()
      {
         using (StringWriter stringWriter = new StringWriter())
         {
            using (XmlWriter xmlWriter = XmlWriter.Create(stringWriter))
            {
               this.WriteXml(xmlWriter);
            }

            return stringWriter.ToString();
         }
      }

      public static bool TryParse(string physicalView, out ViewContext viewContext)
      {
         if (string.IsNullOrEmpty(physicalView))
         {
            viewContext = null;

            return false;
         }

         viewContext = new ViewContext();

         try
         {
            using (StringReader reader = new StringReader(physicalView))
            {
               using (XmlReader xmlReader = XmlReader.Create(reader))
               {
                  viewContext.ReadXml(xmlReader);
               }
            }
         }
         catch (Exception exception)
         {
            Debug.WriteLine(exception);
            viewContext = null;

            return false;
         }

         return true;
      }
   }
}