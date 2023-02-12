
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
            this.Id = new System.Windows.Forms.ColumnHeader();
            this.First = new System.Windows.Forms.ColumnHeader();
            this.Last = new System.Windows.Forms.ColumnHeader();
            this.btnUpdateStudent = new System.Windows.Forms.Button();
            this.btnDeleteStudent = new System.Windows.Forms.Button();
            this.btnNewStudent = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLastname = new System.Windows.Forms.TextBox();
            this.txtFirstname = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lvCourses = new System.Windows.Forms.ListView();
            this.ColId = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
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
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
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
            this.btnBiDirectionalUpdate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDebug
            // 
            this.txtDebug.Location = new System.Drawing.Point(670, 37);
            this.txtDebug.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDebug.Multiline = true;
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(326, 558);
            this.txtDebug.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 15);
            this.label1.TabIndex = 14;
            this.label1.Text = "Sql Server Connection";
            // 
            // txtConnection
            // 
            this.txtConnection.Location = new System.Drawing.Point(36, 39);
            this.txtConnection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtConnection.Name = "txtConnection";
            this.txtConnection.Size = new System.Drawing.Size(572, 23);
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
            this.groupBox1.Location = new System.Drawing.Point(40, 66);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox1.Size = new System.Drawing.Size(568, 144);
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
            this.lvStudents.Location = new System.Drawing.Point(9, 16);
            this.lvStudents.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lvStudents.Name = "lvStudents";
            this.lvStudents.Size = new System.Drawing.Size(347, 118);
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
            this.btnUpdateStudent.Location = new System.Drawing.Point(488, 112);
            this.btnUpdateStudent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnUpdateStudent.Name = "btnUpdateStudent";
            this.btnUpdateStudent.Size = new System.Drawing.Size(58, 22);
            this.btnUpdateStudent.TabIndex = 7;
            this.btnUpdateStudent.Text = "Update";
            this.btnUpdateStudent.UseVisualStyleBackColor = false;
            this.btnUpdateStudent.Click += new System.EventHandler(this.btnUpdateStudent_Click);
            // 
            // btnDeleteStudent
            // 
            this.btnDeleteStudent.BackColor = System.Drawing.Color.Red;
            this.btnDeleteStudent.ForeColor = System.Drawing.Color.White;
            this.btnDeleteStudent.Location = new System.Drawing.Point(426, 112);
            this.btnDeleteStudent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Size = new System.Drawing.Size(58, 22);
            this.btnDeleteStudent.TabIndex = 6;
            this.btnDeleteStudent.Text = "Del";
            this.btnDeleteStudent.UseVisualStyleBackColor = false;
            this.btnDeleteStudent.Click += new System.EventHandler(this.btnDeleteStudent_Click);
            // 
            // btnNewStudent
            // 
            this.btnNewStudent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNewStudent.ForeColor = System.Drawing.Color.White;
            this.btnNewStudent.Location = new System.Drawing.Point(362, 112);
            this.btnNewStudent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNewStudent.Name = "btnNewStudent";
            this.btnNewStudent.Size = new System.Drawing.Size(58, 22);
            this.btnNewStudent.TabIndex = 5;
            this.btnNewStudent.Text = "New";
            this.btnNewStudent.UseVisualStyleBackColor = false;
            this.btnNewStudent.Click += new System.EventHandler(this.btnNewStudent_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(361, 66);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Lastname";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(361, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Firstname";
            // 
            // txtLastname
            // 
            this.txtLastname.Location = new System.Drawing.Point(361, 83);
            this.txtLastname.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtLastname.Name = "txtLastname";
            this.txtLastname.Size = new System.Drawing.Size(180, 23);
            this.txtLastname.TabIndex = 2;
            this.txtLastname.TextChanged += new System.EventHandler(this.txtLastname_TextChanged);
            // 
            // txtFirstname
            // 
            this.txtFirstname.Location = new System.Drawing.Point(361, 38);
            this.txtFirstname.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtFirstname.Name = "txtFirstname";
            this.txtFirstname.Size = new System.Drawing.Size(180, 23);
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
            this.groupBox2.Location = new System.Drawing.Point(40, 224);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox2.Size = new System.Drawing.Size(568, 144);
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
            this.lvCourses.Location = new System.Drawing.Point(9, 19);
            this.lvCourses.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lvCourses.Name = "lvCourses";
            this.lvCourses.Size = new System.Drawing.Size(347, 116);
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
            this.label10.Location = new System.Drawing.Point(473, 69);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 15);
            this.label10.TabIndex = 9;
            this.label10.Text = "Course ID";
            // 
            // txtCourseID
            // 
            this.txtCourseID.Location = new System.Drawing.Point(473, 86);
            this.txtCourseID.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCourseID.Name = "txtCourseID";
            this.txtCourseID.Size = new System.Drawing.Size(62, 23);
            this.txtCourseID.TabIndex = 8;
            // 
            // btnUpdateCourse
            // 
            this.btnUpdateCourse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnUpdateCourse.ForeColor = System.Drawing.Color.White;
            this.btnUpdateCourse.Location = new System.Drawing.Point(488, 112);
            this.btnUpdateCourse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnUpdateCourse.Name = "btnUpdateCourse";
            this.btnUpdateCourse.Size = new System.Drawing.Size(58, 22);
            this.btnUpdateCourse.TabIndex = 7;
            this.btnUpdateCourse.Text = "Update";
            this.btnUpdateCourse.UseVisualStyleBackColor = false;
            this.btnUpdateCourse.Click += new System.EventHandler(this.btnUpdateCourse_Click);
            // 
            // btnDeletCourse
            // 
            this.btnDeletCourse.BackColor = System.Drawing.Color.Red;
            this.btnDeletCourse.ForeColor = System.Drawing.Color.White;
            this.btnDeletCourse.Location = new System.Drawing.Point(426, 112);
            this.btnDeletCourse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDeletCourse.Name = "btnDeletCourse";
            this.btnDeletCourse.Size = new System.Drawing.Size(58, 22);
            this.btnDeletCourse.TabIndex = 6;
            this.btnDeletCourse.Text = "Del";
            this.btnDeletCourse.UseVisualStyleBackColor = false;
            this.btnDeletCourse.Click += new System.EventHandler(this.btnDeletCourse_Click);
            // 
            // btnNewCourse
            // 
            this.btnNewCourse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNewCourse.ForeColor = System.Drawing.Color.White;
            this.btnNewCourse.Location = new System.Drawing.Point(362, 112);
            this.btnNewCourse.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNewCourse.Name = "btnNewCourse";
            this.btnNewCourse.Size = new System.Drawing.Size(58, 22);
            this.btnNewCourse.TabIndex = 5;
            this.btnNewCourse.Text = "New";
            this.btnNewCourse.UseVisualStyleBackColor = false;
            this.btnNewCourse.Click += new System.EventHandler(this.btnNewCourse_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(361, 69);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Credits";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(361, 20);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 15);
            this.label5.TabIndex = 3;
            this.label5.Text = "Title";
            // 
            // txtCredits
            // 
            this.txtCredits.Location = new System.Drawing.Point(361, 86);
            this.txtCredits.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtCredits.Name = "txtCredits";
            this.txtCredits.Size = new System.Drawing.Size(62, 23);
            this.txtCredits.TabIndex = 2;
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(361, 41);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(180, 23);
            this.txtTitle.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnBiDirectionalUpdate);
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
            this.groupBox3.Location = new System.Drawing.Point(40, 384);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBox3.Size = new System.Drawing.Size(614, 212);
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
            this.lvEnrolments.Location = new System.Drawing.Point(59, 75);
            this.lvEnrolments.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lvEnrolments.Name = "lvEnrolments";
            this.lvEnrolments.Size = new System.Drawing.Size(486, 107);
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
            this.btnEnrolmentUpdate.Location = new System.Drawing.Point(161, 187);
            this.btnEnrolmentUpdate.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnEnrolmentUpdate.Name = "btnEnrolmentUpdate";
            this.btnEnrolmentUpdate.Size = new System.Drawing.Size(80, 22);
            this.btnEnrolmentUpdate.TabIndex = 11;
            this.btnEnrolmentUpdate.Text = "Update";
            this.btnEnrolmentUpdate.UseVisualStyleBackColor = false;
            this.btnEnrolmentUpdate.Click += new System.EventHandler(this.btnEnrolmentUpdate_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(60, 188);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 15);
            this.label9.TabIndex = 10;
            this.label9.Text = "Grade";
            // 
            // txtGrade
            // 
            this.txtGrade.Location = new System.Drawing.Point(115, 186);
            this.txtGrade.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtGrade.Name = "txtGrade";
            this.txtGrade.Size = new System.Drawing.Size(42, 23);
            this.txtGrade.TabIndex = 9;
            // 
            // LblCourse
            // 
            this.LblCourse.BackColor = System.Drawing.Color.White;
            this.LblCourse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblCourse.Location = new System.Drawing.Point(367, 46);
            this.LblCourse.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LblCourse.Name = "LblCourse";
            this.LblCourse.Size = new System.Drawing.Size(176, 21);
            this.LblCourse.TabIndex = 8;
            // 
            // lblStudent
            // 
            this.lblStudent.BackColor = System.Drawing.Color.White;
            this.lblStudent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblStudent.Location = new System.Drawing.Point(54, 46);
            this.lblStudent.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStudent.Name = "lblStudent";
            this.lblStudent.Size = new System.Drawing.Size(229, 21);
            this.lblStudent.TabIndex = 7;
            // 
            // btnDeleteEnrol
            // 
            this.btnDeleteEnrol.BackColor = System.Drawing.Color.Red;
            this.btnDeleteEnrol.ForeColor = System.Drawing.Color.White;
            this.btnDeleteEnrol.Location = new System.Drawing.Point(551, 158);
            this.btnDeleteEnrol.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnDeleteEnrol.Name = "btnDeleteEnrol";
            this.btnDeleteEnrol.Size = new System.Drawing.Size(58, 22);
            this.btnDeleteEnrol.TabIndex = 6;
            this.btnDeleteEnrol.Text = "Delete";
            this.btnDeleteEnrol.UseVisualStyleBackColor = false;
            this.btnDeleteEnrol.Click += new System.EventHandler(this.btnDeleteEnrol_Click);
            // 
            // btnNewEnrol
            // 
            this.btnNewEnrol.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.btnNewEnrol.ForeColor = System.Drawing.Color.White;
            this.btnNewEnrol.Location = new System.Drawing.Point(297, 46);
            this.btnNewEnrol.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnNewEnrol.Name = "btnNewEnrol";
            this.btnNewEnrol.Size = new System.Drawing.Size(58, 22);
            this.btnNewEnrol.TabIndex = 5;
            this.btnNewEnrol.Text = "Enrol";
            this.btnNewEnrol.UseVisualStyleBackColor = false;
            this.btnNewEnrol.Click += new System.EventHandler(this.btnNewEnrol_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(361, 24);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 15);
            this.label6.TabIndex = 4;
            this.label6.Text = "Select Course from List Above";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(59, 24);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(140, 15);
            this.label7.TabIndex = 3;
            this.label7.Text = "Select Student List Above";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(677, 19);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 15);
            this.label8.TabIndex = 19;
            this.label8.Text = "Debug messages";
            // 
            // btnSeedData
            // 
            this.btnSeedData.Location = new System.Drawing.Point(417, 11);
            this.btnSeedData.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSeedData.Name = "btnSeedData";
            this.btnSeedData.Size = new System.Drawing.Size(106, 24);
            this.btnSeedData.TabIndex = 20;
            this.btnSeedData.Text = "Seed Test Data";
            this.btnSeedData.UseVisualStyleBackColor = true;
            this.btnSeedData.Click += new System.EventHandler(this.btnSeedData_Click);
            // 
            // btnBiDirectionalUpdate
            // 
            this.btnBiDirectionalUpdate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.btnBiDirectionalUpdate.ForeColor = System.Drawing.Color.White;
            this.btnBiDirectionalUpdate.Location = new System.Drawing.Point(347, 187);
            this.btnBiDirectionalUpdate.Margin = new System.Windows.Forms.Padding(2);
            this.btnBiDirectionalUpdate.Name = "btnBiDirectionalUpdate";
            this.btnBiDirectionalUpdate.Size = new System.Drawing.Size(199, 22);
            this.btnBiDirectionalUpdate.TabIndex = 24;
            this.btnBiDirectionalUpdate.Text = "Bi Directional Update Demo";
            this.btnBiDirectionalUpdate.UseVisualStyleBackColor = false;
            this.btnBiDirectionalUpdate.Click += new System.EventHandler(this.btnBiDirectionalUpdate_Click);
            // 
            // FrmCourseManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 604);
            this.Controls.Add(this.btnSeedData);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtConnection);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
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
        private System.Windows.Forms.Button btnBiDirectionalUpdate;
    }
}

