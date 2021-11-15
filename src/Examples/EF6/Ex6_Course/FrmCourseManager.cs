using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Windows.Forms;

namespace Ex6_Course
{
    public partial class FrmCourseManager : Form
    {
        public FrmCourseManager()
        {
            InitializeComponent();
        }

        public class Logger
        {
            //TO DO Show in text box on screen

            public static void Log(string message)
            {
                Console.WriteLine("EF Message: {0} ", message);
            }
        }

        private void FrmCourseManager_Load(object sender, EventArgs e)
        {
            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;

                db.Database.Delete();
                txtDebug.Text += "Deleted DB\r\n";

                db.Database.CreateIfNotExists();
                txtDebug.Text += "Created DB\r\n";

                txtConnection.Text = db.Database.Connection.ConnectionString;
            }

            SeedData();

            DatabaseLoad_Enrolments();

            DatabaseLoad_Students();
            DatabaseLoad_Courses();

        }

        #region Student-CRUD

        private void lvStudents_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lvStudents.SelectedItems.Count == 0)
            {
                lblStudent.Text = "";
                return;
            }

            int selectedIndex = lvStudents.SelectedIndices[0];

            ListViewItem lvItem = lvStudents.Items[selectedIndex];

            string sPk = lvItem.SubItems[0].Text;
            txtFirstname.Text = lvItem.SubItems[1].Text;
            txtLastname.Text = lvItem.SubItems[2].Text;

            lblStudent.Text = sPk + ":" + txtFirstname.Text + " " + txtLastname.Text;
        }
        void DatabaseLoad_Students()
        {

            lvStudents.Items.Clear();

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;

                DbSet<Student> people = db.Students;

                foreach (Student p in people)
                {
                    txtDebug.Text += String.Format("Loaded: {0} {1} {2} ", p.StudentId, p.FirstName, p.LastName) + "\r\n";

                    string[] row = { p.StudentId.ToString(), p.FirstName, p.LastName };
                    ListViewItem listViewItem = new ListViewItem(row);
                    lvStudents.Items.Add(listViewItem);
                }
            }
        }

        private void btnNewStudent_Click(object sender, EventArgs e)
        {

            txtDebug.Text = "btnNewStudent_Click()\r\n";

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;

                Student student = db.Students.Create();

                student.FirstName = txtFirstname.Text;
                student.LastName = txtLastname.Text;

                db.Students.Add(student);

                // This Exception handler helps to describe what went wrong with the EF database save.
                // It decodes why the data did not comply with defined database field structure. e.g too long or wrong type.
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }
            DatabaseLoad_Students();
        }
        private void btnDeleteStudent_Click(object sender, EventArgs e)
        {
            //Get StudentId from Listview
            if (lvStudents.SelectedItems.Count == 0)
            {
                return;
            }

            int selectedIndex = lvStudents.SelectedIndices[0];
            ListViewItem lvItem = lvStudents.Items[selectedIndex];
            string sPk = lvItem.SubItems[0].Text;
            long pk = Convert.ToInt64(sPk);

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;
                // Get student to delete
                Student StudentToDelete = db.Students.First(s => s.StudentId == pk);

                if (StudentToDelete != null)
                {
                    // Delete 
                    db.Students.Remove(StudentToDelete);
                    db.SaveChanges();
                }
            }
            txtFirstname.Text = txtLastname.Text = "";
            DatabaseLoad_Students();
        }

        private void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            //Get StudentId from Listview
            if (lvStudents.SelectedItems.Count == 0)
            {
                return;
            }

            int selectedIndex = lvStudents.SelectedIndices[0];
            ListViewItem lvItem = lvStudents.Items[selectedIndex];
            string sPk = lvItem.SubItems[0].Text;
            long pk = Convert.ToInt64(sPk);

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;

                Student student = db.Students.SingleOrDefault(b => b.StudentId == pk);
                if (student != null)
                {
                    student.FirstName = txtFirstname.Text;
                    student.LastName = txtLastname.Text;

                    db.SaveChanges();
                }
            }
            DatabaseLoad_Students();

        }
        #endregion

        #region Courses-CRUD

        private void lvCourses_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (lvCourses.SelectedItems.Count == 0)
            {
                LblCourse.Text = "";
                return;
            }

            int selectedIndex = lvCourses.SelectedIndices[0];

            ListViewItem lvItem = lvCourses.Items[selectedIndex];

            string sPk = lvItem.SubItems[0].Text;
            txtCourseID.Text = lvItem.SubItems[1].Text;
            txtTitle.Text = lvItem.SubItems[2].Text;
            txtCredits.Text = lvItem.SubItems[3].Text;

            LblCourse.Text = sPk + ":" + txtCourseID.Text + " " + txtTitle.Text;
        }

        void DatabaseLoad_Courses()
        {

            lvCourses.Items.Clear();

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;

                DbSet<Course> courses = db.Courses;

                int count = courses.Count();

                foreach (Course c in courses)
                {
                    txtDebug.Text += String.Format("Course Loaded: {0} {1} {2} {3} ", c.CourseId, c.CourseLabel, c.Title, c.Credits) + "\r\n";

                    //Note: Could do tuple
                    string[] row = { c.CourseId.ToString(), c.CourseLabel, c.Title, c.Credits.ToString() };
                    ListViewItem listViewItem = new ListViewItem(row);
                    lvCourses.Items.Add(listViewItem);
                }
                lvCourses.Refresh();
            }

        }

        private void btnNewCourse_Click(object sender, EventArgs e)
        {

            txtDebug.Text = "btnNewCourse_Click()\r\n";

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;

                Course course = db.Courses.Create();

                course.CourseLabel = txtCourseID.Text;
                course.Title = txtTitle.Text;
                if (txtCredits.Text != "")
                    course.Credits = int.Parse(txtCredits.Text);

                db.Courses.Add(course);

                // This Exception handler helps to describe what went wrong with the EF database save.
                // It decodes why the data did not comply with defined database field structure. e.g too long or wrong type.
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

            DatabaseLoad_Courses();
        }

        private void btnDeletCourse_Click(object sender, EventArgs e)
        {
            //Get PrimaryKey from Listview
            if (lvCourses.SelectedItems.Count == 0)
            {
                return;
            }

            int selectedIndex = lvCourses.SelectedIndices[0];
            ListViewItem lvItem = lvCourses.Items[selectedIndex];
            string sPk = lvItem.SubItems[0].Text;
            long pk = Convert.ToInt64(sPk);

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;
                // Get course to delete
                Course CourseToDelete = db.Courses.First(c => c.CourseId == pk);

                if (CourseToDelete != null)
                {
                    // Delete 
                    db.Courses.Remove(CourseToDelete);
                    db.SaveChanges();
                }
            }

            txtCourseID.Text = txtTitle.Text = txtCredits.Text = "";

            DatabaseLoad_Courses();
        }

        private void btnUpdateCourse_Click(object sender, EventArgs e)
        {
            //Get PrimaryKey from Listview
            if (lvCourses.SelectedItems.Count == 0)
            {
                return;
            }

            int selectedIndex = lvCourses.SelectedIndices[0];
            ListViewItem lvItem = lvCourses.Items[selectedIndex];
            string sPk = lvItem.SubItems[0].Text;
            long pk = Convert.ToInt64(sPk);

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;
                // Get course to delete
                Course CourseToUpdate = db.Courses.First(c => c.CourseId == pk);

                if (CourseToUpdate != null)
                {
                    CourseToUpdate.CourseLabel = txtCourseID.Text;
                    CourseToUpdate.Title = txtTitle.Text;
                    if (txtCredits.Text != "")
                        CourseToUpdate.Credits = int.Parse(txtCredits.Text);

                    db.SaveChanges();
                }
            }
            DatabaseLoad_Courses();
        }

        #endregion

        #region Enrollment-CRUD
        private void btnNewEnrol_Click(object sender, EventArgs e)
        {
            txtDebug.Text = "btnNewEnrol_Click()\r\n";

            //Get CourseId from Listview
            if (lvCourses.SelectedItems.Count == 0)
            {
                return;
            }

            int selectedIndex = lvCourses.SelectedIndices[0];
            ListViewItem lvItem = lvCourses.Items[selectedIndex];
            string sPk = lvItem.SubItems[0].Text;
            long CoursePk = Convert.ToInt64(sPk);

            //Get StudentId from Listview
            if (lvStudents.SelectedItems.Count == 0)
            {
                return;
            }

            selectedIndex = lvStudents.SelectedIndices[0];
            lvItem = lvStudents.Items[selectedIndex];
            sPk = lvItem.SubItems[0].Text;
            long StudentPk = Convert.ToInt64(sPk);

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;

                Enrollment enroll = db.Enrollments.Create();

                if (txtGrade.Text != "")
                    enroll.Grade = Convert.ToInt32(txtGrade.Text);

                Course CourseToLink = db.Courses.First(c => c.CourseId == CoursePk);
                Student StudentToLink = db.Students.First(s => s.StudentId == StudentPk);

                StudentToLink.Enrollments.Add(enroll);
                CourseToLink.Enrollments.Add(enroll);

                db.Enrollments.Add(enroll);

                // This Exception handler helps to describe what went wrong with the EF database save.
                // It decodes why the data did not comply with defined database field structure. e.g too long or wrong type.
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

            DatabaseLoad_Enrolments();


        }
        private void btnEnrolmentUpdate_Click(object sender, EventArgs e)
        {
            txtDebug.Text = "btnEnrolmentUpdate_Click()\r\n";

            //Get CourseId from Listview
            if (lvEnrolments.SelectedItems.Count == 0)
            {
                txtDebug.Text = "NO Enrolment Selected? \r\n";
                return;
            }

            int selectedIndex = lvEnrolments.SelectedIndices[0];
            ListViewItem lvItem = lvEnrolments.Items[selectedIndex];


            string sPk = lvItem.SubItems[0].Text;
            long EnrolPk = Convert.ToInt64(sPk);

            using (CourseManager db = new CourseManager())
            {

                db.Database.Log = Logger.Log;
                Enrollment EnrolToUpdate = db.Enrollments.First(en => en.EnrollmentId == EnrolPk);

                int grade = 0;
                var isNumeric = int.TryParse(txtGrade.Text, out grade);

                if (isNumeric)
                    EnrolToUpdate.Grade = int.Parse(txtGrade.Text);

                //With Bidirectional Association you can access child objects via parent, e.g Course.Student etc
                
                //Changes work in linked tables
                Student stu = EnrolToUpdate.Student;
                stu.FirstName = "Albert";
                stu.LastName = "Einstein";

                Course course = EnrolToUpdate.Course;
                course.Title = "Relativity";


                // This Exception handler helps to describe what went wrong with the EF database save.
                // It decodes why the data did not comply with defined database field structure. e.g too long or wrong type.
                try
                {
                    db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                {
                    Exception raise = dbEx;
                    foreach (DbEntityValidationResult validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (DbValidationError validationError in validationErrors.ValidationErrors)
                        {
                            string message = string.Format("{0}:{1}",
                                validationErrors.Entry.Entity.ToString(),
                                validationError.ErrorMessage);
                            // raise a new exception nesting the current instance as InnerException  
                            raise = new InvalidOperationException(message, raise);
                        }
                    }
                    throw raise;
                }
            }

            DatabaseLoad_Enrolments();
        }

        private void btnDeleteEnrol_Click(object sender, EventArgs e)
        {
            //Get PrimaryKey from Listview
            if (lvEnrolments.SelectedItems.Count == 0)
            {
                txtDebug.Text = "Cannot delete, NO Enrolment Selected ? \r\n";
                return;
            }

            int selectedIndex = lvEnrolments.SelectedIndices[0];
            ListViewItem lvItem = lvEnrolments.Items[selectedIndex];
            string sPk = lvItem.SubItems[0].Text;
            long pk = Convert.ToInt64(sPk);

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;
                // Get course to delete
                Enrollment EnrolToDelete = db.Enrollments.First(en => en.EnrollmentId == pk);

                if (EnrolToDelete != null)
                {
                    // Delete 
                    db.Enrollments.Remove(EnrolToDelete);
                    db.SaveChanges();
                }
            }

            txtGrade.Text = "";

            DatabaseLoad_Enrolments();
        }

        void DatabaseLoad_Enrolments()
        {
            lvEnrolments.Items.Clear();

            using (CourseManager db = new CourseManager())
            {
                db.Database.Log = Logger.Log;

                DbSet<Enrollment> enrolments = db.Enrollments;
               
                foreach (Enrollment en in enrolments)
                {
                    string StudentName = "";
                    string CourseTitle = "";

                    /*
                    //If you get already an open datareader error?
                    
                    //Simply include "MultipleActiveResultSets=true" in your connection string

                    It is not about closing connection. EF manages connection correctly. 
                    My understanding of this problem is that there are multiple data retrieval commands executed on single connection 
                    (or single command with multiple selects) while next DataReader is executed before first one has completed the reading. 
                    The only way to avoid the exception is to allow multiple nested DataReaders = turn on MultipleActiveResultSets.
                    Another scenario when this always happens is when you iterate through result of the query (IQueryable) 
                    and you will trigger lazy loading for loaded entity inside the iteration.
                    
                    */

                    Course course = en.Course;

                    if (course != null)
                        CourseTitle = course.Title;

                    Student stu = en.Student;

                    if (stu != null)
                        StudentName = stu.FirstName + " " + stu.LastName;
                 
                    txtDebug.Text += String.Format("Loaded: {0} {1} {2} {3} ", en.EnrollmentId, CourseTitle, StudentName, en.Grade.ToString());//;   e.EnrollmentId, e.EnrollmentId);//  + "\r\n";

                    string[] row = { en.EnrollmentId.ToString(), CourseTitle, StudentName, en.Grade.ToString() };

                    ListViewItem listViewItem = new ListViewItem(row);
                    lvEnrolments.Items.Add(listViewItem);
                }
            }
        }


        #endregion


        #region GUIActions

        private void LbCourses_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Get Record and Fill Textboxes

        }


        private void txtFirstname_TextChanged(object sender, EventArgs e)
        {
            //Show Add 
        }


        private void txtLastname_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion


        #region SeedDb
        void SeedData()
        {
            using (CourseManager db = new CourseManager())
            {
                List<Student> students = new List<Student>
            {
            new Student{FirstName="Carson",LastName="Alexander" },//,EnrollmentDate=DateTime.Parse("2005-09-01")},
            new Student{FirstName="Meredith",LastName="Alonso"},//EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstName="Arturo",LastName="Anand"},//EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstName="Gytis",LastName="Barzdukas"},//EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstName="Yan",LastName="Li"},//EnrollmentDate=DateTime.Parse("2002-09-01")},
            new Student{FirstName="Peggy",LastName="Justice"},//EnrollmentDate=DateTime.Parse("2001-09-01")},
            new Student{FirstName="Laura",LastName="Norman"},//EnrollmentDate=DateTime.Parse("2003-09-01")},
            new Student{FirstName="Nino",LastName="Olivetto"}//EnrollmentDate=DateTime.Parse("2005-09-01")}
            };

                //This does not work
                //students.ForEach(s => db.Students.Add(s));

                //Have to use create?  Dont know why?
                foreach (Student s in students)
                {
                    Student stu = db.Students.Create();
                    stu.FirstName = s.FirstName;
                    stu.LastName = s.LastName;

                    db.Students.Add(stu);
                }

                //Duplicate Key added?

                db.SaveChanges();

                List<Course> courses = new List<Course>
            {
            new Course{CourseLabel="1050",Title="Chemistry",Credits=3,},
            new Course{CourseLabel="4022",Title="Microeconomics",Credits=3,},
            new Course{CourseLabel="4041",Title="Macroeconomics",Credits=3,},
            new Course{CourseLabel="1045",Title="Calculus",Credits=4,},
            new Course{CourseLabel="3141",Title="Trigonometry",Credits=4,},
            new Course{CourseLabel="2021",Title="Composition",Credits=3,},
            new Course{CourseLabel="2042",Title="Literature",Credits=4,}
            };

                //This does not work
                //courses.ForEach(s => db.Courses.Add(s));

                //Have to use create?  Dont know why?
                foreach (Course c in courses)
                {
                    Course course = db.Courses.Create();
                    course.CourseLabel = c.CourseLabel;
                    course.Title = c.Title;
                    course.Credits = c.Credits;

                    db.Courses.Add(course);
                }

                db.SaveChanges();
            }

            DatabaseLoad_Students();
            DatabaseLoad_Courses();
        }

        #endregion

        private void btnSeedData_Click(object sender, EventArgs e)
        {

            SeedData();

        }

       
    }
}