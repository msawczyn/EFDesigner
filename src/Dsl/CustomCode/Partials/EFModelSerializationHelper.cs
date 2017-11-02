using System;
using System.Diagnostics;
using System.Xml;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   partial class EFModelSerializationHelper
   {
      /// <summary>
      ///    Checks the version of the file being read.
      /// </summary>
      /// <param name="serializationContext">Serialization context.</param>
      /// <param name="reader">
      ///    Reader for the file being read. The reader is positioned at the open tag of the root element being
      ///    read.
      /// </param>
      protected override void CheckVersion(SerializationContext serializationContext, XmlReader reader)
      {
         #region Check Parameters

         Debug.Assert(serializationContext != null);
         if (serializationContext == null)
            throw new ArgumentNullException(nameof(serializationContext));

         Debug.Assert(reader != null);
         if (reader == null)
            throw new ArgumentNullException(nameof(reader));

         #endregion

         string dslVersionStr = reader.GetAttribute("dslVersion");
         if (dslVersionStr != null)
         {
            try
            {
               Version actualVersion = new Version(dslVersionStr);
               if (actualVersion.Major != 1)
                  EFModelSerializationBehaviorSerializationMessages.VersionMismatch(serializationContext, reader, new Version(1, 0), actualVersion);
            }
            catch (ArgumentException)
            {
               EFModelSerializationBehaviorSerializationMessages.InvalidPropertyValue(serializationContext, reader, "dslVersion", typeof(Version), dslVersionStr);
            }
            catch (FormatException)
            {
               EFModelSerializationBehaviorSerializationMessages.InvalidPropertyValue(serializationContext, reader, "dslVersion", typeof(Version), dslVersionStr);
            }
            catch (OverflowException)
            {
               EFModelSerializationBehaviorSerializationMessages.InvalidPropertyValue(serializationContext, reader, "dslVersion", typeof(Version), dslVersionStr);
            }
         }
      }
   }
}
