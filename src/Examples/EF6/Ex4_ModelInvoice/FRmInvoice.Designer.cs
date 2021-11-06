
namespace Ex4_ModelInvoice
{
    partial class FRmInvoice
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
            this.btnInvoice = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInvoice
            // 
            this.btnInvoice.Location = new System.Drawing.Point(60, 169);
            this.btnInvoice.Name = "btnInvoice";
            this.btnInvoice.Size = new System.Drawing.Size(192, 53);
            this.btnInvoice.TabIndex = 0;
            this.btnInvoice.Text = "Invoice";
            this.btnInvoice.UseVisualStyleBackColor = true;
            this.btnInvoice.Click += new System.EventHandler(this.button1_Click);
            // 
            // FRmInvoice
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnInvoice);
            this.Name = "FRmInvoice";
            this.Text = "Accounting Mock up";
            this.ResumeLayout(false);

        }

      #endregion

      private System.Windows.Forms.Button btnInvoice;
   }
}

