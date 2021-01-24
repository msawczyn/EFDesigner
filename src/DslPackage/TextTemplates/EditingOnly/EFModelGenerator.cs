using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security;

// ReSharper disable RedundantNameQualifier

namespace Sawczyn.EFDesigner.EFModel.EditingOnly
{
   // ReSharper disable once UnusedMember.Global
   public partial class GeneratedTextTransformation
   {
      #region Template
      // EFDesigner v3.0.3
      // Copyright (c) 2017-2021 Michael Sawczyn
      // https://github.com/msawczyn/EFDesigner

      protected void NL()
      {
         WriteLine(string.Empty);
      }

      protected void Output(List<string> segments)
      {
         if (ModelRoot.ChopMethodChains)
            OutputChopped(segments);
         else
            Output(string.Join(".", segments) + ";");

         segments.Clear();
      }

      protected void Output(string text)
      {
         if (text == "}")
            PopIndent();

         WriteLine(text);

         if (text == "{")
            PushIndent(ModelRoot.UseTabs ? "\t" : "   ");
      }

      protected void Output(string template, params object[] items)
      {
         string text = string.Format(template, items);
         Output(text);
      }

      protected void OutputChopped(List<string> segments)
      {
         string[] segmentArray = segments?.ToArray() ?? new string[0];

         if (!segmentArray.Any())
            return;

         int indent = segmentArray[0].IndexOf('.');

         if (indent == -1)
         {
            if (segmentArray.Length > 1)
            {
               segmentArray[0] = $"{segmentArray[0]}.{segmentArray[1]}";
               indent = segmentArray[0].IndexOf('.');
               segmentArray = segmentArray.Where((source, index) => index != 1).ToArray();
            }
         }

         for (int index = 1; index < segmentArray.Length; ++index)
            segmentArray[index] = $"{new string(' ', indent)}.{segmentArray[index]}";

         if (!segmentArray[segmentArray.Length - 1].Trim().EndsWith(";"))
            segmentArray[segmentArray.Length - 1] = segmentArray[segmentArray.Length - 1] + ";";

         foreach (string segment in segmentArray)
            Output(segment);

         segments.Clear();
      }

      public abstract class EFModelGenerator
      {
         protected static string[] xmlDocTags =
         {
            @"<([\s]*c[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*c[\s]*)>"
          , @"<([\s]*code[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*code[\s]*)>"
          , @"<([\s]*description[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*description[\s]*)>"
          , @"<([\s]*example[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*example[\s]*)>"
          , @"<([\s]*exception cref=""[^""]+""[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*exception[\s]*)>"
          , @"<([\s]*include file='[^']+' path='tagpath\[@name=""[^""]+""\]'[\s]*/)>"
          , @"<([\s]*inheritdoc/[\s]*)>"
          , @"<([\s]*item[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*item[\s]*)>"
          , @"<([\s]*list type=""[^""]+""[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*list[\s]*)>"
          , @"<([\s]*listheader[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*listheader[\s]*)>"
          , @"<([\s]*para[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*para[\s]*)>"
          , @"<([\s]*param name=""[^""]+""[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*param[\s]*)>"
          , @"<([\s]*paramref name=""[^""]+""/[\s]*)>"
          , @"<([\s]*permission cref=""[^""]+""[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*permission[\s]*)>"
          , @"<([\s]*remarks[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*remarks[\s]*)>"
          , @"<([\s]*returns[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*returns[\s]*)>"
          , @"<([\s]*see cref=""[^""]+""/[\s]*)>"
          , @"<([\s]*seealso cref=""[^""]+""/[\s]*)>"
          , @"<([\s]*summary[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*summary[\s]*)>"
          , @"<([\s]*term[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*term[\s]*)>"
          , @"<([\s]*typeparam name=""[^""]+""[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*typeparam[\s]*)>"
          , @"<([\s]*typeparamref name=""[^""]+""/[\s]*)>"
          , @"<([\s]*value[\s]*[/]?[\s]*)>"
          , @"<(/[\s]*value[\s]*)>"
         };

         protected EFModelGenerator(GeneratedTextTransformation host)
         {
            this.host = host;
            modelRoot = host.ModelRoot;
         }

#pragma warning disable IDE1006 // Naming Styles
         protected ModelRoot modelRoot { get; set; }
         protected GeneratedTextTransformation host { get; set; }
#pragma warning restore IDE1006 // Naming Styles

         // implementations delegated to the surrounding GeneratedTextTransformation for backward compatability
         protected void NL() { host.NL(); }
         protected void Output(List<string> segments) { host.Output(segments); }
         protected void Output(string text) { host.Output(text); }
         protected void Output(string template, params object[] items) { host.Output(template, items); }
         protected void OutputChopped(List<string> segments) { host.OutputChopped(segments); }
         protected void ClearIndent() { host.ClearIndent(); }

         public static string[] NonNullableTypes
         {
            get
            {
               return new[]
                      {
                         "Binary"
                       , "Geography"
                       , "GeographyCollection"
                       , "GeographyLineString"
                       , "GeographyMultiLineString"
                       , "GeographyMultiPoint"
                       , "GeographyMultiPolygon"
                       , "GeographyPoint"
                       , "GeographyPolygon"
                       , "Geometry"
                       , "GeometryCollection"
                       , "GeometryLineString"
                       , "GeometryMultiLineString"
                       , "GeometryMultiPoint"
                       , "GeometryMultiPolygon"
                       , "GeometryPoint"
                       , "GeometryPolygon"
                       , "String"
                      };
            }
         }

         protected bool AllSuperclassesAreNullOrAbstract(ModelClass modelClass)
         {
            ModelClass superClass = modelClass.Superclass;

            while (superClass != null)
            {
               if (!superClass.IsAbstract)
                  return false;

               superClass = superClass.Superclass;
            }

            return true;
         }

         protected void BeginNamespace(string ns)
         {
            if (!string.IsNullOrEmpty(ns))
            {
               Output($"namespace {ns}");
               Output("{");
            }
         }

         protected static string CreateShadowPropertyName(Association association, List<string> foreignKeyColumns, ModelAttribute identityAttribute)
         {
            string separator = identityAttribute.ModelClass.ModelRoot.ShadowKeyNamePattern == ShadowKeyPattern.TableColumn ? "" : "_";

            string GetShadowPropertyName(string nameBase)
            {
               //if (association.SourceRole == EndpointRole.Dependent)
               //   return $"{nameBase}{identityAttribute.Name}";

               //if (association is BidirectionalAssociation)
               //   return $"{nameBase}{identityAttribute.Name}";

               return $"{nameBase}{separator}{identityAttribute.Name}";
            }

            string GetShadowPropertyNameBase()
            {
               if (association.SourceRole == EndpointRole.Dependent)
                  return association.TargetPropertyName;

               if (association is BidirectionalAssociation b)
                  return b.SourcePropertyName;

               return $"{association.Source.Name}{separator}{association.TargetPropertyName}";
            }

            string shadowNameBase = GetShadowPropertyNameBase();
            string shadowPropertyName = GetShadowPropertyName(shadowNameBase);

            int index = 0;

            while (foreignKeyColumns.Contains(shadowPropertyName))
               shadowPropertyName = GetShadowPropertyName($"{shadowNameBase}{++index}");

            return shadowPropertyName;
         }

         protected void EndNamespace(string ns)
         {
            if (!string.IsNullOrEmpty(ns))
               Output("}");
         }

         protected string FullyQualified(string typeName)
         {
            string[] parts = typeName.Split('.');

            if (parts.Length == 1)
               return typeName;

            string simpleName = parts[0];
            ModelEnum modelEnum = modelRoot.Store.ElementDirectory.AllElements.OfType<ModelEnum>().FirstOrDefault(e => e.Name == simpleName);

            return modelEnum != null
                      ? $"{modelEnum.FullName}.{parts.Last()}"
                      : typeName;
         }

         public abstract void Generate(Manager efModelFileManager);

         protected void GeneratePropertyAnnotations(ModelAttribute modelAttribute)
         {
            if (modelAttribute.Persistent)
            {
               if (modelAttribute.IsIdentity)
                  Output("[Key]");

               if (modelAttribute.IsConcurrencyToken)
                  Output("[ConcurrencyCheck]");
            }
            else
               Output("[NotMapped]");

            if (modelAttribute.Required)
               Output("[Required]");

            if (modelAttribute.FQPrimitiveType == "string")
            {
               if (modelAttribute.MinLength > 0)
                  Output($"[MinLength({modelAttribute.MinLength})]");

               if (modelAttribute.MaxLength > 0)
               {
                  Output($"[MaxLength({modelAttribute.MaxLength})]");
                  Output($"[StringLength({modelAttribute.MaxLength})]");
               }
            }

            if (!string.IsNullOrWhiteSpace(modelAttribute.DisplayText))
               Output($"[Display(Name=\"{modelAttribute.DisplayText.Replace("\"", "\\\"")}\")]");

            if (!string.IsNullOrWhiteSpace(modelAttribute.Summary))
               Output($"[System.ComponentModel.Description(\"{modelAttribute.Summary.Replace("\"", "\\\"")}\")]");
         }

         protected abstract List<string> GetAdditionalUsingStatements();

         protected string GetDefaultConstructorVisibility(ModelClass modelClass)
         {
            if (modelClass.DefaultConstructorVisibility == TypeAccessModifierExt.Default)
            {
               bool hasRequiredParameters = GetRequiredParameters(modelClass, false, true).Any();

               string visibility = (hasRequiredParameters || modelClass.IsAbstract) && !modelClass.IsDependentType
                                      ? "protected"
                                      : "public";

               return visibility;
            }

            return modelClass.DefaultConstructorVisibility.ToString().ToLowerInvariant();
         }

         protected string GetFullContainerName(string containerType, string payloadType)
         {
            string result;

            switch (containerType)
            {
               case "HashSet":
                  result = "System.Collections.Generic.HashSet<T>";

                  break;

               case "LinkedList":
                  result = "System.Collections.Generic.LinkedList<T>";

                  break;

               case "List":
                  result = "System.Collections.Generic.List<T>";

                  break;

               case "SortedSet":
                  result = "System.Collections.Generic.SortedSet<T>";

                  break;

               case "Collection":
                  result = "System.Collections.ObjectModel.Collection<T>";

                  break;

               case "ObservableCollection":
                  result = "System.Collections.ObjectModel.ObservableCollection<T>";

                  break;

               case "BindingList":
                  result = "System.ComponentModel.BindingList<T>";

                  break;

               default:
                  result = containerType;

                  break;
            }

            if (result.EndsWith("<T>"))
               result = result.Replace("<T>", $"<{payloadType}>");

            return result;
         }

         protected string GetMigrationNamespace()
         {
            List<string> nsParts = modelRoot.Namespace.Split('.').ToList();
            nsParts = nsParts.Take(nsParts.Count - 1).ToList();
            nsParts.Add("Migrations");

            return string.Join(".", nsParts);
         }         protected List<string> GetRequiredParameterNames(ModelClass modelClass, bool publicOnly = false)
         {
            return GetRequiredParameters(modelClass, null, publicOnly).Select(p => p.Split(' ')[1]).ToList();
         }

         /// <summary>Gets the local required properties for the ModelClass in formal parameter format</summary>
         /// <param name="modelClass">Source</param>
         /// <param name="haveDefaults">If true, only return those with default values. If false, only return those without default values. If null, return both.</param>
         /// <param name="publicOnly">If true, only return those with public setters. If false, only return those without public setters. If null, return both.</param>
         protected List<string> GetRequiredParameters(ModelClass modelClass, bool? haveDefaults, bool publicOnly = false)
         {
            List<string> requiredParameters = new List<string>();

            if (haveDefaults != true)
            {
               // false or null - get those without default values 
               requiredParameters.AddRange(modelClass.AllRequiredAttributes
                                                     .Where(x => (!x.IsIdentity || x.IdentityType == IdentityType.Manual)
                                                              && !x.IsConcurrencyToken
                                                              && (x.SetterVisibility == SetterAccessModifier.Public || !publicOnly)
                                                              && string.IsNullOrEmpty(x.InitialValue))
                                                     .Select(x => $"{x.FQPrimitiveType} {x.Name.ToLower()}"));

               // don't use 1..1 associations in constructor parameters. Becomes a Catch-22 scenario.
               requiredParameters.AddRange(modelClass.AllRequiredNavigationProperties()
                                                     .Where(np => np.AssociationObject.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One
                                                               || np.AssociationObject.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                                                     .Select(x => $"{x.ClassType.FullName} {x.PropertyName.ToLower()}"));
            }

            if (haveDefaults != false)
            {
               // true or null - get those with default values
               requiredParameters.AddRange(modelClass.AllRequiredAttributes
                                                     .Where(x => (!x.IsIdentity || x.IdentityType == IdentityType.Manual)
                                                              && !x.IsConcurrencyToken
                                                              && (x.SetterVisibility == SetterAccessModifier.Public || !publicOnly)
                                                              && !string.IsNullOrEmpty(x.InitialValue))
                                                     .Select(x =>
                                                             {
                                                                string quote = x.PrimitiveType == "string"
                                                                                  ? "\""
                                                                                  : x.PrimitiveType == "char"
                                                                                     ? "'"
                                                                                     : string.Empty;

                                                                string value = FullyQualified(quote.Length > 0
                                                                                                 ? x.InitialValue.Trim(quote[0])
                                                                                                 : x.InitialValue);

                                                                return $"{x.FQPrimitiveType} {x.Name.ToLower()} = {quote}{value}{quote}";
                                                             }));
            }

            return requiredParameters;
         }

         public static bool IsNullable(ModelAttribute modelAttribute)
         {
            return !modelAttribute.Required && !modelAttribute.IsIdentity && !modelAttribute.IsConcurrencyToken && !NonNullableTypes.Contains(modelAttribute.Type);
         }

         protected virtual void WriteClass(ModelClass modelClass)
         {
            Output("using System;");
            Output("using System.Collections.Generic;");
            Output("using System.Collections.ObjectModel;");
            Output("using System.ComponentModel;");
            Output("using System.ComponentModel.DataAnnotations;");
            Output("using System.ComponentModel.DataAnnotations.Schema;");
            Output("using System.Linq;");
            Output("using System.Runtime.CompilerServices;");

            List<string> additionalUsings = GetAdditionalUsingStatements();

            if (additionalUsings.Any())
               Output(string.Join("\n", additionalUsings));

            NL();

            BeginNamespace(modelClass.EffectiveNamespace);

            string isAbstract = modelClass.IsAbstract
                                   ? "abstract "
                                   : string.Empty;

            List<string> bases = new List<string>();

            if (modelClass.Superclass != null)
               bases.Add(modelClass.Superclass.FullName);

            if (!string.IsNullOrEmpty(modelClass.CustomInterfaces))
               bases.AddRange(modelClass.CustomInterfaces.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries));

            string baseClass = string.Join(", ", bases.Select(x => x.Trim()));

            if (!string.IsNullOrEmpty(modelClass.Summary))
            {
               Output("/// <summary>");
               WriteCommentBody(modelClass.Summary);
               Output("/// </summary>");
            }

            if (!string.IsNullOrEmpty(modelClass.Description))
            {
               Output("/// <remarks>");
               WriteCommentBody(modelClass.Description);
               Output("/// </remarks>");
            }

            if (!string.IsNullOrWhiteSpace(modelClass.CustomAttributes))
               Output($"[{modelClass.CustomAttributes.Trim('[', ']')}]");

            if (!string.IsNullOrWhiteSpace(modelClass.Summary))
               Output($"[System.ComponentModel.Description(\"{modelClass.Summary.Replace("\"", "\\\"")}\")]");

            Output(baseClass.Length > 0
                      ? $"public {isAbstract}partial class {modelClass.Name}: {baseClass}"
                      : $"public {isAbstract}partial class {modelClass.Name}");

            Output("{");

            WriteConstructor(modelClass);
            WriteProperties(modelClass);
            WriteNavigationProperties(modelClass);

            Output("}");

            EndNamespace(modelClass.EffectiveNamespace);
            NL();
         }

         protected string[] GenerateCommentBody(string comment)
         {
            List<string> result = new List<string>();
            int chunkSize = 80;
            string[] parts = comment.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string value in parts)
            {
               string text = value;

               while (text.Length > 0)
               {
                  string outputText = text;

                  if (outputText.Length > chunkSize)
                  {
                     outputText = (text.IndexOf(' ', chunkSize) > 0
                                      ? text.Substring(0, text.IndexOf(' ', chunkSize))
                                      : text).Trim();

                     text = text.Substring(outputText.Length).Trim();
                  }
                  else
                     text = string.Empty;

                  result.Add(SecurityElement.Escape(outputText));
               }
            }

            return result.ToArray();
         }

         protected void WriteCommentBody(string comment)
         {
            foreach (string s in GenerateCommentBody(comment))
                Output($"/// {s}");
         }

         protected void WriteConstructor(ModelClass modelClass)
         {
            Output("partial void Init();");
            NL();

            /***********************************************************************/
            // Default constructor
            /***********************************************************************/

            bool hasRequiredParameters = GetRequiredParameters(modelClass, false, true).Any();

            bool hasOneToOneAssociations = modelClass.AllRequiredNavigationProperties()
                                                     .Any(np => np.AssociationObject.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One
                                                             && np.AssociationObject.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One);

            string visibility = GetDefaultConstructorVisibility(modelClass);

            if (visibility == "public")
            {
               Output("/// <summary>");
               Output("/// Default constructor");
               Output("/// </summary>");
            }
            else if (modelClass.IsAbstract)
            {
               Output("/// <summary>");
               Output("/// Default constructor. Protected due to being abstract.");
               Output("/// </summary>");
            }
            else if (hasRequiredParameters)
            {
               Output("/// <summary>");
               Output("/// Default constructor. Protected due to required properties, but present because EF needs it.");
               Output("/// </summary>");
            }

            List<string> remarks = new List<string>();

            if (hasOneToOneAssociations)
            {
               List<Association> oneToOneAssociations = modelClass.AllRequiredNavigationProperties()
                                                                  .Where(np => np.AssociationObject.SourceMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One
                                                                            && np.AssociationObject.TargetMultiplicity == Sawczyn.EFDesigner.EFModel.Multiplicity.One)
                                                                  .Select(np => np.AssociationObject)
                                                                  .ToList();

               List<ModelClass> otherEndsOneToOne = oneToOneAssociations.Where(a => a.Source != modelClass).Select(a => a.Target)
                                                                        .Union(oneToOneAssociations.Where(a => a.Target != modelClass).Select(a => a.Source))
                                                                        .ToList();

               if (oneToOneAssociations.Any(a => a.Source.Name == modelClass.Name && a.Target.Name == modelClass.Name))
                  otherEndsOneToOne.Add(modelClass);

               if (otherEndsOneToOne.Any())
               {
                  string nameList = otherEndsOneToOne.Count == 1
                                       ? otherEndsOneToOne.First().Name
                                       : string.Join(", ", otherEndsOneToOne.Take(otherEndsOneToOne.Count - 1).Select(c => c.Name))
                                       + " and "
                                       + (otherEndsOneToOne.Last().Name != modelClass.Name
                                             ? otherEndsOneToOne.Last().Name
                                             : "itself");

                  remarks.Add($"// NOTE: This class has one-to-one associations with {nameList}.");
                  remarks.Add("// One-to-one associations are not validated in constructors since this causes a scenario where each one must be constructed before the other.");
               }
            }

            Output(modelClass.Superclass != null
                      ? $"{visibility} {modelClass.Name}(): base()"
                      : $"{visibility} {modelClass.Name}()");

            Output("{");

            if (remarks.Count > 0)
            {
               foreach (string remark in remarks)
                  Output(remark);

               NL();
            }

            WriteDefaultConstructorBody(modelClass);

            Output("}");
            NL();

            if (visibility != "public" && !modelClass.IsAbstract)
            {
               Output("/// <summary>");
               Output("/// Replaces default constructor, since it's protected. Caller assumes responsibility for setting all required values before saving.");
               Output("/// </summary>");
               Output($"public static {modelClass.Name} Create{modelClass.Name}Unsafe()");
               Output("{");
               Output($"return new {modelClass.Name}();");
               Output("}");
               NL();
            }

            /***********************************************************************/
            // Constructor with required parameters (if necessary)
            /***********************************************************************/

            if (hasRequiredParameters)
            {
               visibility = modelClass.IsAbstract
                               ? "protected"
                               : "public";

               Output("/// <summary>");
               Output("/// Public constructor with required data");
               Output("/// </summary>");

               WriteConstructorComments(modelClass);
               Output($"{visibility} {modelClass.Name}({string.Join(", ", GetRequiredParameters(modelClass, null, true))})");
               Output("{");

               if (remarks.Count > 0)
               {
                  foreach (string remark in remarks)
                     Output(remark);

                  NL();
               }

               foreach (ModelAttribute requiredAttribute in modelClass.AllRequiredAttributes
                                                                      .Where(x => (!x.IsIdentity || x.IdentityType == IdentityType.Manual)
                                                                               && !x.IsConcurrencyToken
                                                                               && x.SetterVisibility == SetterAccessModifier.Public))
               {
                  if (requiredAttribute.Type == "String")
                     Output($"if (string.IsNullOrEmpty({requiredAttribute.Name.ToLower()})) throw new ArgumentNullException(nameof({requiredAttribute.Name.ToLower()}));");
                  else if (requiredAttribute.Type.StartsWith("Geo"))
                     Output($"if ({requiredAttribute.Name.ToLower()} == null) throw new ArgumentNullException(nameof({requiredAttribute.Name.ToLower()}));");

                  Output($"this.{requiredAttribute.Name} = {requiredAttribute.Name.ToLower()};");
                  NL();
               }

               foreach (ModelAttribute modelAttribute in modelClass.Attributes.Where(x => x.SetterVisibility == SetterAccessModifier.Public
                                                                                       && !x.Required
                                                                                       && !string.IsNullOrEmpty(x.InitialValue)
                                                                                       && x.InitialValue != "null"))
               {
                  string quote = modelAttribute.Type == "String"
                                    ? "\""
                                    : modelAttribute.Type == "Char"
                                       ? "'"
                                       : string.Empty;

                  Output(quote.Length > 0
                            ? $"this.{modelAttribute.Name} = {quote}{FullyQualified(modelAttribute.InitialValue.Trim(quote[0]))}{quote};"
                            : $"this.{modelAttribute.Name} = {quote}{FullyQualified(modelAttribute.InitialValue)}{quote};");
               }

               // all required navigation properties that aren't a 1..1 relationship
               foreach (NavigationProperty requiredNavigationProperty in modelClass.AllRequiredNavigationProperties()
                                                                                   .Where(np => np.AssociationObject.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One
                                                                                             || np.AssociationObject.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One))
               {
                  NavigationProperty otherSide = requiredNavigationProperty.OtherSide;
                  string parameterName = requiredNavigationProperty.PropertyName.ToLower();
                  Output($"if ({parameterName} == null) throw new ArgumentNullException(nameof({parameterName}));");

                  Output(requiredNavigationProperty.IsCollection
                            ? $"{requiredNavigationProperty.PropertyName}.Add({parameterName});"
                            : $"this.{requiredNavigationProperty.PropertyName} = {parameterName};");

                  if (!string.IsNullOrEmpty(otherSide.PropertyName))
                  {
                     Output(otherSide.IsCollection 
                               ? $"{parameterName}.{otherSide.PropertyName}.Add(this);" 
                               : $"{parameterName}.{otherSide.PropertyName} = this;");
                  }

                  NL();
               }

               foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties()
                                                                           .Where(x => x.AssociationObject.Persistent
                                                                                    && (x.IsCollection || x.ClassType.IsDependentType)
                                                                                    && !x.ConstructorParameterOnly))
               {
                  if (!navigationProperty.IsCollection && navigationProperty.ClassType.IsDependentType)
                     Output($"this.{navigationProperty.PropertyName} = new {navigationProperty.ClassType.FullName}();");
                  else if (navigationProperty.IsCollection)
                  {
                     string collectionType = GetFullContainerName(navigationProperty.AssociationObject.CollectionClass, navigationProperty.ClassType.FullName);
                     Output($"this.{navigationProperty.PropertyName} = new {collectionType}();");
                  }
               }

               Output("Init();");
               Output("}");
               NL();

               if (!modelClass.IsAbstract)
               {
                  Output("/// <summary>");
                  Output("/// Static create function (for use in LINQ queries, etc.)");
                  Output("/// </summary>");
                  WriteConstructorComments(modelClass);

                  string newToken = string.Empty;
                  List<string> requiredParameters = GetRequiredParameters(modelClass, null, true);

                  if (!AllSuperclassesAreNullOrAbstract(modelClass))
                  {
                     List<string> superclassRequiredParameters = GetRequiredParameters(modelClass.Superclass, null, true);

                     if (!requiredParameters.Except(superclassRequiredParameters).Any())
                        newToken = "new ";
                  }

                  List<string> requiredParameterNames = GetRequiredParameterNames(modelClass, true);
                  Output($"public static {newToken}{modelClass.Name} Create({string.Join(", ", requiredParameters)})");
                  Output("{");
                  Output($"return new {modelClass.Name}({string.Join(", ", requiredParameterNames)});");
                  Output("}");
                  NL();
               }
            }
         }

         protected void WriteConstructorComments(ModelClass modelClass)
         {
            foreach (ModelAttribute requiredAttribute in modelClass.AllRequiredAttributes.Where(x => (!x.IsIdentity || x.IdentityType == IdentityType.Manual)
                                                                                                  && !x.IsConcurrencyToken
                                                                                                  && x.SetterVisibility == SetterAccessModifier.Public))
               Output($@"/// <param name=""{requiredAttribute.Name.ToLower()}"">{string.Join(" ", GenerateCommentBody(requiredAttribute.Summary))}</param>");

            // TODO: Add comment if available
            foreach (NavigationProperty requiredNavigationProperty in modelClass.AllRequiredNavigationProperties()
                                                                                .Where(np => np.AssociationObject.SourceMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One
                                                                                          || np.AssociationObject.TargetMultiplicity != Sawczyn.EFDesigner.EFModel.Multiplicity.One))
               Output($@"/// <param name=""{requiredNavigationProperty.PropertyName.ToLower()}""></param>");
         }

         protected void WriteDbContextComments()
         {
            if (!string.IsNullOrEmpty(modelRoot.Summary))
            {
               Output("/// <summary>");
               WriteCommentBody(modelRoot.Summary);
               Output("/// </summary>");

               if (!string.IsNullOrEmpty(modelRoot.Description))
               {
                  Output("/// <remarks>");
                  WriteCommentBody(modelRoot.Description);
                  Output("/// </remarks>");
               }
            }
            else
               Output("/// <inheritdoc/>");
         }

         protected void WriteDefaultConstructorBody(ModelClass modelClass)
         {
            int lineCount = 0;

            foreach (ModelAttribute modelAttribute in modelClass.Attributes.Where(x => x.SetterVisibility == SetterAccessModifier.Public
                                                                                    && !string.IsNullOrEmpty(x.InitialValue)
                                                                                    && x.InitialValue.Trim('"') != "null"))
            {
               string quote = modelAttribute.Type == "String"
                                 ? "\""
                                 : modelAttribute.Type == "Char"
                                    ? "'"
                                    : string.Empty;

               Output(quote.Length == 1
                         ? $"{modelAttribute.Name} = {quote}{FullyQualified(modelAttribute.InitialValue.Trim(quote[0]))}{quote};"
                         : $"{modelAttribute.Name} = {quote}{FullyQualified(modelAttribute.InitialValue)}{quote};");

               ++lineCount;
            }

            foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties()
                                                                        .Where(x => x.AssociationObject.Persistent && x.IsCollection && !x.ConstructorParameterOnly))
            {
               string collectionType = GetFullContainerName(navigationProperty.AssociationObject.CollectionClass, navigationProperty.ClassType.FullName);
               Output($"{navigationProperty.PropertyName} = new {collectionType}();");
               ++lineCount;
            }

            WriteRequiredNavigationsInConstructorBody(modelClass, ref lineCount);

            if (lineCount > 0)
               NL();

            Output("Init();");
         }

         protected virtual void WriteRequiredNavigationsInConstructorBody(ModelClass modelClass, ref int lineCount)
         {
            foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties()
                                                                        .Where(x => x.AssociationObject.Persistent
                                                                                 && x.Required
                                                                                 && !x.IsCollection
                                                                                 && !x.ConstructorParameterOnly))
            {
               if (navigationProperty.ClassType.IsAbstract)
                  Output($"// Assignment of required association {navigationProperty.ClassType.Name} {navigationProperty.PropertyName} was not generated because {navigationProperty.ClassType.Name} is abstract. This must be assigned with a concrete object before saving.");
               else
               {
                  Output(GetDefaultConstructorVisibility(navigationProperty.ClassType) == "public"
                            ? $"{navigationProperty.PropertyName} = new {navigationProperty.ClassType.FullName}();"
                            : $"{navigationProperty.PropertyName} = {navigationProperty.ClassType.FullName}.Create{navigationProperty.ClassType.Name}Unsafe();");
               }

               ++lineCount;
            }
         }

         protected void WriteEnum(ModelEnum modelEnum)
         {
            Output("using System;");
            NL();

            BeginNamespace(modelEnum.EffectiveNamespace);

            if (!string.IsNullOrEmpty(modelEnum.Summary))
            {
               Output("/// <summary>");
               WriteCommentBody(modelEnum.Summary);
               Output("/// </summary>");
            }

            if (!string.IsNullOrEmpty(modelEnum.Description))
            {
               Output("/// <remarks>");
               WriteCommentBody(modelEnum.Description);
               Output("/// </remarks>");
            }

            if (modelEnum.IsFlags)
               Output("[Flags]");

            if (!string.IsNullOrWhiteSpace(modelEnum.CustomAttributes))
               Output($"[{modelEnum.CustomAttributes.Trim('[', ']')}]");

            if (!string.IsNullOrWhiteSpace(modelEnum.Summary))
               Output($"[System.ComponentModel.Description(\"{modelEnum.Summary.Replace("\"", "\\\"")}\")]");

            Output($"public enum {modelEnum.Name} : {modelEnum.ValueType}");
            Output("{");

            ModelEnumValue[] values = modelEnum.Values.ToArray();

            for (int index = 0; index < values.Length; ++index)
            {
               if (!string.IsNullOrEmpty(values[index].Summary))
               {
                  Output("/// <summary>");
                  WriteCommentBody(values[index].Summary);
                  Output("/// </summary>");
               }

               if (!string.IsNullOrEmpty(values[index].Description))
               {
                  Output("/// <remarks>");
                  WriteCommentBody(values[index].Description);
                  Output("/// </remarks>");
               }

               if (!string.IsNullOrWhiteSpace(values[index].CustomAttributes))
                  Output($"[{values[index].CustomAttributes.Trim('[', ']')}]");

               if (!string.IsNullOrWhiteSpace(values[index].Summary))
                  Output($"[System.ComponentModel.Description(\"{values[index].Summary.Replace("\"", "\\\"")}\")]");

               if (!string.IsNullOrWhiteSpace(values[index].DisplayText))
                  Output($"[System.ComponentModel.DataAnnotations.Display(Name=\"{values[index].DisplayText.Replace("\"", "\\\"")}\")]");

               Output(string.IsNullOrEmpty(values[index].Value)
                         ? $"{values[index].Name}{(index < values.Length - 1 ? "," : string.Empty)}"
                         : $"{values[index].Name} = {values[index].Value}{(index < values.Length - 1 ? "," : string.Empty)}");
            }

            Output("}");

            EndNamespace(modelEnum.EffectiveNamespace);
         }

         [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
         protected void WriteNavigationProperties(ModelClass modelClass)
         {
            if (!modelClass.LocalNavigationProperties().Any(x => x.AssociationObject.Persistent))
               return;

            Output("/*************************************************************************");
            Output(" * Navigation properties");
            Output(" *************************************************************************/");
            NL();

            foreach (NavigationProperty navigationProperty in modelClass.LocalNavigationProperties().Where(x => !x.ConstructorParameterOnly))
            {
               string type = navigationProperty.IsCollection
                                ? $"ICollection<{navigationProperty.ClassType.FullName}>"
                                : navigationProperty.ClassType.FullName;

               if (!navigationProperty.IsAutoProperty)
               {
                  Output("/// <summary>");
                  Output($"/// Backing field for {navigationProperty.PropertyName}");
                  Output("/// </summary>");
                  Output($"protected {type} {navigationProperty.BackingFieldName};");
                  NL();

                  if (!navigationProperty.IsCollection)
                  {
                     Output("/// <summary>");
                     Output($"/// When provided in a partial class, allows value of {navigationProperty.PropertyName} to be changed before setting.");
                     Output("/// </summary>");
                     Output($"partial void Set{navigationProperty.PropertyName}({type} oldValue, ref {type} newValue);");
                     NL();

                     Output("/// <summary>");
                     Output($"/// When provided in a partial class, allows value of {navigationProperty.PropertyName} to be changed before returning.");
                     Output("/// </summary>");
                     Output($"partial void Get{navigationProperty.PropertyName}(ref {type} result);");

                     NL();
                  }
               }

               List<string> comments = new List<string>();

               if (navigationProperty.Required)
                  comments.Add("Required");

               string comment = comments.Count > 0
                                   ? string.Join(", ", comments)
                                   : string.Empty;

               if (!string.IsNullOrEmpty(navigationProperty.Summary) || !string.IsNullOrEmpty(comment))
               {
                  Output("/// <summary>");

                  if (!string.IsNullOrEmpty(comment) && !string.IsNullOrEmpty(navigationProperty.Summary))
                     comment += "<br/>";

                  if (!string.IsNullOrEmpty(comment))
                     WriteCommentBody(comment);

                  if (!string.IsNullOrEmpty(navigationProperty.Summary))
                     WriteCommentBody(navigationProperty.Summary);

                  Output("/// </summary>");
               }

               if (!string.IsNullOrEmpty(navigationProperty.Description))
               {
                  Output("/// <remarks>");
                  WriteCommentBody(navigationProperty.Description);
                  Output("/// </remarks>");
               }

               if (!string.IsNullOrWhiteSpace(navigationProperty.CustomAttributes))
                  Output($"[{navigationProperty.CustomAttributes.Trim('[', ']')}]");

               if (!string.IsNullOrWhiteSpace(navigationProperty.Summary))
                  Output($"[Description(\"{navigationProperty.Summary.Replace("\"", "\\\"")}\")]");

               if (!string.IsNullOrWhiteSpace(navigationProperty.DisplayText))
                  Output($"[Display(Name=\"{navigationProperty.DisplayText.Replace("\"", "\\\"")}\")]");

               if (navigationProperty.IsAutoProperty)
               {
                  if (navigationProperty.IsCollection)
                     Output($"public virtual {type} {navigationProperty.PropertyName} {{ get; private set; }}");
                  else
                     Output($"public virtual {type} {navigationProperty.PropertyName} {{ get; set; }}");
               }
               else if (navigationProperty.IsCollection)
               {
                  Output($"public virtual {type} {navigationProperty.PropertyName}");
                  Output("{");
                  Output("get");
                  Output("{");
                  Output($"return {navigationProperty.BackingFieldName};");
                  Output("}");
                  Output("private set");
                  Output("{");
                  Output($"{navigationProperty.BackingFieldName} = value;");
                  Output("}");
                  Output("}");
               }
               else
               {
                  Output($"public virtual {type} {navigationProperty.PropertyName}");
                  Output("{");
                  Output("get");
                  Output("{");
                  Output($"{type} value = {navigationProperty.BackingFieldName};");
                  Output($"Get{navigationProperty.PropertyName}(ref value);");
                  Output($"return ({navigationProperty.BackingFieldName} = value);");
                  Output("}");
                  Output("set");
                  Output("{");
                  Output($"{type} oldValue = {navigationProperty.BackingFieldName};");
                  Output($"Set{navigationProperty.PropertyName}(oldValue, ref value);");
                  Output("if (oldValue != value)");
                  Output("{");
                  Output($"{navigationProperty.BackingFieldName} = value;");

                  if (navigationProperty.ImplementNotify)
                     Output("OnPropertyChanged();");

                  Output("}");
                  Output("}");
                  Output("}");
               }

               NL();
            }
         }

         protected void WriteProperties(ModelClass modelClass)
         {
            Output("/*************************************************************************");
            Output(" * Properties");
            Output(" *************************************************************************/");
            NL();

            List<string> segments = new List<string>();

            foreach (ModelAttribute modelAttribute in modelClass.Attributes)
            {
               segments.Clear();

               if (modelAttribute.IsIdentity)
                  segments.Add("Identity");

               if (modelAttribute.Indexed)
                  segments.Add("Indexed");

               if (modelAttribute.Required || modelAttribute.IsIdentity)
                  segments.Add("Required");

               if (modelAttribute.MinLength > 0)
                  segments.Add($"Min length = {modelAttribute.MinLength}");

               if (modelAttribute.MaxLength > 0)
                  segments.Add($"Max length = {modelAttribute.MaxLength}");

               if (!string.IsNullOrEmpty(modelAttribute.InitialValue))
               {
                  string quote = modelAttribute.PrimitiveType == "string"
                                    ? "\""
                                    : modelAttribute.PrimitiveType == "char"
                                       ? "'"
                                       : string.Empty;

                  segments.Add($"Default value = {quote}{FullyQualified(modelAttribute.InitialValue.Trim('"'))}{quote}");
               }

               string nullable = IsNullable(modelAttribute)
                                    ? "?"
                                    : string.Empty;

               if (!modelAttribute.IsConcurrencyToken && !modelAttribute.AutoProperty)
               {
                  string visibility = modelAttribute.Indexed ? "internal" : "protected";
                  Output("/// <summary>");
                  Output($"/// Backing field for {modelAttribute.Name}");
                  Output("/// </summary>");
                  Output($"{visibility} {modelAttribute.FQPrimitiveType}{nullable} {modelAttribute.BackingFieldName};");
                  Output("/// <summary>");
                  Output($"/// When provided in a partial class, allows value of {modelAttribute.Name} to be changed before setting.");
                  Output("/// </summary>");
                  Output($"partial void Set{modelAttribute.Name}({modelAttribute.FQPrimitiveType}{nullable} oldValue, ref {modelAttribute.FQPrimitiveType}{nullable} newValue);");
                  Output("/// <summary>");
                  Output($"/// When provided in a partial class, allows value of {modelAttribute.Name} to be changed before returning.");
                  Output("/// </summary>");
                  Output($"partial void Get{modelAttribute.Name}(ref {modelAttribute.FQPrimitiveType}{nullable} result);");

                  NL();
               }

               if (!string.IsNullOrEmpty(modelAttribute.Summary) || segments.Any())
               {
                  Output("/// <summary>");

                  if (segments.Any())
                     WriteCommentBody($"{string.Join(", ", segments)}");

                  if (!string.IsNullOrEmpty(modelAttribute.Summary))
                     WriteCommentBody(modelAttribute.Summary);

                  Output("/// </summary>");
               }

               if (!string.IsNullOrEmpty(modelAttribute.Description))
               {
                  Output("/// <remarks>");
                  WriteCommentBody(modelAttribute.Description);
                  Output("/// </remarks>");
               }

               string setterVisibility = modelAttribute.SetterVisibility == SetterAccessModifier.Protected
                                            ? "protected "
                                            : modelAttribute.SetterVisibility == SetterAccessModifier.Internal
                                               ? "internal "
                                               : string.Empty;

               GeneratePropertyAnnotations(modelAttribute);

               if (!string.IsNullOrWhiteSpace(modelAttribute.CustomAttributes))
                  Output($"[{modelAttribute.CustomAttributes.Trim('[', ']')}]");

               if (modelAttribute.IsAbstract)
                  Output($"public abstract {modelAttribute.FQPrimitiveType}{nullable} {modelAttribute.Name} {{ get; {setterVisibility}set; }}");
               else if (modelAttribute.IsConcurrencyToken || modelAttribute.AutoProperty)
                  Output($"public {modelAttribute.FQPrimitiveType}{nullable} {modelAttribute.Name} {{ get; {setterVisibility}set; }}");
               else
               {
                  Output($"public {modelAttribute.FQPrimitiveType}{nullable} {modelAttribute.Name}");
                  Output("{");
                  Output("get");
                  Output("{");
                  Output($"{modelAttribute.FQPrimitiveType}{nullable} value = {modelAttribute.BackingFieldName};");
                  Output($"Get{modelAttribute.Name}(ref value);");
                  Output($"return ({modelAttribute.BackingFieldName} = value);");
                  Output("}");
                  Output($"{setterVisibility}set");
                  Output("{");
                  Output($"{modelAttribute.FQPrimitiveType}{nullable} oldValue = {modelAttribute.BackingFieldName};");
                  Output($"Set{modelAttribute.Name}(oldValue, ref value);");
                  Output("if (oldValue != value)");
                  Output("{");
                  Output($"{modelAttribute.BackingFieldName} = value;");

                  if (modelAttribute.ImplementNotify)
                     Output("OnPropertyChanged();");

                  Output("}");
                  Output("}");
                  Output("}");
               }

               NL();
            }

            if (!modelClass.AllAttributes.Any(x => x.IsConcurrencyToken)
             && (modelClass.Concurrency == ConcurrencyOverride.Optimistic || modelRoot.ConcurrencyDefault == Concurrency.Optimistic))
            {
               Output("/// <summary>");
               Output("/// Concurrency token");
               Output("/// </summary>");
               Output("[Timestamp]");
               Output("public Byte[] Timestamp { get; set; }");
               NL();
            }
         }
      }
      #endregion Template
   }
}

