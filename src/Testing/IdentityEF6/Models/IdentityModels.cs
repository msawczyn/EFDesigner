using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace IdentitySample.Models
{
   // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
   public partial class ApplicationUser
   {
      public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
      {
         // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
         ClaimsIdentity userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
         // Add custom user claims here
         return userIdentity;
      }
   }



   public partial class ApplicationDbContext
   {
      static ApplicationDbContext()
      {
         // Set the database intializer which is run once during application start
         // This seeds the database with admin user credentials and admin role
         Database.SetInitializer(new ApplicationDbContextDatabaseInitializer());
      }

      public static ApplicationDbContext Create()
      {
         return new ApplicationDbContext();
      }
   }
}