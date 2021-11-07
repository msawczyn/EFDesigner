
namespace Ex6_Course
{
    partial class FrmCourseManager
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
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtConnection = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUpdateStudent = new System.Windows.Forms.Button();
            this.btnDeleteStudent = new System.Windows.Forms.Button();
            this.btnNewStudent = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLastname = new System.Windows.Forms.TextBox();
            this.txtFirstname = new System.Windows.Forms.TextBox();
            this.lbStudents = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnUpdateCourse = new System.Windows.Forms.Button();
            this.btnDeletCourse = new System.Windows.Forms.Button();
            this.btnNewCourse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCredits = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.LbCourses = new System.Windows.Forms.ListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LblCourse = new System.Windows.Forms.Label();
            this.lblStudent = new System.Windows.Forms.Label();
            this.btnDeleteEnrol = new System.Windows.Forms.Button();
            this.btnNewEnrol = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbEnrolments = new System.Windows.Forms.ListBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(862, 50);
            this.txtDebug.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(418, 743);
            this.txtDebug.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Sql Server Connection";
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(47, 53);
            this.txtConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(734, 26);
            this.txtConnection.TabIndex = 13;
            this.txtConnection.Text = "...";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUpdateStudent);
            this.groupBox1.Controls.Add(this.btnDeleteStudent);
            this.groupBox1.Controls.Add(this.btnNewStudent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtLastname);
            this.groupBox1.Controls.Add(this.txtFirstname);
            this.groupBox1.Controls.Add(this.lbStudents);
            this.groupBox1.Location = new System.Drawing.Point(51, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(730, 193);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Student";
            // 
            // btnUpdateStudent
            // 
            this.btnUpdateStudent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnUpdateStudent.ForeColor = System.Drawing.Color.White;
            this.btnUpdateStudent.Location = new System.Drawing.Point(629, 150);
            this.btnUpdateStudent.Name = "btnUpdateStudent";
            this.btnUpdateStudent.Size = new System.Drawing.Size(75, 29);
            this.btnUpdateStudent.TabIndex = 7;
            this.btnUpdateStudent.Text = "Update";
            this.btnUpdateStudent.UseVisualStyleBackColor = false;
            this.btnUpdateStudent.Click += new System.EventHandler(this.btnUpdateStudent_Click);
            // 
            // btnDeleteStudent
            // 
            this.btnDeleteStudent.BackColor = System.Drawing.Color.Red;
            this.btnDeleteStudent.ForeColor = System.Drawing.Color.White;
            this.btnDeleteStudent.Location = new System.Drawing.Point(548, 150);
            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Size = new System.Drawing.Size(75, 29);
            this.btnDeleteStudent.TabIndex = 6;
            this.btnDeleteStudent.Text = "Del";
            this.btnDeleteStudent.UseVisualStyleBackColor = false;
            this.btnDeleteStudent.Click += new System.EventHandler(this.btnDeleteStudent_Click);
            // 
            // btnNewStudent
            // 
            this.btnNewStudent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNewStudent.ForeColor = System.Drawing.Color.White;
            this.btnNewStudent.Location = new System.Drawing.Point(467, 150);
            this.btnNewStudent.Name = "btnNewStudent";
            this.btnNewStudent.Size = new System.Drawing.Size(75, 29);
            this.btnNewStudent.TabIndex = 5;
            this.btnNewStudent.Text = "New";
            this.btnNewStudent.UseVisualStyleBackColor = false;
            this.btnNewStudent.Click += new System.EventHandler(this.btnNewStudent_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(463, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Lastname";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(463, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Firstname";
            // 
            // txtLastname
            // 
            this.txtLastname.Location = new System.Drawing.Point(463, 111);
            this.txtLastname.Name = "txtLastname";
            this.txtLastname.Size = new System.Drawing.Size(230, 26);
            this.txtLastname.TabIndex = 2;
            this.txtLastname.TextChanged += new System.EventHandler(this.txtLastname_TextChanged);
            // 
            // txtFirstname
            // 
            this.txtFirstname.Location = new System.Drawing.Point(463, 51);
            this.txtFirstname.Name = "txtFirstname";
            this.txtFirstname.Size = new System.Drawing.Size(230, 26);
            this.txtFirstname.TabIndex = 1;
            this.txtFirstname.TextChanged += new System.EventHandler(this.txtFirstname_TextChanged);
            // 
            // lbStudents
            // 
            this.lbStudents.FormattingEnabled = true;
            this.lbStudents.ItemHeight = 20;
            this.lbStudents.Location = new System.Drawing.Point(12, 35);
            this.lbStudents.Name = "lbStudents";
            this.lbStudents.Size = new System.Drawing.Size(423, 144);
            this.lbStudents.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUpdateCourse);
            this.groupBox2.Controls.Add(this.btnDeletCourse);
            this.groupBox2.Controls.Add(this.btnNewCourse);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtCredits);
            this.groupBox2.Controls.Add(this.txtTitle);
            this.groupBox2.Controls.Add(this.LbCourses);
            this.groupBox2.Location = new System.Drawing.Point(51, 299);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(435, 193);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Course";
            // 
            // btnUpdateCourse
            // 
            this.btnUpdateCourse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnUpdateCourse.ForeColor = System.Drawing.Color.White;
            this.btnUpdateCourse.Location = new System.Drawing.Point(345, 154);
            this.btnUpdateCourse.Name = "btnUpdateCourse";
            this.btnUpdateCourse.Size = new System.Drawing.Size(75, 29);
            this.btnUpdateCourse.TabIndex = 7;
            this.btnUpdateCourse.Text = "Update";
            this.btnUpdateCourse.UseVisualStyleBackColor = false;
            // 
            // btnDeletCourse
            // 
            this.btnDeletCourse.BackColor = System.Drawing.Color.Red;
            this.btnDeletCourse.ForeColor = System.Drawing.Color.White;
            this.btnDeletCourse.Location = new System.Drawing.Point(264, 154);
            this.btnDeletCourse.Name = "btnDeletCourse";
            this.btnDeletCourse.Size = new System.Drawing.Size(75, 29);
            this.btnDeletCourse.TabIndex = 6;
            this.btnDeletCourse.Text = "Del";
            this.btnDeletCourse.UseVisualStyleBackColor = false;
            // 
            // btnNewCourse
            // 
            this.btnNewCourse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNewCourse.ForeColor = System.Drawing.Color.White;
            this.btnNewCourse.Location = new System.Drawing.Point(183, 154);
            this.btnNewCourse.Name = "btnNewCourse";
            this.btnNewCourse.Size = new System.Drawing.Size(75, 29);
            this.btnNewCourse.TabIndex = 5;
            this.btnNewCourse.Text = "New";
            this.btnNewCourse.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(179, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Credits";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(179, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Title";
            // 
            // txtCredits
            // 
            this.txtCredits.Location = new System.Drawing.Point(179, 115);
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.Size = new System.Drawing.Size(79, 26);
            this.txtCredits.TabIndex = 2;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(179, 55);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(230, 26);
            this.txtTitle.TabIndex = 1;
            // 
            // LbCourses
            // 
            this.LbCourses.FormattingEnabled = true;
            this.LbCourses.ItemHeight = 20;
            this.LbCourses.Location = new System.Drawing.Point(12, 34);
            this.LbCourses.Name = "LbCourses";
            this.LbCourses.Size = new System.Drawing.Size(151, 144);
            this.LbCourses.TabIndex = 0;
            this.LbCourses.SelectedIndexChanged += new System.EventHandler(this.LbCourses_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button1);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Controls.Add(this.LblCourse);
            this.groupBox3.Controls.Add(this.lblStudent);
            this.groupBox3.Controls.Add(this.btnDeleteEnrol);
            this.groupBox3.Controls.Add(this.btnNewEnrol);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lbEnrolments);
            this.groupBox3.Location = new System.Drawing.Point(51, 512);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(789, 281);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Enrolments - Link STUDENT with COURSE to ENROL and Record Grade";
            // 
            // LblCourse
            // 
            this.LblCourse.BackColor = System.Drawing.Color.White;
            this.LblCourse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblCourse.Location = new System.Drawing.Point(472, 61);
            this.LblCourse.Name = "LblCourse";
            this.LblCourse.Size = new System.Drawing.Size(225, 27);
            this.LblCourse.TabIndex = 8;
            // 
            // lblStudent
            // 
            this.lblStudent.BackColor = System.Drawing.Color.White;
            this.lblStudent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStudent.Location = new System.Drawing.Point(71, 61);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(295, 27);
            this.lblStudent.TabIndex = 7;
            // 
            // btnDeleteEnrol
            // 
            this.btnDeleteEnrol.BackColor = System.Drawing.Color.Red;
            this.btnDeleteEnrol.ForeColor = System.Drawing.Color.White;
            this.btnDeleteEnrol.Location = new System.Drawing.Point(708, 211);
            this.btnDeleteEnrol.Name = "btnDeleteEnrol";
            this.btnDeleteEnrol.Size = new System.Drawing.Size(75, 29);
            this.btnDeleteEnrol.TabIndex = 6;
            this.btnDeleteEnrol.Text = "Delete";
            this.btnDeleteEnrol.UseVisualStyleBackColor = false;
            // 
            // btnNewEnrol
            // 
            this.btnNewEnrol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNewEnrol.ForeColor = System.Drawing.Color.White;
            this.btnNewEnrol.Location = new System.Drawing.Point(382, 61);
            this.btnNewEnrol.Name = "btnNewEnrol";
            this.btnNewEnrol.Size = new System.Drawing.Size(75, 29);
            this.btnNewEnrol.TabIndex = 5;
            this.btnNewEnrol.Text = "Enrol";
            this.btnNewEnrol.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(463, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(223, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Select Course from List Above";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(77, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(193, 20);
            this.label7.TabIndex = 3;
            this.label7.Text = "Select Student List Above";
            // 
            // lbEnrolments
            // 
            this.lbEnrolments.FormattingEnabled = true;
            this.lbEnrolments.ItemHeight = 20;
            this.lbEnrolments.Location = new System.Drawing.Point(71, 96);
            this.lbEnrolments.Name = "lbEnrolments";
            this.lbEnrolments.Size = new System.Drawing.Size(626, 144);
            this.lbEnrolments.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(870, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(134, 20);
            this.label8.TabIndex = 19;
            this.label8.Text = "Debug messages";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(200, 250);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(150, 26);
            this.textBox1.TabIndex = 9;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(131, 253);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 20);
            this.label9.TabIndex = 10;
            this.label9.Text = "Grade";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(382, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 11;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // FrmCourseManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1306, 805);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConnection);
            this.Name = "FrmCourseManager";
            this.Text = "Course Manager";
            this.Load += new System.EventHandler(this.FrmCourseManager_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

      #endregion

      private System.Windows.Forms.TextBox txtDebug;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox txtConnection;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button btnUpdateStudent;
      private System.Windows.Forms.Button btnDeleteStudent;
      private System.Windows.Forms.Button btnNewStudent;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox txtLastname;
      private System.Windows.Forms.TextBox txtFirstname;
      private System.Windows.Forms.ListBox lbStudents;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Button btnUpdateCourse;
      private System.Windows.Forms.Button btnDeletCourse;
      private System.Windows.Forms.Button btnNewCourse;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox txtCredits;
      private System.Windows.Forms.TextBox txtTitle;
      private System.Windows.Forms.ListBox LbCourses;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label LblCourse;
      private System.Windows.Forms.Label lblStudent;
      private System.Windows.Forms.Button btnDeleteEnrol;
      private System.Windows.Forms.Button btnNewEnrol;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.ListBox lbEnrolments;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.TextBox textBox1;
   }
}

