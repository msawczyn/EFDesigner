
namespace Ex1_Person
{
    partial class frmPerson
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
            this.btnTestPerson = new System.Windows.Forms.Button();
            this.txtConnection = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnTestPerson
            // 
            this.btnTestPerson.Location = new System.Drawing.Point(36, 117);
            this.btnTestPerson.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTestPerson.Name = "btnTestPerson";
            this.btnTestPerson.Size = new System.Drawing.Size(170, 63);
            this.btnTestPerson.TabIndex = 0;
            this.btnTestPerson.Text = "Test Person";
            this.btnTestPerson.UseVisualStyleBackColor = true;
            this.btnTestPerson.Click += new System.EventHandler(this.btnTestPerson_Click);
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(36, 55);
            this.txtConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(734, 26);
            this.txtConnection.TabIndex = 1;
            this.txtConnection.Text = "...";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Sql Server Connection";
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(350, 117);
            this.txtDebug.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(421, 266);
            this.txtDebug.TabIndex = 3;
            // 
            // frmPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 398);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConnection);
            this.Controls.Add(this.btnTestPerson);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmPerson";
            this.Text = "Ex 1: Simple Table - Entity Framework Examples: Using Visual Designer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      #endregion

      private System.Windows.Forms.Button btnTestPerson;
      private System.Windows.Forms.TextBox txtConnection;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtDebug;
   }
}

