
namespace Sawczyn.EFDesigner.EFModel
{
   partial class SearchForm
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.MainContainer = new System.Windows.Forms.SplitContainer();
            this.SearchOptionPanel = new System.Windows.Forms.Panel();
            this.EnumSearchOptionsPanel = new System.Windows.Forms.Panel();
            this.EnumSearchOptions = new System.Windows.Forms.Panel();
            this.EnumPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.EnumSearchOptionsHeader = new System.Windows.Forms.Panel();
            this.EnumSearchLabel = new System.Windows.Forms.Label();
            this.CollapseEnumOptions = new System.Windows.Forms.PictureBox();
            this.ExpandEnumOptions = new System.Windows.Forms.PictureBox();
            this.AssociationSearchOptionsPanel = new System.Windows.Forms.Panel();
            this.AssociationSearchOptions = new System.Windows.Forms.Panel();
            this.AssociationsPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.AssociationsSearchOptionsHeader = new System.Windows.Forms.Panel();
            this.AssociationsSearchLabel = new System.Windows.Forms.Label();
            this.CollapseAssociationsOptions = new System.Windows.Forms.PictureBox();
            this.ExpandAssociationsOptions = new System.Windows.Forms.PictureBox();
            this.PropertySearchOptionsPanel = new System.Windows.Forms.Panel();
            this.PropertySearchOptions = new System.Windows.Forms.Panel();
            this.PropertyPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.PropertySearchOptionsHeader = new System.Windows.Forms.Panel();
            this.PropertySearchLabel = new System.Windows.Forms.Label();
            this.CollapsePropertyOptions = new System.Windows.Forms.PictureBox();
            this.ExpandPropertyOptions = new System.Windows.Forms.PictureBox();
            this.ClassSearchOptionsPanel = new System.Windows.Forms.Panel();
            this.ClassSearchOptions = new System.Windows.Forms.Panel();
            this.ClassPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.ClassSearchOptionsHeader = new System.Windows.Forms.Panel();
            this.ClassSearchLabel = new System.Windows.Forms.Label();
            this.CollapseClassOptions = new System.Windows.Forms.PictureBox();
            this.ExpandClassOptions = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ClearOptionsButton = new System.Windows.Forms.Button();
            this.SearchImages = new System.Windows.Forms.ImageList(this.components);
            this.ResultGrid = new System.Windows.Forms.DataGridView();
            this.SelectedOptionsBox = new System.Windows.Forms.GroupBox();
            this.EffectiveCriteriaLabel = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.SearchButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).BeginInit();
            this.MainContainer.Panel1.SuspendLayout();
            this.MainContainer.Panel2.SuspendLayout();
            this.MainContainer.SuspendLayout();
            this.SearchOptionPanel.SuspendLayout();
            this.EnumSearchOptionsPanel.SuspendLayout();
            this.EnumSearchOptions.SuspendLayout();
            this.EnumSearchOptionsHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollapseEnumOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpandEnumOptions)).BeginInit();
            this.AssociationSearchOptionsPanel.SuspendLayout();
            this.AssociationSearchOptions.SuspendLayout();
            this.AssociationsSearchOptionsHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollapseAssociationsOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpandAssociationsOptions)).BeginInit();
            this.PropertySearchOptionsPanel.SuspendLayout();
            this.PropertySearchOptions.SuspendLayout();
            this.PropertySearchOptionsHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollapsePropertyOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpandPropertyOptions)).BeginInit();
            this.ClassSearchOptionsPanel.SuspendLayout();
            this.ClassSearchOptions.SuspendLayout();
            this.ClassSearchOptionsHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CollapseClassOptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpandClassOptions)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).BeginInit();
            this.SelectedOptionsBox.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 0);
            this.MainContainer.Name = "MainContainer";
            // 
            // MainContainer.Panel1
            // 
            this.MainContainer.Panel1.Controls.Add(this.SearchOptionPanel);
            // 
            // MainContainer.Panel2
            // 
            this.MainContainer.Panel2.Controls.Add(this.ResultGrid);
            this.MainContainer.Panel2.Controls.Add(this.SelectedOptionsBox);
            this.MainContainer.Panel2.Controls.Add(this.panel2);
            this.MainContainer.Size = new System.Drawing.Size(800, 511);
            this.MainContainer.SplitterDistance = 266;
            this.MainContainer.TabIndex = 0;
            // 
            // SearchOptionPanel
            // 
            this.SearchOptionPanel.AutoScroll = true;
            this.SearchOptionPanel.Controls.Add(this.EnumSearchOptionsPanel);
            this.SearchOptionPanel.Controls.Add(this.AssociationSearchOptionsPanel);
            this.SearchOptionPanel.Controls.Add(this.PropertySearchOptionsPanel);
            this.SearchOptionPanel.Controls.Add(this.ClassSearchOptionsPanel);
            this.SearchOptionPanel.Controls.Add(this.panel1);
            this.SearchOptionPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchOptionPanel.Location = new System.Drawing.Point(0, 0);
            this.SearchOptionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SearchOptionPanel.Name = "SearchOptionPanel";
            this.SearchOptionPanel.Size = new System.Drawing.Size(266, 511);
            this.SearchOptionPanel.TabIndex = 0;
            // 
            // EnumSearchOptionsPanel
            // 
            this.EnumSearchOptionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EnumSearchOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.EnumSearchOptionsPanel.Controls.Add(this.EnumSearchOptions);
            this.EnumSearchOptionsPanel.Controls.Add(this.EnumSearchOptionsHeader);
            this.EnumSearchOptionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.EnumSearchOptionsPanel.Location = new System.Drawing.Point(0, 92);
            this.EnumSearchOptionsPanel.Name = "EnumSearchOptionsPanel";
            this.EnumSearchOptionsPanel.Size = new System.Drawing.Size(266, 22);
            this.EnumSearchOptionsPanel.TabIndex = 4;
            // 
            // EnumSearchOptions
            // 
            this.EnumSearchOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.EnumSearchOptions.Controls.Add(this.EnumPropertyGrid);
            this.EnumSearchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnumSearchOptions.Location = new System.Drawing.Point(0, 20);
            this.EnumSearchOptions.Name = "EnumSearchOptions";
            this.EnumSearchOptions.Size = new System.Drawing.Size(264, 0);
            this.EnumSearchOptions.TabIndex = 2;
            // 
            // EnumPropertyGrid
            // 
            this.EnumPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnumPropertyGrid.HelpVisible = false;
            this.EnumPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.EnumPropertyGrid.Name = "EnumPropertyGrid";
            this.EnumPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.EnumPropertyGrid.Size = new System.Drawing.Size(264, 0);
            this.EnumPropertyGrid.TabIndex = 0;
            this.EnumPropertyGrid.ToolbarVisible = false;
            this.EnumPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.SearchPropertyValueChanged);
            // 
            // EnumSearchOptionsHeader
            // 
            this.EnumSearchOptionsHeader.Controls.Add(this.EnumSearchLabel);
            this.EnumSearchOptionsHeader.Controls.Add(this.CollapseEnumOptions);
            this.EnumSearchOptionsHeader.Controls.Add(this.ExpandEnumOptions);
            this.EnumSearchOptionsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.EnumSearchOptionsHeader.Location = new System.Drawing.Point(0, 0);
            this.EnumSearchOptionsHeader.Name = "EnumSearchOptionsHeader";
            this.EnumSearchOptionsHeader.Size = new System.Drawing.Size(264, 20);
            this.EnumSearchOptionsHeader.TabIndex = 0;
            // 
            // EnumSearchLabel
            // 
            this.EnumSearchLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.EnumSearchLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnumSearchLabel.Location = new System.Drawing.Point(0, 0);
            this.EnumSearchLabel.Name = "EnumSearchLabel";
            this.EnumSearchLabel.Size = new System.Drawing.Size(224, 20);
            this.EnumSearchLabel.TabIndex = 2;
            this.EnumSearchLabel.Text = "Enums";
            this.EnumSearchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.EnumSearchLabel.DoubleClick += new System.EventHandler(this.EnumSearchLabel_DoubleClick);
            // 
            // CollapseEnumOptions
            // 
            this.CollapseEnumOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CollapseEnumOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CollapseEnumOptions.BackgroundImage")));
            this.CollapseEnumOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CollapseEnumOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CollapseEnumOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.CollapseEnumOptions.Location = new System.Drawing.Point(224, 0);
            this.CollapseEnumOptions.Name = "CollapseEnumOptions";
            this.CollapseEnumOptions.Size = new System.Drawing.Size(20, 20);
            this.CollapseEnumOptions.TabIndex = 1;
            this.CollapseEnumOptions.TabStop = false;
            this.CollapseEnumOptions.Visible = false;
            this.CollapseEnumOptions.Click += new System.EventHandler(this.CollapseEnumOptions_Click);
            // 
            // ExpandEnumOptions
            // 
            this.ExpandEnumOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExpandEnumOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ExpandEnumOptions.BackgroundImage")));
            this.ExpandEnumOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExpandEnumOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExpandEnumOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExpandEnumOptions.Location = new System.Drawing.Point(244, 0);
            this.ExpandEnumOptions.Name = "ExpandEnumOptions";
            this.ExpandEnumOptions.Size = new System.Drawing.Size(20, 20);
            this.ExpandEnumOptions.TabIndex = 0;
            this.ExpandEnumOptions.TabStop = false;
            this.ExpandEnumOptions.Click += new System.EventHandler(this.ExpandEnumOptions_Click);
            // 
            // AssociationSearchOptionsPanel
            // 
            this.AssociationSearchOptionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AssociationSearchOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.AssociationSearchOptionsPanel.Controls.Add(this.AssociationSearchOptions);
            this.AssociationSearchOptionsPanel.Controls.Add(this.AssociationsSearchOptionsHeader);
            this.AssociationSearchOptionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.AssociationSearchOptionsPanel.Location = new System.Drawing.Point(0, 70);
            this.AssociationSearchOptionsPanel.Name = "AssociationSearchOptionsPanel";
            this.AssociationSearchOptionsPanel.Size = new System.Drawing.Size(266, 22);
            this.AssociationSearchOptionsPanel.TabIndex = 3;
            // 
            // AssociationSearchOptions
            // 
            this.AssociationSearchOptions.AutoSize = true;
            this.AssociationSearchOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AssociationSearchOptions.Controls.Add(this.AssociationsPropertyGrid);
            this.AssociationSearchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssociationSearchOptions.Location = new System.Drawing.Point(0, 20);
            this.AssociationSearchOptions.Name = "AssociationSearchOptions";
            this.AssociationSearchOptions.Size = new System.Drawing.Size(264, 0);
            this.AssociationSearchOptions.TabIndex = 2;
            // 
            // AssociationsPropertyGrid
            // 
            this.AssociationsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssociationsPropertyGrid.HelpVisible = false;
            this.AssociationsPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.AssociationsPropertyGrid.Name = "AssociationsPropertyGrid";
            this.AssociationsPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.AssociationsPropertyGrid.Size = new System.Drawing.Size(264, 0);
            this.AssociationsPropertyGrid.TabIndex = 0;
            this.AssociationsPropertyGrid.ToolbarVisible = false;
            this.AssociationsPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.SearchPropertyValueChanged);
            // 
            // AssociationsSearchOptionsHeader
            // 
            this.AssociationsSearchOptionsHeader.Controls.Add(this.AssociationsSearchLabel);
            this.AssociationsSearchOptionsHeader.Controls.Add(this.CollapseAssociationsOptions);
            this.AssociationsSearchOptionsHeader.Controls.Add(this.ExpandAssociationsOptions);
            this.AssociationsSearchOptionsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.AssociationsSearchOptionsHeader.Location = new System.Drawing.Point(0, 0);
            this.AssociationsSearchOptionsHeader.Name = "AssociationsSearchOptionsHeader";
            this.AssociationsSearchOptionsHeader.Size = new System.Drawing.Size(264, 20);
            this.AssociationsSearchOptionsHeader.TabIndex = 0;
            // 
            // AssociationsSearchLabel
            // 
            this.AssociationsSearchLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.AssociationsSearchLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AssociationsSearchLabel.Location = new System.Drawing.Point(0, 0);
            this.AssociationsSearchLabel.Name = "AssociationsSearchLabel";
            this.AssociationsSearchLabel.Size = new System.Drawing.Size(224, 20);
            this.AssociationsSearchLabel.TabIndex = 2;
            this.AssociationsSearchLabel.Text = "Associations";
            this.AssociationsSearchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AssociationsSearchLabel.DoubleClick += new System.EventHandler(this.AssociationsSearchLabel_DoubleClick);
            // 
            // CollapseAssociationsOptions
            // 
            this.CollapseAssociationsOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CollapseAssociationsOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CollapseAssociationsOptions.BackgroundImage")));
            this.CollapseAssociationsOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CollapseAssociationsOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CollapseAssociationsOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.CollapseAssociationsOptions.Location = new System.Drawing.Point(224, 0);
            this.CollapseAssociationsOptions.Name = "CollapseAssociationsOptions";
            this.CollapseAssociationsOptions.Size = new System.Drawing.Size(20, 20);
            this.CollapseAssociationsOptions.TabIndex = 1;
            this.CollapseAssociationsOptions.TabStop = false;
            this.CollapseAssociationsOptions.Visible = false;
            this.CollapseAssociationsOptions.Click += new System.EventHandler(this.CollapseAssociationOptions_Click);
            // 
            // ExpandAssociationsOptions
            // 
            this.ExpandAssociationsOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExpandAssociationsOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ExpandAssociationsOptions.BackgroundImage")));
            this.ExpandAssociationsOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExpandAssociationsOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExpandAssociationsOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExpandAssociationsOptions.Location = new System.Drawing.Point(244, 0);
            this.ExpandAssociationsOptions.Name = "ExpandAssociationsOptions";
            this.ExpandAssociationsOptions.Size = new System.Drawing.Size(20, 20);
            this.ExpandAssociationsOptions.TabIndex = 0;
            this.ExpandAssociationsOptions.TabStop = false;
            this.ExpandAssociationsOptions.Click += new System.EventHandler(this.ExpandAssociationOptions_Click);
            // 
            // PropertySearchOptionsPanel
            // 
            this.PropertySearchOptionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PropertySearchOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PropertySearchOptionsPanel.Controls.Add(this.PropertySearchOptions);
            this.PropertySearchOptionsPanel.Controls.Add(this.PropertySearchOptionsHeader);
            this.PropertySearchOptionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.PropertySearchOptionsPanel.Location = new System.Drawing.Point(0, 48);
            this.PropertySearchOptionsPanel.Name = "PropertySearchOptionsPanel";
            this.PropertySearchOptionsPanel.Size = new System.Drawing.Size(266, 22);
            this.PropertySearchOptionsPanel.TabIndex = 2;
            // 
            // PropertySearchOptions
            // 
            this.PropertySearchOptions.AutoSize = true;
            this.PropertySearchOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PropertySearchOptions.Controls.Add(this.PropertyPropertyGrid);
            this.PropertySearchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertySearchOptions.Location = new System.Drawing.Point(0, 20);
            this.PropertySearchOptions.Name = "PropertySearchOptions";
            this.PropertySearchOptions.Size = new System.Drawing.Size(264, 0);
            this.PropertySearchOptions.TabIndex = 2;
            // 
            // PropertyPropertyGrid
            // 
            this.PropertyPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertyPropertyGrid.HelpVisible = false;
            this.PropertyPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.PropertyPropertyGrid.Name = "PropertyPropertyGrid";
            this.PropertyPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.PropertyPropertyGrid.Size = new System.Drawing.Size(264, 0);
            this.PropertyPropertyGrid.TabIndex = 0;
            this.PropertyPropertyGrid.ToolbarVisible = false;
            this.PropertyPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.SearchPropertyValueChanged);
            // 
            // PropertySearchOptionsHeader
            // 
            this.PropertySearchOptionsHeader.Controls.Add(this.PropertySearchLabel);
            this.PropertySearchOptionsHeader.Controls.Add(this.CollapsePropertyOptions);
            this.PropertySearchOptionsHeader.Controls.Add(this.ExpandPropertyOptions);
            this.PropertySearchOptionsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PropertySearchOptionsHeader.Location = new System.Drawing.Point(0, 0);
            this.PropertySearchOptionsHeader.Name = "PropertySearchOptionsHeader";
            this.PropertySearchOptionsHeader.Size = new System.Drawing.Size(264, 20);
            this.PropertySearchOptionsHeader.TabIndex = 0;
            // 
            // PropertySearchLabel
            // 
            this.PropertySearchLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.PropertySearchLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PropertySearchLabel.Location = new System.Drawing.Point(0, 0);
            this.PropertySearchLabel.Name = "PropertySearchLabel";
            this.PropertySearchLabel.Size = new System.Drawing.Size(224, 20);
            this.PropertySearchLabel.TabIndex = 2;
            this.PropertySearchLabel.Text = "Attributes";
            this.PropertySearchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.PropertySearchLabel.DoubleClick += new System.EventHandler(this.PropertySearchLabel_DoubleClick);
            // 
            // CollapsePropertyOptions
            // 
            this.CollapsePropertyOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CollapsePropertyOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CollapsePropertyOptions.BackgroundImage")));
            this.CollapsePropertyOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CollapsePropertyOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CollapsePropertyOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.CollapsePropertyOptions.Location = new System.Drawing.Point(224, 0);
            this.CollapsePropertyOptions.Name = "CollapsePropertyOptions";
            this.CollapsePropertyOptions.Size = new System.Drawing.Size(20, 20);
            this.CollapsePropertyOptions.TabIndex = 1;
            this.CollapsePropertyOptions.TabStop = false;
            this.CollapsePropertyOptions.Visible = false;
            this.CollapsePropertyOptions.Click += new System.EventHandler(this.CollapsePropertyOptions_Click);
            // 
            // ExpandPropertyOptions
            // 
            this.ExpandPropertyOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExpandPropertyOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ExpandPropertyOptions.BackgroundImage")));
            this.ExpandPropertyOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExpandPropertyOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExpandPropertyOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExpandPropertyOptions.Location = new System.Drawing.Point(244, 0);
            this.ExpandPropertyOptions.Name = "ExpandPropertyOptions";
            this.ExpandPropertyOptions.Size = new System.Drawing.Size(20, 20);
            this.ExpandPropertyOptions.TabIndex = 0;
            this.ExpandPropertyOptions.TabStop = false;
            this.ExpandPropertyOptions.Click += new System.EventHandler(this.ExpandPropertyOptions_Click);
            // 
            // ClassSearchOptionsPanel
            // 
            this.ClassSearchOptionsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClassSearchOptionsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ClassSearchOptionsPanel.Controls.Add(this.ClassSearchOptions);
            this.ClassSearchOptionsPanel.Controls.Add(this.ClassSearchOptionsHeader);
            this.ClassSearchOptionsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ClassSearchOptionsPanel.Location = new System.Drawing.Point(0, 26);
            this.ClassSearchOptionsPanel.Name = "ClassSearchOptionsPanel";
            this.ClassSearchOptionsPanel.Size = new System.Drawing.Size(266, 22);
            this.ClassSearchOptionsPanel.TabIndex = 1;
            // 
            // ClassSearchOptions
            // 
            this.ClassSearchOptions.AutoSize = true;
            this.ClassSearchOptions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClassSearchOptions.Controls.Add(this.ClassPropertyGrid);
            this.ClassSearchOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClassSearchOptions.Location = new System.Drawing.Point(0, 20);
            this.ClassSearchOptions.Name = "ClassSearchOptions";
            this.ClassSearchOptions.Size = new System.Drawing.Size(264, 0);
            this.ClassSearchOptions.TabIndex = 2;
            // 
            // ClassPropertyGrid
            // 
            this.ClassPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClassPropertyGrid.HelpVisible = false;
            this.ClassPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.ClassPropertyGrid.Name = "ClassPropertyGrid";
            this.ClassPropertyGrid.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.ClassPropertyGrid.Size = new System.Drawing.Size(264, 0);
            this.ClassPropertyGrid.TabIndex = 0;
            this.ClassPropertyGrid.ToolbarVisible = false;
            this.ClassPropertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.SearchPropertyValueChanged);
            // 
            // ClassSearchOptionsHeader
            // 
            this.ClassSearchOptionsHeader.Controls.Add(this.ClassSearchLabel);
            this.ClassSearchOptionsHeader.Controls.Add(this.CollapseClassOptions);
            this.ClassSearchOptionsHeader.Controls.Add(this.ExpandClassOptions);
            this.ClassSearchOptionsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.ClassSearchOptionsHeader.Location = new System.Drawing.Point(0, 0);
            this.ClassSearchOptionsHeader.Name = "ClassSearchOptionsHeader";
            this.ClassSearchOptionsHeader.Size = new System.Drawing.Size(264, 20);
            this.ClassSearchOptionsHeader.TabIndex = 0;
            // 
            // ClassSearchLabel
            // 
            this.ClassSearchLabel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClassSearchLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClassSearchLabel.Location = new System.Drawing.Point(0, 0);
            this.ClassSearchLabel.Name = "ClassSearchLabel";
            this.ClassSearchLabel.Size = new System.Drawing.Size(224, 20);
            this.ClassSearchLabel.TabIndex = 2;
            this.ClassSearchLabel.Text = "Classes";
            this.ClassSearchLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ClassSearchLabel.DoubleClick += new System.EventHandler(this.ClassSearchLabel_DoubleClick);
            // 
            // CollapseClassOptions
            // 
            this.CollapseClassOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.CollapseClassOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("CollapseClassOptions.BackgroundImage")));
            this.CollapseClassOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CollapseClassOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CollapseClassOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.CollapseClassOptions.Location = new System.Drawing.Point(224, 0);
            this.CollapseClassOptions.Name = "CollapseClassOptions";
            this.CollapseClassOptions.Size = new System.Drawing.Size(20, 20);
            this.CollapseClassOptions.TabIndex = 1;
            this.CollapseClassOptions.TabStop = false;
            this.CollapseClassOptions.Visible = false;
            this.CollapseClassOptions.Click += new System.EventHandler(this.CollapseClassOptions_Click);
            // 
            // ExpandClassOptions
            // 
            this.ExpandClassOptions.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ExpandClassOptions.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ExpandClassOptions.BackgroundImage")));
            this.ExpandClassOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ExpandClassOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ExpandClassOptions.Dock = System.Windows.Forms.DockStyle.Right;
            this.ExpandClassOptions.Location = new System.Drawing.Point(244, 0);
            this.ExpandClassOptions.Name = "ExpandClassOptions";
            this.ExpandClassOptions.Size = new System.Drawing.Size(20, 20);
            this.ExpandClassOptions.TabIndex = 0;
            this.ExpandClassOptions.TabStop = false;
            this.ExpandClassOptions.Click += new System.EventHandler(this.ExpandClassOptions_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel1.Controls.Add(this.ClearOptionsButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(266, 26);
            this.panel1.TabIndex = 5;
            // 
            // ClearOptionsButton
            // 
            this.ClearOptionsButton.Dock = System.Windows.Forms.DockStyle.Left;
            this.ClearOptionsButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ClearOptionsButton.ImageIndex = 0;
            this.ClearOptionsButton.ImageList = this.SearchImages;
            this.ClearOptionsButton.Location = new System.Drawing.Point(0, 0);
            this.ClearOptionsButton.Name = "ClearOptionsButton";
            this.ClearOptionsButton.Size = new System.Drawing.Size(68, 26);
            this.ClearOptionsButton.TabIndex = 0;
            this.ClearOptionsButton.Text = "Reset";
            this.ClearOptionsButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ClearOptionsButton.UseVisualStyleBackColor = true;
            this.ClearOptionsButton.Click += new System.EventHandler(this.ClearOptionsButton_Click);
            // 
            // SearchImages
            // 
            this.SearchImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("SearchImages.ImageStream")));
            this.SearchImages.TransparentColor = System.Drawing.Color.Transparent;
            this.SearchImages.Images.SetKeyName(0, "Restart_16x.png");
            this.SearchImages.Images.SetKeyName(1, "Search_16x.png");
            // 
            // ResultGrid
            // 
            this.ResultGrid.AllowUserToAddRows = false;
            this.ResultGrid.AllowUserToDeleteRows = false;
            this.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultGrid.Location = new System.Drawing.Point(0, 98);
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.ReadOnly = true;
            this.ResultGrid.Size = new System.Drawing.Size(530, 413);
            this.ResultGrid.TabIndex = 1;
            // 
            // SelectedOptionsBox
            // 
            this.SelectedOptionsBox.Controls.Add(this.EffectiveCriteriaLabel);
            this.SelectedOptionsBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.SelectedOptionsBox.Location = new System.Drawing.Point(0, 26);
            this.SelectedOptionsBox.Name = "SelectedOptionsBox";
            this.SelectedOptionsBox.Size = new System.Drawing.Size(530, 72);
            this.SelectedOptionsBox.TabIndex = 0;
            this.SelectedOptionsBox.TabStop = false;
            this.SelectedOptionsBox.Text = "Find";
            // 
            // EffectiveCriteriaLabel
            // 
            this.EffectiveCriteriaLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EffectiveCriteriaLabel.Location = new System.Drawing.Point(3, 16);
            this.EffectiveCriteriaLabel.Name = "EffectiveCriteriaLabel";
            this.EffectiveCriteriaLabel.Size = new System.Drawing.Size(524, 53);
            this.EffectiveCriteriaLabel.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel2.Controls.Add(this.SearchButton);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(530, 26);
            this.panel2.TabIndex = 6;
            // 
            // SearchButton
            // 
            this.SearchButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.SearchButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SearchButton.ImageIndex = 1;
            this.SearchButton.ImageList = this.SearchImages;
            this.SearchButton.Location = new System.Drawing.Point(462, 0);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(68, 26);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "Search";
            this.SearchButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchForm
            // 
            this.AcceptButton = this.SearchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(800, 511);
            this.Controls.Add(this.MainContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SearchForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Search Model";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchForm_FormClosing);
            this.MainContainer.Panel1.ResumeLayout(false);
            this.MainContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainContainer)).EndInit();
            this.MainContainer.ResumeLayout(false);
            this.SearchOptionPanel.ResumeLayout(false);
            this.EnumSearchOptionsPanel.ResumeLayout(false);
            this.EnumSearchOptions.ResumeLayout(false);
            this.EnumSearchOptionsHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CollapseEnumOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpandEnumOptions)).EndInit();
            this.AssociationSearchOptionsPanel.ResumeLayout(false);
            this.AssociationSearchOptionsPanel.PerformLayout();
            this.AssociationSearchOptions.ResumeLayout(false);
            this.AssociationsSearchOptionsHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CollapseAssociationsOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpandAssociationsOptions)).EndInit();
            this.PropertySearchOptionsPanel.ResumeLayout(false);
            this.PropertySearchOptionsPanel.PerformLayout();
            this.PropertySearchOptions.ResumeLayout(false);
            this.PropertySearchOptionsHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CollapsePropertyOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpandPropertyOptions)).EndInit();
            this.ClassSearchOptionsPanel.ResumeLayout(false);
            this.ClassSearchOptionsPanel.PerformLayout();
            this.ClassSearchOptions.ResumeLayout(false);
            this.ClassSearchOptionsHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CollapseClassOptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ExpandClassOptions)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).EndInit();
            this.SelectedOptionsBox.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.SplitContainer MainContainer;
      private System.Windows.Forms.Panel SearchOptionPanel;
      private System.Windows.Forms.GroupBox SelectedOptionsBox;
      private System.Windows.Forms.Panel ClassSearchOptionsHeader;
      private System.Windows.Forms.PictureBox CollapseClassOptions;
      private System.Windows.Forms.PictureBox ExpandClassOptions;
      private System.Windows.Forms.Label ClassSearchLabel;
      private System.Windows.Forms.Panel ClassSearchOptionsPanel;
      private System.Windows.Forms.Panel ClassSearchOptions;
      private System.Windows.Forms.PropertyGrid ClassPropertyGrid;
      private System.Windows.Forms.Panel PropertySearchOptionsPanel;
      private System.Windows.Forms.Panel PropertySearchOptions;
      private System.Windows.Forms.PropertyGrid PropertyPropertyGrid;
      private System.Windows.Forms.Panel PropertySearchOptionsHeader;
      private System.Windows.Forms.Label PropertySearchLabel;
      private System.Windows.Forms.PictureBox CollapsePropertyOptions;
      private System.Windows.Forms.PictureBox ExpandPropertyOptions;
      private System.Windows.Forms.DataGridView ResultGrid;
      private System.Windows.Forms.Panel EnumSearchOptionsPanel;
      private System.Windows.Forms.Panel EnumSearchOptions;
      private System.Windows.Forms.PropertyGrid EnumPropertyGrid;
      private System.Windows.Forms.Panel EnumSearchOptionsHeader;
      private System.Windows.Forms.Label EnumSearchLabel;
      private System.Windows.Forms.PictureBox CollapseEnumOptions;
      private System.Windows.Forms.PictureBox ExpandEnumOptions;
      private System.Windows.Forms.Panel AssociationSearchOptionsPanel;
      private System.Windows.Forms.Panel AssociationSearchOptions;
      private System.Windows.Forms.PropertyGrid AssociationsPropertyGrid;
      private System.Windows.Forms.Panel AssociationsSearchOptionsHeader;
      private System.Windows.Forms.Label AssociationsSearchLabel;
      private System.Windows.Forms.PictureBox CollapseAssociationsOptions;
      private System.Windows.Forms.PictureBox ExpandAssociationsOptions;
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.Button ClearOptionsButton;
      private System.Windows.Forms.Label EffectiveCriteriaLabel;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Button SearchButton;
      private System.Windows.Forms.ImageList SearchImages;
   }
}