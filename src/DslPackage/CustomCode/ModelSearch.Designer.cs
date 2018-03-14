namespace Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode
{
   partial class ModelSearch
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

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModelSearch));
         this.label1 = new System.Windows.Forms.Label();
         this.txtSearchText = new System.Windows.Forms.TextBox();
         this.okBtn = new System.Windows.Forms.Button();
         this.cancelBtn = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 8);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(41, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Search";
         // 
         // txtSearchText
         // 
         this.txtSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtSearchText.Location = new System.Drawing.Point(50, 5);
         this.txtSearchText.Name = "txtSearchText";
         this.txtSearchText.Size = new System.Drawing.Size(166, 20);
         this.txtSearchText.TabIndex = 1;
         // 
         // okBtn
         // 
         this.okBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.okBtn.BackColor = System.Drawing.Color.Transparent;
         this.okBtn.Image = ((System.Drawing.Image)(resources.GetObject("okBtn.Image")));
         this.okBtn.Location = new System.Drawing.Point(221, 2);
         this.okBtn.Margin = new System.Windows.Forms.Padding(2);
         this.okBtn.Name = "okBtn";
         this.okBtn.Size = new System.Drawing.Size(26, 26);
         this.okBtn.TabIndex = 2;
         this.okBtn.UseVisualStyleBackColor = false;
         // 
         // cancelBtn
         // 
         this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.cancelBtn.BackColor = System.Drawing.Color.Transparent;
         this.cancelBtn.Image = ((System.Drawing.Image)(resources.GetObject("cancelBtn.Image")));
         this.cancelBtn.Location = new System.Drawing.Point(251, 2);
         this.cancelBtn.Margin = new System.Windows.Forms.Padding(2);
         this.cancelBtn.Name = "cancelBtn";
         this.cancelBtn.Size = new System.Drawing.Size(26, 26);
         this.cancelBtn.TabIndex = 3;
         this.cancelBtn.UseVisualStyleBackColor = false;
         // 
         // ModelSearch
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.Transparent;
         this.Controls.Add(this.cancelBtn);
         this.Controls.Add(this.okBtn);
         this.Controls.Add(this.txtSearchText);
         this.Controls.Add(this.label1);
         this.Name = "ModelSearch";
         this.Size = new System.Drawing.Size(279, 28);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtSearchText;
      private System.Windows.Forms.Button okBtn;
      private System.Windows.Forms.Button cancelBtn;
   }
}
