using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using Ex6_Mvp.Models;
using Microsoft.EntityFrameworkCore;


namespace Ex6_Mvp._Repositories
{
    //This is the Entityframework CRUD for the database, it is easy to test and review without breaking GUI

    // PM:> Install-Package Microsoft.EntityFramework
    // PM:> Install-Package Microsoft.EntityFrameworkCore.SqlServer
    // PM:> Install-Package Microsoft.EntityFrameworkCore.Proxies

    public class PetRepository : IPetRepository
    {
        //Constructor
        public DbContextOptionsBuilder<EFPetDb> optionsBuilder;

        public PetRepository()
        {
            //Setup Connection string holder
            optionsBuilder = new DbContextOptionsBuilder<EFPetDb>();
            optionsBuilder.UseSqlServer(EFPetDb.ConnectionString);

            // Required for Bi directional 1 to many loading
            //  optionsBuilder.UseLazyLoadingProxies(); // <-- no need to set here,
            // You enable Lazy Loading via efmodeller gui property

            if (Debugger.IsAttached)
            {
                optionsBuilder.EnableDetailedErrors();
                optionsBuilder.EnableSensitiveDataLogging();
                optionsBuilder.EnableDetailedErrors();
            }

            using (EFPetDb db = new EFPetDb(optionsBuilder.Options))
            {
                
                //if (!db.Database.Exists())
                //{
                    //Initalises just first time
                    DeleteandSeed();
                //}
            }
        }

        public void DeleteandSeed()
        {
            using (EFPetDb db = new EFPetDb(optionsBuilder.Options))
            {
                //Reset Database
                db.Database.EnsureDeleted();
                Console.WriteLine("Deleted DB\r\n");
                
                db.Database.EnsureCreated();
                Console.WriteLine("Created DB\r\n");

                Console.WriteLine(EFPetDb.ConnectionString);
            }
            SeedData();
        }

        public class Logger
        {
            public static void Log(string message)
            {
                Console.WriteLine("EF Message: {0} ", message);
            }
        }
        void SeedData()
        {
            using (EFPetDb db = new EFPetDb(optionsBuilder.Options))
            {
                List<Pet> Pets = new List<Pet>
                {
                    new Pet{Name= "Buttons", Type = "Dog", Colour = "White"},
                    new Pet{Name= "Coda", Type = "Cat", Colour = "Multicolor"},
                    new Pet{Name= "Merlin", Type = "Parrot", Colour = "Green-Yellow"},
                    new Pet{Name= "Nina", Type = "Turtle", Colour = "Dark Gray"},
                    new Pet{Name= "Luna", Type = "Rabbit", Colour = "White"},
                    new Pet{Name= "Domino", Type = "Hamster", Colour = "Orange"},
                    new Pet{Name= "Lucy", Type = "Monkey", Colour = "Brown"},
                    new Pet{Name= "Daysi", Type = "Horse", Colour = "White"},
                    new Pet{Name= "Zoe", Type = "Snake", Colour = "Yellow white"},
                    new Pet{Name= "Max", Type = "Budgie", Colour = "Yellow"},
                    new Pet{Name= "Charlie", Type = "Mouse", Colour = "White"},
                    new Pet{Name= "Rocky", Type = "Squirrel", Colour = "Brown-Orange"},
                    new Pet{Name= "Leo", Type = "Dog", Colour = "White-Black"},
                    new Pet{Name= "Loki", Type = "Cat", Colour = "Black"},
                    new Pet{Name= "Jasper", Type = "Dog", Colour = "Silver"},
                    
                };

                foreach (Pet p in Pets)
                {
                    Pet pet = new Pet(); 
                    pet.Name = p.Name;
                    pet.Type = p.Type;
                    pet.Colour = p.Colour;

                    db.Pets.Add(pet);
                    Console.WriteLine($"Added Pet: {pet.Name}");
                }
                db.SaveChanges();
                Console.WriteLine("Saved seed data to Db ok");
            }
        }


                //Methods
        public void Add(Pet petModel)
        {
            using (EFPetDb db = new EFPetDb(optionsBuilder.Options))
            {
                db.Pets.Add(petModel);
                db.SaveChanges();
            }
        }

        public void Delete(long id)
        {
            using (EFPetDb db = new EFPetDb(optionsBuilder.Options))
            {
                //Pet pet = db.Pets.FirstOrDefault(p => p.Id == id);
                Pet pet = db.Pets.Find(id);
                db.Pets.Remove(pet);
                db.SaveChanges();
            }
        }

        public void Edit(Pet pet)
        {
            using (EFPetDb db = new EFPetDb(optionsBuilder.Options))
            {
                Pet petinDb = db.Pets.Find(pet.Id);
                petinDb.Name = pet.Name;
                petinDb.Type = pet.Type;
                petinDb.Colour = pet.Colour;
                db.SaveChanges();
            }
        }

        public IEnumerable<Pet> GetAll()
        {
            var petList = new List<Pet>();
            using (EFPetDb db = new EFPetDb(optionsBuilder.Options))
            {
                petList = db.Pets.ToList();
            }
            return petList;
        }

        public IEnumerable<Pet> GetByValue(string value)
        {
            List<Pet> petList = new List<Pet>();
            int petId = int.TryParse(value, out _) ? Convert.ToInt32(value) : 0;

            using (EFPetDb db = new EFPetDb(optionsBuilder.Options))
            {
                // Query for all pets that match value in any field
                var enumpetList = from b in db.Pets
                                  where
                                  (
                                        b.Id == petId 
                                        || b.Name.ToUpper().Contains(value.ToUpper())
                                        || b.Type.ToUpper().Contains(value.ToUpper()) 
                                        || b.Colour.ToUpper().Contains(value.ToUpper())
                                   )
                                   select b;

                petList = enumpetList.ToList();
            }
            return petList;
        }
    }
}
