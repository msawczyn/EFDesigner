using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex2_ModelOne2One
{
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
            
            SetupLocalDb("mssqllocaldb", "EFVisualExamples");

            Application.Run(new FrmOne2One());

        }


        static void SetupLocalDb(string InstanceName = "mssqllocaldb", string DatabaseName = "EFVisualExamples") //This is a default instance name in local DB v13
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