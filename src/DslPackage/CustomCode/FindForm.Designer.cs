namespace Sawczyn.EFDesigner.EFModel.DslPackage.CustomCode
{
   partial class FindForm
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
         this.label1 = new System.Windows.Forms.Label();
         this.txtFind = new System.Windows.Forms.TextBox();
         this.btnOK = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(12, 9);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(56, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Find name";
         // 
         // txtFind
         // 
         this.txtFind.Location = new System.Drawing.Point(12, 25);
         this.txtFind.Name = "txtFind";
         this.txtFind.Size = new System.Drawing.Size(276, 20);
         this.txtFind.TabIndex = 1;
         // 
         // btnOK
         // 
         this.btnOK.Location = new System.Drawing.Point(132, 55);
         this.btnOK.Name = "btnOK";
         this.btnOK.Size = new System.Drawing.Size(75, 23);
         this.btnOK.TabIndex = 2;
         this.btnOK.Text = "OK";
         this.btnOK.UseVisualStyleBackColor = true;
         this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
         // 
         // btnCancel
         // 
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(213, 55);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 23);
         this.btnCancel.TabIndex = 3;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         // 
         // FindForm
         // 
         this.AcceptButton = this.btnOK;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(300, 89);
         this.ControlBox = false;
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOK);
         this.Controls.Add(this.txtFind);
         this.Controls.Add(this.label1);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.Name = "FindForm";
         this.ShowIcon = false;
         this.ShowInTaskbar = false;
         this.Text = "Find...";
         this.TopMost = true;
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtFind;
      private System.Windows.Forms.Button btnOK;
      private System.Windows.Forms.Button btnCancel;
   }
}