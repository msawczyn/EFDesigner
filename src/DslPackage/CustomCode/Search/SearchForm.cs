using System;
using System.Windows.Forms;

using Sawczyn.EFDesigner.EFModel.Search;

namespace Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode
{
   public partial class SearchForm : Form
   {
      public SearchForm()
      {
         InitializeComponent();
         ClassPropertyGrid.SelectedObject = ClassSearchCriteria;
         PropertyPropertyGrid.SelectedObject = AttributeSearchCriteria;
         EnumPropertyGrid.SelectedObject = EnumSearchCriteria;
         AssociationsPropertyGrid.SelectedObject = AssociationSearchCriteria;
      }

      public static ClassSearchCriteria ClassSearchCriteria { get; private set; } = new ClassSearchCriteria();
      public static AttributeSearchCriteria AttributeSearchCriteria { get; private set; } = new AttributeSearchCriteria();
      public static EnumSearchCriteria EnumSearchCriteria { get; private set; } = new EnumSearchCriteria();
      public static AssociationSearchCriteria AssociationSearchCriteria { get; private set; } = new AssociationSearchCriteria();

      private void ClearOptionsButton_Click(object sender, EventArgs e)
      {
         ClassSearchCriteria = new ClassSearchCriteria();
         AttributeSearchCriteria = new AttributeSearchCriteria();
         EnumSearchCriteria = new EnumSearchCriteria();
         AssociationSearchCriteria = new AssociationSearchCriteria();

         ClassPropertyGrid.SelectedObject = ClassSearchCriteria;
         PropertyPropertyGrid.SelectedObject = AttributeSearchCriteria;
         EnumPropertyGrid.SelectedObject = EnumSearchCriteria;
         AssociationsPropertyGrid.SelectedObject = AssociationSearchCriteria;
      }

      private void CollapseAssociationOptions_Click(object sender, EventArgs e)
      {
         TogglePanel(AssociationsPropertyGrid, 0, ExpandAssociationsOptions, CollapseAssociationsOptions);
      }

      private void CollapseClassOptions_Click(object sender, EventArgs e)
      {
         TogglePanel(ClassPropertyGrid, 0, ExpandClassOptions, CollapseClassOptions);
      }

      private void CollapseEnumOptions_Click(object sender, EventArgs e)
      {
         TogglePanel(EnumPropertyGrid, 0, ExpandEnumOptions, CollapseEnumOptions);
      }

      private void CollapsePropertyOptions_Click(object sender, EventArgs e)
      {
         TogglePanel(PropertyPropertyGrid, 0, ExpandClassOptions, CollapseClassOptions);
      }

      private void ExpandAssociationOptions_Click(object sender, EventArgs e)
      {
         TogglePanel(AssociationsPropertyGrid, 100, CollapseAssociationsOptions, ExpandAssociationsOptions);
      }

      private void ExpandClassOptions_Click(object sender, EventArgs e)
      {
         TogglePanel(ClassPropertyGrid, 100, CollapseClassOptions, ExpandClassOptions);
      }

      private void ExpandEnumOptions_Click(object sender, EventArgs e)
      {
         TogglePanel(EnumPropertyGrid, 100, CollapseEnumOptions, ExpandEnumOptions);
      }

      private void ExpandPropertyOptions_Click(object sender, UICuesEventArgs e)
      {
         TogglePanel(PropertyPropertyGrid, 100, CollapsePropertyOptions, ExpandPropertyOptions);
      }

      private void TogglePanel(PropertyGrid grid, int height, PictureBox visible, PictureBox hidden)
      {
         grid.Height = height;
         hidden.Visible = false;
         visible.Visible = true;
      }

      private void SearchButton_Click(object sender, EventArgs e)
      {

      }

      private void SearchPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
      {
         EffectiveCriteriaLabel.Text += "foo ";
      }

      private void SearchForm_FormClosing(object sender, FormClosingEventArgs e)
      {
         Hide();
         e.Cancel = true;
      }
   }
}