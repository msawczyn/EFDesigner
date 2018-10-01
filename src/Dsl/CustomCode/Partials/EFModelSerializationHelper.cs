using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace Sawczyn.EFDesigner.EFModel
{
   public partial class EFModelSerializationHelper
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

      /// <summary>
      /// Loads a ModelRoot instance from a stream.
      /// </summary>
      /// <param name="serializationResult">Stores serialization result from the load operation.</param>
      /// <param name="partition">Partition in which the new ModelRoot instance will be created.</param>
      /// <param name="location">Source location associated with stream from which the ModelRoot instance is to be loaded. Usually a file path, but can be any string, including null.</param>
      /// <param name="schemaResolver">
      /// An ISchemaResolver that allows the serializer to do schema validation on the root element (and everything inside it).
      /// If null is passed, schema validation will not be performed.
      /// </param>
      /// <param name="validationController">
      /// A ValidationController that will be used to do load-time validation (validations with validation category "Load"). If null
      /// is passed, load-time validation will not be performed.
      /// </param>
      /// <param name="serializerLocator">
      /// An ISerializerLocator that will be used to locate any additional domain model types required to load the model. Can be null.
      /// </param>
      /// <param name="stream">The Stream from which the ModelRoot will be deserialized.</param>
      /// <returns>The loaded ModelRoot instance.</returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Generated code")]
      public override ModelRoot LoadModel(SerializationResult serializationResult, Partition partition, string location, ISchemaResolver schemaResolver, ValidationController validationController, ISerializerLocator serializerLocator, System.IO.Stream stream)
      {
         #region Check Parameters

         if (serializationResult == null)
            throw new ArgumentNullException(nameof(serializationResult));

         if (partition == null)
            throw new ArgumentNullException(nameof(partition));

         if (stream == null)
            throw new ArgumentNullException(nameof(stream));

         #endregion

         // Prior to v1.2.6.3, the XML format was a bit different.
         // To maintain backward compatability, we're going to check the format and morph it if needed.
         // The verified (or changed) stream is them passed down for further processing in the base class

         Stream workingStream = stream;
         workingStream = ModelMigration.To_1_2_6_11(workingStream);

         if (workingStream == null)
            throw new FileFormatException();

         workingStream.Seek(0, SeekOrigin.Begin);
         return base.LoadModel(serializationResult, partition, location, schemaResolver, validationController, serializerLocator, workingStream);
      }
   }
}
