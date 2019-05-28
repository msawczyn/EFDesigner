namespace Sawczyn.EFDesigner.EFModel
{
   partial class AddCodeForm
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
         this.lblClassName = new System.Windows.Forms.Label();
         this.label2 = new System.Windows.Forms.Label();
         this.btnOk = new System.Windows.Forms.Button();
         this.btnCancel = new System.Windows.Forms.Button();
         this.txtCode = new System.Windows.Forms.TextBox();
         this.statusStrip1 = new System.Windows.Forms.StatusStrip();
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(9, 13);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(63, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Class Name";
         // 
         // lblClassName
         // 
         this.lblClassName.AutoSize = true;
         this.lblClassName.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.lblClassName.Location = new System.Drawing.Point(78, 12);
         this.lblClassName.Name = "lblClassName";
         this.lblClassName.Size = new System.Drawing.Size(70, 15);
         this.lblClassName.TabIndex = 1;
         this.lblClassName.Text = "classname";
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(9, 38);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(54, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "Properties";
         // 
         // btnOk
         // 
         this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnOk.Location = new System.Drawing.Point(235, 257);
         this.btnOk.Name = "btnOk";
         this.btnOk.Size = new System.Drawing.Size(75, 23);
         this.btnOk.TabIndex = 4;
         this.btnOk.Text = "OK";
         this.btnOk.UseVisualStyleBackColor = true;
         this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
         // 
         // btnCancel
         // 
         this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
         this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
         this.btnCancel.Location = new System.Drawing.Point(316, 257);
         this.btnCancel.MinimumSize = new System.Drawing.Size(75, 23);
         this.btnCancel.Name = "btnCancel";
         this.btnCancel.Size = new System.Drawing.Size(75, 23);
         this.btnCancel.TabIndex = 5;
         this.btnCancel.Text = "Cancel";
         this.btnCancel.UseVisualStyleBackColor = true;
         // 
         // txtCode
         // 
         this.txtCode.AcceptsReturn = true;
         this.txtCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
         this.txtCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
         this.txtCode.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.txtCode.Location = new System.Drawing.Point(12, 54);
         this.txtCode.Multiline = true;
         this.txtCode.Name = "txtCode";
         this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
         this.txtCode.Size = new System.Drawing.Size(379, 197);
         this.txtCode.TabIndex = 8;
         this.txtCode.WordWrap = false;
         // 
         // statusStrip1
         // 
         this.statusStrip1.Location = new System.Drawing.Point(0, 286);
         this.statusStrip1.Name = "statusStrip1";
         this.statusStrip1.Size = new System.Drawing.Size(403, 22);
         this.statusStrip1.TabIndex = 9;
         this.statusStrip1.Text = "statusStrip1";
         // 
         // AddCodeForm
         // 
         this.AcceptButton = this.btnOk;
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.CancelButton = this.btnCancel;
         this.ClientSize = new System.Drawing.Size(403, 308);
         this.ControlBox = false;
         this.Controls.Add(this.statusStrip1);
         this.Controls.Add(this.txtCode);
         this.Controls.Add(this.btnCancel);
         this.Controls.Add(this.btnOk);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.lblClassName);
         this.Controls.Add(this.label1);
         this.MaximizeBox = false;
         this.MinimizeBox = false;
         this.MinimumSize = new System.Drawing.Size(340, 237);
         this.Name = "AddCodeForm";
         this.Text = "Add properties as code";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label lblClassName;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button btnOk;
      private System.Windows.Forms.Button btnCancel;
      private System.Windows.Forms.TextBox txtCode;
      private System.Windows.Forms.StatusStrip statusStrip1;
   }
}