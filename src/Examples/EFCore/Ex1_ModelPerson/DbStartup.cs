using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;


// Step 1: Entity Framework Core has come a long way from only supporting Microsoft SQL Server.
//PM > Install-Package Microsoft.EntityFrameworkCore.SqlServer
using Microsoft.EntityFrameworkCore;


namespace Ex1_ModelPerson
{
    public partial class PersonModel : DbContext
    {
        // https://learn.microsoft.com/en-us/ef/core/dbcontext-configuration/#dbcontextoptions

        // In EF Core the configuration system is very flexible,
        // and the connection string could be stored in :
        //  1. appsettings.json,
        //  2. an environment variable,
        //  3. the user secret store,
        //  4. or another configuration source.

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("myrealconnectionstring");
        //}

        partial void CustomInit(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(PersonModel.ConnectionString);
        }
    }
}
