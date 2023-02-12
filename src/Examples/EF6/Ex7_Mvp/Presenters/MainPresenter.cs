using System;
using Ex7_Mvp.Views;
using Ex7_Mvp.Models;
using Ex7_Mvp._Repositories;

namespace Ex7_Mvp.Presenters
{
    public class MainPresenter
    {
        private IMainView mainView;
       
        public MainPresenter(IMainView mainView)
        {
            this.mainView = mainView;
            this.mainView.ShowPetView += ShowPetsView;
        }

        private void ShowPetsView(object sender, EventArgs e)
        {
            IPetView view = PetView.GetInstace((MainView)mainView);

            IPetRepository repository = new PetRepository();

            new PetPresenter(view, repository);
        }
    }
}
