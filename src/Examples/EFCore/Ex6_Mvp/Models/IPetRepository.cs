using System.Collections.Generic;

namespace Ex6_Mvp.Models
{
    // This is created manually to expose repository (database methods)
    // Used by the presenter to databind the GUI
    public interface IPetRepository
    {
        void Add(Pet petModel);
        void Edit(Pet petModel);
        void Delete(long id);
        IEnumerable<Pet> GetAll();
        IEnumerable<Pet> GetByValue(string value);//Searchs


        //  Service Broker?

        // Sql Dependancy
    }
}
