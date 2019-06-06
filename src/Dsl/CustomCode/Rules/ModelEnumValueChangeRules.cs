using System.CodeDom.Compiler;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.VisualStudio.Modeling;

using Sawczyn.EFDesigner.EFModel.Extensions;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelEnumValue), FireTime = TimeToFire.LocalCommit)]
   public class ModelEnumValueChangeRules : ChangeRule
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelEnumValue element = (ModelEnumValue)e.ModelElement;
         if (element.IsDeleted)
            return;

         ModelEnum modelEnum = element.Enum;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
            return;

         if (Equals(e.NewValue, e.OldValue))
            return;

         string errorMessage = null;

         switch (e.DomainProperty.Name)
         {
            case "Name":
               string newName = (string)e.NewValue;
               Match match = Regex.Match(newName, @"(.+)\s*=\s*(\d+)");

               if (match != Match.Empty)
               {
                  newName = match.Groups[1].Value;
                  element.Value = match.Groups[2].Value;
               }

               if (string.IsNullOrWhiteSpace(newName) || !CodeGenerator.IsValidLanguageIndependentIdentifier(newName))
                  errorMessage = $"{modelEnum.Name}.{newName}: Name must be a valid .NET identifier";
               else if (modelEnum.Values.Except(new[] {element}).Any(v => v.Name == newName))
                  errorMessage = $"{modelEnum.Name}.{newName}: Name already in use";
               else if (!string.IsNullOrWhiteSpace((string)e.OldValue))
               {
                  // find ModelAttributes where the default value is this ModelEnumValue and change it to the new name
                  string oldInitialValue = $"{modelEnum.Name}.{e.OldValue}";
                  string newInitialValue = $"{modelEnum.Name}.{e.NewValue}";

                  foreach (ModelAttribute modelAttribute in store.Get<ModelAttribute>().Where(a => a.InitialValue == oldInitialValue))
                     modelAttribute.InitialValue = newInitialValue;
               }

               break;

            case "Value":
               string newValue = (string)e.NewValue;

               //if (modelEnum.IsFlags)
               //{
               //   int index = modelEnum.Values.IndexOf(element);
               //   int properValue = (int)Math.Pow(2, index);
               //   if (newValue != properValue.ToString())
               //      current.Rollback();
               //   return;
               //}

               if (newValue != null)
               {
                  bool badValue = false;

                  switch (modelEnum.ValueType)
                  {
                     case EnumValueType.Int16:
                        badValue = !short.TryParse(newValue, out short _);

                        break;
                     case EnumValueType.Int32:
                        badValue = !int.TryParse(newValue, out int _);

                        break;
                     case EnumValueType.Int64:
                        badValue = !long.TryParse(newValue, out long _);

                        break;
                  }

                  if (badValue)
                     errorMessage = $"Invalid value for {modelEnum.Name}. Must be {modelEnum.ValueType}.";
                  else
                  {
                     bool hasDuplicates = modelEnum.Values.Any(x => x != element && x.Value == newValue);

                     if (hasDuplicates)
                        errorMessage = $"Value {newValue} is already present in {modelEnum.Name}. Can't have duplicate values.";
                  }
               }

               break;
         }

         if (errorMessage != null)
         {
            current.Rollback();
            ErrorDisplay.Show(errorMessage);
         }
      }
   }
}
