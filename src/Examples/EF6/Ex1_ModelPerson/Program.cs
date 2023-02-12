using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex1_Person
{



    // ---- Objective: Easy GUI Database create and use ---
    // Uses models / classes to setup data that is then stored in a Database.

    // ---------  BACKGROUND  --------------
    // The no ORM (Object Relationship Model) of doing Basic Database Operations is using C# hand crafted SQL CRUD
    // Works for simple database work but does not scale and is hard to maintiain.
    //https://www.geeksforgeeks.org/basic-database-operations-using-c-sharp/


    // ---------  Introducing ENTITY FRAMEWORK   --------------
    // This is a modern ORM way of using DB access, robust and solid using Code First & Fluent API  (
    // Unfortunatley the 'Code first' and 'Fluent API' are console like commands that require a deep dive to utilise and apply to your requirements.

    // ---------  Problem --------------
    // The Entity framework Microsoft ADO.Net modeller GUI wrapper for 'Code first' and 'Fluent API' modelling tool .edmx is fragile and incomplete.

    // Microsofts .EDMX visual tools DO NOT WORK - EDMX Entitfy framework gui tools from Microsoft suck!
    // DO NOT USE ->https://docs.microsoft.com/en-us/visualstudio/data-tools/entity-data-model-tools-in-visual-studio?view=vs-2019


    // ----------- Solution: Entity Framework Visual Editor  ------------------
    // An open source EF GUI tool works, here are some examples of how you get setup to use it..

    // It makes it easy to visualise, create and maintain complex relational databases


    // ---------  HOW TO GET SETUP --------------
    // Entity Framework Visual Editor - creates .efmodel etc from a GUI tool
    //https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner


    // ----------  SETUP DATABASE SERVER - MsSqlServer LocalDb -------- //

    // LocalDB is a special, low impact version of the SQL Server engine, that is not installed as a Windows Service,
    // but launched(made to run) on demand by the ADO.NET client opening a connection to it.It is intended for single user scenarios, and not for any production use -
    // for production you should use SQL Server Express(or higher)

    //Download SQL Server Express   - LocalDB
    // https://www.sqlshack.com/install-microsoft-sql-server-express-localdb/
    // https://www.microsoft.com/en-au/download/confirmation.aspx?id=101064
    // Microsoft SQL Server Express LocalDB supports silent installation. A user should download SqlLocalDB.msi and run the Command Prompt window as an administrator. Then, they should paste the following command:
    // msiexec /i SqlLocalDB.msi /qn IACCEPTSQLLOCALDBLICENSETERMS = YES


    // Initialise Database File using Cmd Prompt

    /* https://www.sqlshack.com/how-to-connect-and-use-microsoft-sql-server-express-localdb/

    C:\>sqllocaldb create DbInstance

    //Connection string is now 
    //Data Source=(localdb)\DbInstance;Initial Catalog=EFLocalDb;Integrated Security=True

    C:\>sqllocaldb info DbInstance
    Name:               DbInstance
    Version:            13.1.4001.0
    Shared name:
    Owner:              DEV\David
    Auto-create:        No
    State:              Stopped
    Last start time:    2/11/2021 1:02:19 PM
    Instance pipe name:  <- Pipe name entry missing, SqlLocalDB not started?  Should auto start

    C:\>SqlLocalDB start v11
    LocalDB instance "v11" started.

    C:\>sqllocaldb info DbInstance
    Name:               DbInstance
    Version:            13.1.4001.0
    Shared name:
    Owner:              DEV\David
    Auto-create:        No
    State:              Running
    Last start time:    2/11/2021 1:04:51 PM
    Instance pipe name: np:\\.\pipe\LOCALDB#385051FC\tsql\query

     */

    // Download and Use: SSMS - Sql Server Management Studio - Create Database 'EFLocalDb'
    // https://www.microsoft.com/en-au/download/details.aspx?id=101064
    // Paste your connection as (localdb)\DbInstance and use windows permisions

    // Note: SSMS database tool can hold a lock on the Database so the code cannot delete or update the database, simply close SSMS.

    //NOTE: Microsofts .EDMX visual tools DO NOT WORK - EDMX Entitfy framework gui tools from Microsoft suck!
    // DO NOT USE ->https://docs.microsoft.com/en-us/visualstudio/data-tools/entity-data-model-tools-in-visual-studio?view=vs-2019

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Match Connection string in .efmodel 
            //Data Source=(localdb)\mssqllocaldb;Initial Catalog=EFVisualExamples;Integrated Security=True
            SetupLocalDb("mssqllocaldb", "EFVisualExamples");

            Application.Run(new frmPerson());
           
        }


        static void SetupLocalDb(string InstanceName = "mssqllocaldb", string DatabaseName = "HomeOpenDb") //This is a default instance name in local DB v13
        {
            //Step 1: Islocaldb installed?
            if (!CreateLocalDBInstance(InstanceName))
            {

                MessageBox.Show("CRITICAL ERROR: SqlLocalDb software is not installed!");

                //Download SQL Server Express   - LocalDB
                // https://www.sqlshack.com/install-microsoft-sql-server-express-localdb/
                // https://www.microsoft.com/en-au/download/confirmation.aspx?id=101064
                // Microsoft SQL Server Express LocalDB supports silent installation. A user should download SqlLocalDB.msi and run the Command Prompt window as an administrator. Then, they should paste the following command:
                // msiexec /i SqlLocalDB.msi /qn IACCEPTSQLLOCALDBLICENSETERMS = YES
                string url = @"https://www.microsoft.com/en-au/download/confirmation.aspx?id=101064";
                Process.Start(url);

                Application.Exit();
            }

            //Step2: Create Database

            string connectionString = @"Data Source=(localdb)\" + InstanceName + $"; Integrated Security=True;";

            if (!CheckDatabaseExists(connectionString, DatabaseName))
            {

                string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\EFVisualExamples";
                Directory.CreateDirectory(AppDataPath);

                SqlConnection connection = new SqlConnection(connectionString);
                using (connection)
                {
                    connection.Open();

                    string sql = $@"
                            CREATE DATABASE
                                {DatabaseName}
                            ON PRIMARY (
                                NAME={DatabaseName}_data,
                                FILENAME = '{AppDataPath}\{DatabaseName}.mdf'
                            )
                            LOG ON (
                                NAME={DatabaseName}_log,
                                FILENAME = '{AppDataPath}\{DatabaseName}.ldf'
                            )";

                    SqlCommand command = new SqlCommand(sql, connection);
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"CRITICAL ERROR: Database cannot be created\r\n{ex.ToString()}\r\nDoes Db exists in another instance?\r\n(Note: You cannot duplicate names across instances- requires unique name)");
                        //You can debug using commandline
                        //>sqllocaldb info mssqllocaldb

                        //https://docs.microsoft.com/en-us/sql/tools/sqllocaldb-utility?view=sql-server-ver15

                        string url = @"https://docs.microsoft.com/en-us/sql/relational-databases/express-localdb-instance-apis/command-line-management-tool-sqllocaldb-exe?view=sql-server-ver15";
                        Process.Start(url);

                        Application.Exit();
                    }

                    MessageBox.Show($" A New empty Local Database has been created successfully!\r\n\r\nSQLLOCALDB:{InstanceName}\r\nDATABASE:{DatabaseName}");
                }
            }
        }

        static bool CreateLocalDBInstance(string InstanceName)
        {
            // Lists all instances
            // >sqllocaldb info

            // Start the child process.
            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = $"/C sqllocaldb c {InstanceName}";  //Create Local Db Instance
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.Start();
            string sOutput = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            //If LocalDb is not installed then it will return that 'sqllocaldb' is not recognized as an internal or external command operable program or batch file.
            if (sOutput == null || sOutput.Trim().Length == 0 || sOutput.Contains("not recognized"))
                return false;
            if (sOutput.ToLower().Contains(InstanceName))
                return true;
            return false;
        }

        public static bool CheckDatabaseExists(string connectionString, string databaseName)
        {

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{databaseName}')", connection))
                {
                    connection.Open();
                    var retval = command.ExecuteScalar();

                    return (retval != DBNull.Value);
                }
            }
        }
    }
}