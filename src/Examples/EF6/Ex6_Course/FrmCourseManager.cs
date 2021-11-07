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
      }

      void DatabaseLoad_Students()
      {
         //WIP
         //var clients = new List<Tuple<int, string, string>>();
         //clients = GetClients();
         //comboBoxClient.DataSource = clients;
         //comboBoxClient.DisplayMember = clients[0].Item2;

         using (CourseManager db = new CourseManager())
         {
            DbSet<Student> people = db.Students;

            foreach (Student p in people)
            {
               txtDebug.Text += String.Format("Loaded: {0} {1} {2} ", p.StudentId, p.FirstName, p.LastName) + "\r\n";
              

            }
         }

      }

      private void btnNewStudent_Click(object sender, EventArgs e)
      {

         txtDebug.Text = "btnNewStudent_Click()\r\n";

         using (CourseManager db = new CourseManager())
         {
            Student person = db.Students.Create();
          
            person.FirstName = txtFirstname.Text;
            person.LastName = txtLastname.Text;

            db.Students.Add(person);

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

      }

      private void btnUpdateStudent_Click(object sender, EventArgs e)
      {

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

    
   }
}
