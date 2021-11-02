using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EF6
{

   // Entity Framework Visual Editor - creates .efmodel etc from a GUI tool
   //https://marketplace.visualstudio.com/items?itemName=michaelsawczyn.EFDesigner


   // ----------  SETUP DATABASE SERVER - MsSqlServer LocalDb -------- //

   //Download SQL Server Express   - LocalDB
   // https://www.sqlshack.com/install-microsoft-sql-server-express-localdb/
   // https://www.microsoft.com/en-au/download/confirmation.aspx?id=101064
   // Microsoft SQL Server Express LocalDB supports silent installation. A user should download SqlLocalDB.msi and run the Command Prompt window as an administrator. Then, they should paste the following command:
   // msiexec /i SqlLocalDB.msi /qn IACCEPTSQLLOCALDBLICENSETERMS = YES


   // Initialise Database File using Cmd Prompt

   /* https://www.sqlshack.com/how-to-connect-and-use-microsoft-sql-server-express-localdb/
   C:\>sqllocaldb info v11
   Name:               v11
   Version:            13.1.4001.0
   Shared name:
   Owner:              DEV\David
   Auto-create:        No
   State:              Stopped
   Last start time:    2/11/2021 1:02:19 PM
   Instance pipe name:

   C:\>SqlLocalDB start v11
   LocalDB instance "v11" started.

   C:\>sqllocaldb info v11
   Name:               v11
   Version:            13.1.4001.0
   Shared name:
   Owner:              CV-DEV\David-PWD
   Auto-create:        No
   State:              Running
   Last start time:    2/11/2021 1:04:51 PM
   Instance pipe name: np:\\.\pipe\LOCALDB#385051FC\tsql\query
   
    */

   // Download and Use: SSMS - Sql Server Management Studio - Create Database 'EFLocalDb'
   // https://www.microsoft.com/en-au/download/details.aspx?id=101064
   // Paste your named pipe from above np:\\.\pipe\LOCALDB#385051FC\tsql\query 

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
            Application.Run(new frmExamples());
        }
    }

   //NOTE: Microsofts .EDMX visual tools DO NOT WORK - EDMX Entitfy framework gui tools from Microsoft suck!
   // DO NOT USE ->https://docs.microsoft.com/en-us/visualstudio/data-tools/entity-data-model-tools-in-visual-studio?view=vs-2019


}
