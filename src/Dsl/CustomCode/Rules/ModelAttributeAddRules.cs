using Microsoft.VisualStudio.Modeling;

namespace Sawczyn.EFDesigner.EFModel
{
   [RuleOn(typeof(ModelAttribute), FireTime = TimeToFire.TopLevelCommit)]
   internal class ModelAttributeAddRules : AddRule
   {
      public override void ElementAdded(ElementAddedEventArgs e)
      {
         base.ElementAdded(e);

         ModelAttribute element = (ModelAttribute)e.ModelElement;
         ModelClass modelClass = element.ModelClass;

         // set a new default value if we want to implement notify, to reduce the chance of forgetting to change it
         if (element.ImplementNotify)
            element.AutoProperty = false;
      }
   }
}
