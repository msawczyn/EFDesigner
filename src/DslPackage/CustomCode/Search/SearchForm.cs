using System;
using System.Windows.Forms;

using Sawczyn.EFDesigner.EFModel.Search;

namespace Sawczyn.EFDesigner.EFModel
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

      #region Expand/Collapse

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
         SearchOptionPanel.SuspendLayout();
         AssociationSearchOptionsPanel.Height = 22;
         ExpandAssociationsOptions.Visible = true;
         CollapseAssociationsOptions.Visible = false;
         SearchOptionPanel.ResumeLayout(true);
      }

      private void CollapseClassOptions_Click(object sender, EventArgs e)
      {
         SearchOptionPanel.SuspendLayout();
         ClassSearchOptionsPanel.Height = 22;
         ExpandClassOptions.Visible = true;
         CollapseClassOptions.Visible = false;
         SearchOptionPanel.ResumeLayout(true);
      }

      private void CollapseEnumOptions_Click(object sender, EventArgs e)
      {
         SearchOptionPanel.SuspendLayout();
         EnumSearchOptionsPanel.Height = 22;
         ExpandEnumOptions.Visible = true;
         CollapseEnumOptions.Visible = false;
         SearchOptionPanel.ResumeLayout(true);
      }

      private void CollapsePropertyOptions_Click(object sender, EventArgs e)
      {
         SearchOptionPanel.SuspendLayout();
         PropertySearchOptionsPanel.Height = 22;
         ExpandPropertyOptions.Visible = true;
         CollapsePropertyOptions.Visible = false;
         SearchOptionPanel.ResumeLayout(true);
      }

      private void ExpandAssociationOptions_Click(object sender, EventArgs e)
      {
         SearchOptionPanel.SuspendLayout();
         AssociationSearchOptionsPanel.Height = 222;
         ExpandAssociationsOptions.Visible = false;
         CollapseAssociationsOptions.Visible = true;
         SearchOptionPanel.ResumeLayout(true);
      }

      private void ExpandClassOptions_Click(object sender, EventArgs e)
      {
         SearchOptionPanel.SuspendLayout();
         ClassSearchOptionsPanel.Height = 222;
         ExpandClassOptions.Visible = false;
         CollapseClassOptions.Visible = true;
         SearchOptionPanel.ResumeLayout(true);
      }

      private void ExpandEnumOptions_Click(object sender, EventArgs e)
      {
         SearchOptionPanel.SuspendLayout();
         EnumSearchOptionsPanel.Height = 222;
         ExpandEnumOptions.Visible = false;
         CollapseEnumOptions.Visible = true;
         SearchOptionPanel.ResumeLayout(true);
      }

      private void ExpandPropertyOptions_Click(object sender, EventArgs e)
      {
         SearchOptionPanel.SuspendLayout();
         PropertySearchOptionsPanel.Height = 222;
         ExpandPropertyOptions.Visible = false;
         CollapsePropertyOptions.Visible = true;
         SearchOptionPanel.ResumeLayout(true);
      }

      private void ClassSearchLabel_DoubleClick(object sender, EventArgs e)
      {
         if (ExpandClassOptions.Visible)
            ExpandClassOptions_Click(sender, null);
         else
            CollapseClassOptions_Click(sender, null);
      }

      private void PropertySearchLabel_DoubleClick(object sender, EventArgs e)
      {
         if (ExpandPropertyOptions.Visible)
            ExpandPropertyOptions_Click(sender, null);
         else
            CollapsePropertyOptions_Click(sender, null);
      }

      private void AssociationsSearchLabel_DoubleClick(object sender, EventArgs e)
      {
         if (ExpandAssociationsOptions.Visible)
            ExpandAssociationOptions_Click(sender, null);
         else
            CollapseAssociationOptions_Click(sender, null);
      }

      private void EnumSearchLabel_DoubleClick(object sender, EventArgs e)
      {
         if (ExpandEnumOptions.Visible)
            ExpandEnumOptions_Click(sender, null);
         else
            CollapseEnumOptions_Click(sender, null);
      }

      #endregion

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