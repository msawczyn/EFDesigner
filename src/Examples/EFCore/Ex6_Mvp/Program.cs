using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ex6_Mvp.Models;
using Ex6_Mvp.Presenters;
using Ex6_Mvp._Repositories;
using Ex6_Mvp.Views;
using System.Configuration;

namespace Ex6_Mvp
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

            IMainView view = new MainView();           
            
            new MainPresenter(view);

            Application.Run((Form)view);
        }
    }
}
