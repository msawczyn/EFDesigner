using System;
using Ex6_Mvp.Views;
using Ex6_Mvp.Models;
using Ex6_Mvp._Repositories;

namespace Ex6_Mvp.Presenters
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
