using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel.CustomCode.Rules
{
   [RuleOn(typeof(ModelEnumValue), FireTime = TimeToFire.TopLevelCommit)]
   public class ModelEnumValueChangeRules : ChangeRule
   {
      public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
      {
         base.ElementPropertyChanged(e);

         ModelEnumValue element = (ModelEnumValue)e.ModelElement;
         ModelEnum modelEnum = element.Enum;

         Store store = element.Store;
         Transaction current = store.TransactionManager.CurrentTransaction;

         if (current.IsSerializing)
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
                  errorMessage = "Name must be a valid .NET identifier";
               else if (modelEnum.Values.Except(new[] { element }).Any(v => v.Name == newName))
                  errorMessage = "Value name already in use";

               break;

            case "Value":
               string newValue = (string)e.NewValue;
               if (newValue != null)
               {
                  bool badValue = false;
                  switch (modelEnum.ValueType)
                  {
                     case EnumValueType.Int16:
                        badValue = !Int16.TryParse(newValue, out Int16 result16);
                        break;
                     case EnumValueType.Int32:
                        badValue = !Int32.TryParse(newValue, out Int32 result32);
                        break;
                     case EnumValueType.Int64:
                        badValue = !Int64.TryParse(newValue, out Int64 result64);
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
            MessageBox.Show(errorMessage);
         }
      }
   }
}
