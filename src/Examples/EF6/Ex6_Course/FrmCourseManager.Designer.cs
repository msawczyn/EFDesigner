
namespace Ex5_Course
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
            this.lvStudents = new System.Windows.Forms.ListView();
            this.Id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.First = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Last = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnUpdateStudent = new System.Windows.Forms.Button();
            this.btnDeleteStudent = new System.Windows.Forms.Button();
            this.btnNewStudent = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLastname = new System.Windows.Forms.TextBox();
            this.txtFirstname = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvCourses = new System.Windows.Forms.ListView();
            this.ColId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label10 = new System.Windows.Forms.Label();
            this.txtCourseID = new System.Windows.Forms.TextBox();
            this.btnUpdateCourse = new System.Windows.Forms.Button();
            this.btnDeletCourse = new System.Windows.Forms.Button();
            this.btnNewCourse = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCredits = new System.Windows.Forms.TextBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lvEnrolments = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnEnrolmentUpdate = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGrade = new System.Windows.Forms.TextBox();
            this.LblCourse = new System.Windows.Forms.Label();
            this.lblStudent = new System.Windows.Forms.Label();
            this.btnDeleteEnrol = new System.Windows.Forms.Button();
            this.btnNewEnrol = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSeedData = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(862, 49);
            this.txtDebug.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(418, 742);
            this.txtDebug.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Sql Server Connection";
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(46, 52);
            this.txtConnection.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(734, 26);
            this.txtConnection.TabIndex = 13;
            this.txtConnection.Text = "...";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvStudents);
            this.groupBox1.Controls.Add(this.btnUpdateStudent);
            this.groupBox1.Controls.Add(this.btnDeleteStudent);
            this.groupBox1.Controls.Add(this.btnNewStudent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtLastname);
            this.groupBox1.Controls.Add(this.txtFirstname);
            this.groupBox1.Location = new System.Drawing.Point(51, 88);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(730, 192);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Student";
            // 
            // lvStudents
            // 
            this.lvStudents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Id,
            this.First,
            this.Last});
            this.lvStudents.FullRowSelect = true;
            this.lvStudents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvStudents.HideSelection = false;
            this.lvStudents.Location = new System.Drawing.Point(12, 22);
            this.lvStudents.Name = "lvStudents";
            this.lvStudents.Size = new System.Drawing.Size(445, 156);
            this.lvStudents.TabIndex = 21;
            this.lvStudents.UseCompatibleStateImageBehavior = false;
            this.lvStudents.View = System.Windows.Forms.View.Details;
            this.lvStudents.SelectedIndexChanged += new System.EventHandler(this.lvStudents_SelectedIndexChanged);
            // 
            // Id
            // 
            this.Id.Text = "Id";
            this.Id.Width = 30;
            // 
            // First
            // 
            this.First.Text = "First";
            this.First.Width = 150;
            // 
            // Last
            // 
            this.Last.Text = "Last";
            this.Last.Width = 200;
            // 
            // btnUpdateStudent
            // 
            this.btnUpdateStudent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnUpdateStudent.ForeColor = System.Drawing.Color.White;
            this.btnUpdateStudent.Location = new System.Drawing.Point(628, 149);
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
            this.btnDeleteStudent.Location = new System.Drawing.Point(548, 149);
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
            this.btnNewStudent.Location = new System.Drawing.Point(466, 149);
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
            this.label3.Location = new System.Drawing.Point(464, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Lastname";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(464, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Firstname";
            // 
            // txtLastname
            // 
            this.txtLastname.Location = new System.Drawing.Point(464, 111);
            this.txtLastname.Name = "txtLastname";
            this.txtLastname.Size = new System.Drawing.Size(230, 26);
            this.txtLastname.TabIndex = 2;
            this.txtLastname.TextChanged += new System.EventHandler(this.txtLastname_TextChanged);
            // 
            // txtFirstname
            // 
            this.txtFirstname.Location = new System.Drawing.Point(464, 51);
            this.txtFirstname.Name = "txtFirstname";
            this.txtFirstname.Size = new System.Drawing.Size(230, 26);
            this.txtFirstname.TabIndex = 1;
            this.txtFirstname.TextChanged += new System.EventHandler(this.txtFirstname_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lvCourses);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtCourseID);
            this.groupBox2.Controls.Add(this.btnUpdateCourse);
            this.groupBox2.Controls.Add(this.btnDeletCourse);
            this.groupBox2.Controls.Add(this.btnNewCourse);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtCredits);
            this.groupBox2.Controls.Add(this.txtTitle);
            this.groupBox2.Location = new System.Drawing.Point(51, 298);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(730, 192);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Course";
            // 
            // lvCourses
            // 
            this.lvCourses.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColId,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvCourses.FullRowSelect = true;
            this.lvCourses.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvCourses.HideSelection = false;
            this.lvCourses.Location = new System.Drawing.Point(12, 25);
            this.lvCourses.Name = "lvCourses";
            this.lvCourses.Size = new System.Drawing.Size(445, 153);
            this.lvCourses.TabIndex = 21;
            this.lvCourses.UseCompatibleStateImageBehavior = false;
            this.lvCourses.View = System.Windows.Forms.View.Details;
            this.lvCourses.SelectedIndexChanged += new System.EventHandler(this.lvCourses_SelectedIndexChanged);
            // 
            // ColId
            // 
            this.ColId.Text = "Id";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Course";
            this.columnHeader1.Width = 45;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Desc";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Credits";
            this.columnHeader3.Width = 80;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(608, 92);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 20);
            this.label10.TabIndex = 9;
            this.label10.Text = "Course ID";
            // 
            // txtCourseID
            // 
            this.txtCourseID.Location = new System.Drawing.Point(608, 115);
            this.txtCourseID.Name = "txtCourseID";
            this.txtCourseID.Size = new System.Drawing.Size(79, 26);
            this.txtCourseID.TabIndex = 8;
            // 
            // btnUpdateCourse
            // 
            this.btnUpdateCourse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnUpdateCourse.ForeColor = System.Drawing.Color.White;
            this.btnUpdateCourse.Location = new System.Drawing.Point(628, 149);
            this.btnUpdateCourse.Name = "btnUpdateCourse";
            this.btnUpdateCourse.Size = new System.Drawing.Size(75, 29);
            this.btnUpdateCourse.TabIndex = 7;
            this.btnUpdateCourse.Text = "Update";
            this.btnUpdateCourse.UseVisualStyleBackColor = false;
            this.btnUpdateCourse.Click += new System.EventHandler(this.btnUpdateCourse_Click);
            // 
            // btnDeletCourse
            // 
            this.btnDeletCourse.BackColor = System.Drawing.Color.Red;
            this.btnDeletCourse.ForeColor = System.Drawing.Color.White;
            this.btnDeletCourse.Location = new System.Drawing.Point(548, 149);
            this.btnDeletCourse.Name = "btnDeletCourse";
            this.btnDeletCourse.Size = new System.Drawing.Size(75, 29);
            this.btnDeletCourse.TabIndex = 6;
            this.btnDeletCourse.Text = "Del";
            this.btnDeletCourse.UseVisualStyleBackColor = false;
            this.btnDeletCourse.Click += new System.EventHandler(this.btnDeletCourse_Click);
            // 
            // btnNewCourse
            // 
            this.btnNewCourse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNewCourse.ForeColor = System.Drawing.Color.White;
            this.btnNewCourse.Location = new System.Drawing.Point(466, 149);
            this.btnNewCourse.Name = "btnNewCourse";
            this.btnNewCourse.Size = new System.Drawing.Size(75, 29);
            this.btnNewCourse.TabIndex = 5;
            this.btnNewCourse.Text = "New";
            this.btnNewCourse.UseVisualStyleBackColor = false;
            this.btnNewCourse.Click += new System.EventHandler(this.btnNewCourse_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(464, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Credits";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(464, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 20);
            this.label5.TabIndex = 3;
            this.label5.Text = "Title";
            // 
            // txtCredits
            // 
            this.txtCredits.Location = new System.Drawing.Point(464, 115);
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.Size = new System.Drawing.Size(79, 26);
            this.txtCredits.TabIndex = 2;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(464, 55);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(230, 26);
            this.txtTitle.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lvEnrolments);
            this.groupBox3.Controls.Add(this.btnEnrolmentUpdate);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.txtGrade);
            this.groupBox3.Controls.Add(this.LblCourse);
            this.groupBox3.Controls.Add(this.lblStudent);
            this.groupBox3.Controls.Add(this.btnDeleteEnrol);
            this.groupBox3.Controls.Add(this.btnNewEnrol);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(51, 512);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(789, 282);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Enrolments - Link STUDENT with COURSE to ENROL and Record Grade";
            // 
            // lvEnrolments
            // 
            this.lvEnrolments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7});
            this.lvEnrolments.FullRowSelect = true;
            this.lvEnrolments.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvEnrolments.HideSelection = false;
            this.lvEnrolments.Location = new System.Drawing.Point(76, 100);
            this.lvEnrolments.Name = "lvEnrolments";
            this.lvEnrolments.Size = new System.Drawing.Size(624, 141);
            this.lvEnrolments.TabIndex = 23;
            this.lvEnrolments.UseCompatibleStateImageBehavior = false;
            this.lvEnrolments.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Id";
            this.columnHeader4.Width = 45;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Course";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Student";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Grade";
            this.columnHeader7.Width = 45;
            // 
            // btnEnrolmentUpdate
            // 
            this.btnEnrolmentUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnEnrolmentUpdate.ForeColor = System.Drawing.Color.White;
            this.btnEnrolmentUpdate.Location = new System.Drawing.Point(382, 249);
            this.btnEnrolmentUpdate.Name = "btnEnrolmentUpdate";
            this.btnEnrolmentUpdate.Size = new System.Drawing.Size(103, 29);
            this.btnEnrolmentUpdate.TabIndex = 11;
            this.btnEnrolmentUpdate.Text = "Update";
            this.btnEnrolmentUpdate.UseVisualStyleBackColor = false;
            this.btnEnrolmentUpdate.Click += new System.EventHandler(this.btnEnrolmentUpdate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(130, 252);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 20);
            this.label9.TabIndex = 10;
            this.label9.Text = "Grade";
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(200, 249);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(150, 26);
            this.txtGrade.TabIndex = 9;
            // 
            // LblCourse
            // 
            this.LblCourse.BackColor = System.Drawing.Color.White;
            this.LblCourse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblCourse.Location = new System.Drawing.Point(472, 62);
            this.LblCourse.Name = "LblCourse";
            this.LblCourse.Size = new System.Drawing.Size(226, 27);
            this.LblCourse.TabIndex = 8;
            // 
            // lblStudent
            // 
            this.lblStudent.BackColor = System.Drawing.Color.White;
            this.lblStudent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStudent.Location = new System.Drawing.Point(70, 62);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(294, 27);
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
            this.btnDeleteEnrol.Click += new System.EventHandler(this.btnDeleteEnrol_Click);
            // 
            // btnNewEnrol
            // 
            this.btnNewEnrol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNewEnrol.ForeColor = System.Drawing.Color.White;
            this.btnNewEnrol.Location = new System.Drawing.Point(382, 62);
            this.btnNewEnrol.Name = "btnNewEnrol";
            this.btnNewEnrol.Size = new System.Drawing.Size(75, 29);
            this.btnNewEnrol.TabIndex = 5;
            this.btnNewEnrol.Text = "Enrol";
            this.btnNewEnrol.UseVisualStyleBackColor = false;
            this.btnNewEnrol.Click += new System.EventHandler(this.btnNewEnrol_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(464, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(223, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Select Course from List Above";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(76, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(193, 20);
            this.label7.TabIndex = 3;
            this.label7.Text = "Select Student List Above";
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
            // btnSeedData
            // 
            this.btnSeedData.Location = new System.Drawing.Point(536, 15);
            this.btnSeedData.Name = "btnSeedData";
            this.btnSeedData.Size = new System.Drawing.Size(136, 32);
            this.btnSeedData.TabIndex = 20;
            this.btnSeedData.Text = "Seed Test Data";
            this.btnSeedData.UseVisualStyleBackColor = true;
            this.btnSeedData.Click += new System.EventHandler(this.btnSeedData_Click);
            // 
            // FrmCourseManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1306, 805);
            this.Controls.Add(this.btnSeedData);
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
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Button btnUpdateCourse;
      private System.Windows.Forms.Button btnDeletCourse;
      private System.Windows.Forms.Button btnNewCourse;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.TextBox txtCredits;
      private System.Windows.Forms.TextBox txtTitle;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label LblCourse;
      private System.Windows.Forms.Label lblStudent;
      private System.Windows.Forms.Button btnDeleteEnrol;
      private System.Windows.Forms.Button btnNewEnrol;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Label label7;
      private System.Windows.Forms.Label label8;
      private System.Windows.Forms.Button btnEnrolmentUpdate;
      private System.Windows.Forms.Label label9;
      private System.Windows.Forms.TextBox txtGrade;
      private System.Windows.Forms.Label label10;
      private System.Windows.Forms.TextBox txtCourseID;
      private System.Windows.Forms.Button btnSeedData;
      private System.Windows.Forms.ListView lvStudents;
      private System.Windows.Forms.ColumnHeader Id;
      private System.Windows.Forms.ColumnHeader First;
      private System.Windows.Forms.ColumnHeader Last;
      private System.Windows.Forms.ListView lvCourses;
      private System.Windows.Forms.ColumnHeader ColId;
      private System.Windows.Forms.ColumnHeader columnHeader1;
      private System.Windows.Forms.ColumnHeader columnHeader2;
      private System.Windows.Forms.ColumnHeader columnHeader3;
      private System.Windows.Forms.ListView lvEnrolments;
      private System.Windows.Forms.ColumnHeader columnHeader4;
      private System.Windows.Forms.ColumnHeader columnHeader5;
      private System.Windows.Forms.ColumnHeader columnHeader6;
      private System.Windows.Forms.ColumnHeader columnHeader7;
   }
}

