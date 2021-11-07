using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
               var listViewItem = new ListViewItem(row);
               lvStudents.Items.Add(listViewItem); 
            }
         }
      }

      void DatabaseLoad_Courses()
      {

         lvCourses.Items.Clear();

         using (CourseManager db = new CourseManager())
         {
            db.Database.Log = Logger.Log;

            DbSet<Course> courses = db.Courses;

            foreach (Course c in courses)
            {
               txtDebug.Text += String.Format("Course Loaded: {0} {1} {2} {3} ", c.CourseId, c.CourseLabel, c.Title, c.Credits) + "\r\n";

               //Note: Could do tuple
               string[] row = { c.CourseId.ToString(), c.CourseLabel, c.Title, c.Credits.ToString()};
               var listViewItem = new ListViewItem(row);
               lvCourses.Items.Add(listViewItem);
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
         Int64 StudentId = 1;

         using (CourseManager db = new CourseManager())
         {
            db.Database.Log = Logger.Log;
            // Get student to delete
            Student StudentToDelete = db.Students.First(s => s.StudentId == StudentId);

            if (StudentToDelete != null)
            {
               // Delete 
               db.Students.Remove(StudentToDelete);
               db.SaveChanges();
            }
         }
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

      private void btnNewCourse_Click(object sender, EventArgs e)
      {
         DatabaseLoad_Courses();
      }

      private void btnDeletCourse_Click(object sender, EventArgs e)
      {

         DatabaseLoad_Courses();
      }

      private void btnUpdateCourse_Click(object sender, EventArgs e)
      {

         DatabaseLoad_Courses();
      }

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
            var students = new List<Student>
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
               var stu = db.Students.Create();
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
               var course = db.Courses.Create();
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

         lblStudent.Text = sPk + ":" + txtFirstname.Text+ " " + txtLastname.Text;
      }

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

      private void btnNewEnrol_Click(object sender, EventArgs e)
      {

      }
   }
}
