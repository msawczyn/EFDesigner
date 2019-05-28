namespace Sawczyn.EFDesigner.EFModel
{
   partial class ChooseForm
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
         this.btnOK = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.lblTitle = new System.Windows.Forms.Label();
         this.lbChoices = new System.Windows.Forms.ListBox();
         this.SuspendLayout();
         // 
         // btnOK
         // 
         this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnOK.Enabled = false;
         this.btnOK.Location = new System.Drawing.Point(132, 220);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(75, 23);
         this.btnOK.TabIndex = 0;
         this.btnOK.Text = "OK";
         this.btnOK.UseVisualStyleBackColor = true;
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(213, 220);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 23);
         this.btnCancel.TabIndex = 1;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         // 
         // lblTitle
         // 
         this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.lblTitle.Location = new System.Drawing.Point(12, 9);
         this.lblTitle.Name = "lblTitle";
         this.lblTitle.Size = new System.Drawing.Size(276, 23);
         this.lblTitle.TabIndex = 2;
         // 
         // lbChoices
         // 
         this.lbChoices.FormattingEnabled = true;
         this.lbChoices.Location = new System.Drawing.Point(15, 35);
         this.lbChoices.Name = "lbChoices";
         this.lbChoices.Size = new System.Drawing.Size(273, 173);
         this.lbChoices.TabIndex = 3;
         this.lbChoices.SelectedIndexChanged += new System.EventHandler(this.LbChoices_SelectedIndexChanged);
         // 
         // ChooseForm
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(300, 255);
         this.ControlBox = false;
         this.Controls.Add(this.lbChoices);
         this.Controls.Add(this.lblTitle);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOK);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "ChooseForm";
         this.ShowInTaskbar = false;
         this.Text = "Select...";
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button btnOK;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.Label lblTitle;
      private System.Windows.Forms.ListBox lbChoices;
   }
}