using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Rules
{
   [RuleOn(typeof(ModelAttribute), FireTime = TimeToFire.TopLevelCommit)]
   public class ModelAttributeChangeRules : ChangeRule
   {
      public static readonly string[] ValidIdentityAttributeTypes =
      {
         "Int16",
         "Int32",
         "Int64",
         "Byte",
         "Guid"
      };

      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelAttribute element = (ModelAttribute)e.ModelElement;
         ModelClass modelClass = element.ModelClass;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         List<string> errorMessages = EFCoreValidator.GetErrors(element).ToList();

         switch (e.DomainProperty.Name)
         {
            case "Autoproperty":

               if (element.AutoProperty && modelClass.ImplementNotify)
                  WarningDisplay.Show($"{element.Name} is an autoproperty, so will not participate in INotifyPropertyChanged messages");

               break;

            case "Indexed":
               if (element.IsIdentity)
                  element.Indexed = true;

               if (element.IsConcurrencyToken)
                  element.Indexed = false;

               if (element.Indexed)
                  element.Persistent = true;

               break;

            case "Type":
               string newType = (string)e.NewValue;
               if (element.IsIdentity)
               {
                  if (!ValidIdentityAttributeTypes.Contains(ModelAttribute.ToCLRType(newType)))
                  {
                     errorMessages.Add($"Properties of type {newType} can't be used as identity properties.");
                  }
                  else
                  {
                     element.Required = true;
                     element.Persistent = true;
                  }
               }

               if (newType != "String")
               {
                  element.MaxLength = 0;
                  element.StringType = HTML5Type.None;
               }
               else
               {
                  if (!element.IsValidInitialValue(newType))
                     element.InitialValue = null;
               }

               break;

            case "MinLength":
               int newMinLength = (int)e.NewValue;
               if (element.Type != "String")
                  element.MinLength = 0;

               if (newMinLength < 0)
                  errorMessages.Add("MinLength must be zero or a positive number");

               break;

            case "MaxLength":
               int newMaxLength = (int)e.NewValue;
               if (element.Type != "String")
                  element.MaxLength = 0;

               if (newMaxLength < 0)
                  errorMessages.Add("MaxLength must be zero or a positive number");

               break;

            case "IdentityType":
               if (element.IsIdentity)
               {
                  if (element.IdentityType == IdentityType.None)
                     errorMessages.Add("Identity properties must have an identity type defined");
                  else
                     element.AutoProperty = true;
               }
               else if (!element.IsIdentity)
                  element.IdentityType = IdentityType.None;

               break;

            case "ReadOnly":
               if (!element.Persistent || element.SetterVisibility != SetterAccessModifier.Public)
                  element.ReadOnly = false;
               break;

            case "IsIdentity":
               bool newIsIdentity = (bool)e.NewValue;

               if (newIsIdentity)
               {
                  if (element.ModelClass.IsDependentType)
                  {
                     errorMessages.Add($"Can't make {element.Name} an identity because {element.ModelClass.Name} is a dependent type and can't have an identity property.");
                  }
                  else
                  {
                     if (!ValidIdentityAttributeTypes.Contains(element.Type))
                     {
                        errorMessages.Add($"Properties of type {element.Type} can't be used as identity properties.");
                     }
                     else
                     {
                        element.IsConcurrencyToken = false;
                        element.Indexed            = true;
                        element.IndexedUnique      = true;
                        element.Persistent         = true;
                        element.Required           = true;

                        if (element.IdentityType == IdentityType.None)
                           element.IdentityType = IdentityType.AutoGenerated;
                     }
                  }
               }
               else
                  element.IdentityType = IdentityType.None;

               break;

            case "IsConcurrencyToken":
               bool newIsConcurrencyToken = (bool)e.NewValue;
               if (newIsConcurrencyToken)
               {
                  element.IsIdentity = false;
                  element.Persistent = true;
                  element.Required = true;
               }

               break;

            case "Required":
               bool newRequired = (bool)e.NewValue;
               if (!newRequired)
                  if (element.IsIdentity || element.IsConcurrencyToken)
                     element.Required = true;

               break;

            case "Persistent":
               bool newPersistent = (bool)e.NewValue;
               if (!newPersistent)
               {
                  element.IsIdentity = false;
                  element.Indexed = false;
                  element.IndexedUnique = false;
                  element.IdentityType = IdentityType.None;
                  element.IsConcurrencyToken = false;
                  element.Virtual = false;
               }

               break;

            case "Name":
               string newName = (string)e.NewValue;

               if (string.IsNullOrEmpty(newName))
                  errorMessages.Add("Name must be a valid .NET identifier");
               else
               {
                  ParseResult fragment;
                  try
                  {
                     fragment = ModelAttribute.Parse(element.ModelClass.ModelRoot, newName);

                     if (fragment == null)
                        errorMessages.Add($"Could not parse entry '{newName}'");
                     else
                     {
                        if (string.IsNullOrEmpty(fragment.Name) || !CodeGenerator.IsValidLanguageIndependentIdentifier(fragment.Name))
                           errorMessages.Add("Name must be a valid .NET identifier");
                        else if (modelClass.AllAttributes.Except(new[] {element}).Any(x => x.Name == fragment.Name))
                           errorMessages.Add("Property name already in use");
                        else if (modelClass.AllNavigationProperties().Any(p => p.PropertyName == fragment.Name))
                           errorMessages.Add("Property name already in use");
                        else
                        {
                           element.Name = fragment.Name;
                           if (fragment.Type != null) element.Type = fragment.Type;
                           if (fragment.Required != null) element.Required = fragment.Required.Value;
                           if (fragment.MaxLength != null) element.MaxLength = fragment.MaxLength.Value;
                           if (fragment.InitialValue != null) element.InitialValue = fragment.InitialValue;
                           if (fragment.IsIdentity) element.IsIdentity = true; // don't reset to false if not entered as part of name
                        }
                     }
                  }
                  catch (Exception exception)
                  {
                     errorMessages.Add($"Could not parse entry '{newName}': {exception.Message}");
                  }
               }

               break;

            case "InitialValue":
               string newInitialValue = (string)e.NewValue;
               if (!element.IsValidInitialValue(null, newInitialValue))
                  errorMessages.Add($"{newInitialValue} isn't a valid value for {element.Type}");

               break;
         }

         errorMessages = errorMessages.Where(m => m != null).ToList();
         if (errorMessages.Any())
         {
            current.Rollback();
            ErrorDisplay.Show(string.Join("; ", errorMessages));
         }
      }
   }
}
