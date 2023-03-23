namespace Ex1_ModelPerson
{
    partial class FrmPerson
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnection = new System.Windows.Forms.TextBox();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnTestPerson
            // 
            this.btnTestPerson.Location = new System.Drawing.Point(92, 86);
            this.btnTestPerson.Name = "btnTestPerson";
            this.btnTestPerson.Size = new System.Drawing.Size(75, 23);
            this.btnTestPerson.TabIndex = 0;
            this.btnTestPerson.Text = "Test Person";
            this.btnTestPerson.UseVisualStyleBackColor = true;
            this.btnTestPerson.Click += new System.EventHandler(this.btnTestPerson_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection String";
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(7, 31);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(241, 23);
            this.txtConnection.TabIndex = 2;
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(266, 31);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(242, 167);
            this.txtDebug.TabIndex = 3;
            // 
            // FrmPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 210);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.txtConnection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTestPerson);
            this.Name = "FrmPerson";
            this.Text = "FrmPerson";
            this.Load += new System.EventHandler(this.FrmPerson_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnTestPerson;
        private Label label1;
        private TextBox txtConnection;
        private TextBox txtDebug;
    }
}