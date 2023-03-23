using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ex7_Mvp.Views
{
    public partial class MainView : Form, IMainView
    {
        public MainView()
        {
            InitializeComponent();
           
        }

        public event EventHandler ShowPetView;
        public event EventHandler ShowOwnerView; //To Do
        public event EventHandler ShowVetsView;

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnPets_Click(object sender, EventArgs e)
        {
            ShowPetView?.Invoke(this, EventArgs.Empty);
        }

        private void MainView_Load(object sender, EventArgs e)
        {
            //Show default 
            ShowPetView?.Invoke(this, EventArgs.Empty);
        }

       
    }
}
