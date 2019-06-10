using System.Linq;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;

namespace IdentitySample.Models
{
   // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

   public class ApplicationUserManager : UserManager<ApplicationUser>
   {
      public ApplicationUserManager(IUserStore<ApplicationUser> store)
          : base(store)
      {
      }

      public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
          IOwinContext context)
      {
         ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
         // Configure validation logic for usernames
         manager.UserValidator = new UserValidator<ApplicationUser>(manager)
         {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
         };
         // Configure validation logic for passwords
         manager.PasswordValidator = new PasswordValidator
         {
            RequiredLength = 1,
            RequireNonLetterOrDigit = false,
            RequireDigit = false,
            RequireLowercase = false,
            RequireUppercase = false,
         };
         // Configure user lockout defaults
         manager.UserLockoutEnabledByDefault = true;
         manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
         manager.MaxFailedAccessAttemptsBeforeLockout = 5;
         // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
         // You can write your own provider and plug in here.
         manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
         {
            MessageFormat = "Your security code is: {0}"
         });
         manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
         {
            Subject = "SecurityCode",
            BodyFormat = "Your security code is {0}"
         });
         manager.EmailService = new EmailService();
         manager.SmsService = new SmsService();
         Microsoft.Owin.Security.DataProtection.IDataProtectionProvider dataProtectionProvider = options.DataProtectionProvider;
         if (dataProtectionProvider != null)
         {
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
         }
         return manager;
      }
   }

   // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
   public class ApplicationRoleManager : RoleManager<IdentityRole>
   {
      public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
          : base(roleStore)
      {
      }

      public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
      {
         return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
      }
   }

   public class EmailService : IIdentityMessageService
   {
      public Task SendAsync(IdentityMessage message)
      {
         // Plug in your email service here to send an email.
         return Task.FromResult(0);
      }
   }

   public class SmsService : IIdentityMessageService
   {
      public Task SendAsync(IdentityMessage message)
      {
         // Plug in your sms service here to send a text message.
         return Task.FromResult(0);
      }
   }

   // This is useful if you do not want to tear down the database each time you run the application.
   // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
   // This example shows you how to create a new database if the Model changes
   public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
   {
      protected override void Seed(ApplicationDbContext context)
      {
         InitializeIdentityForEF(context);
         base.Seed(context);
      }

      //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
      public static void InitializeIdentityForEF(ApplicationDbContext db)
      {
         ApplicationUserManager userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
         ApplicationRoleManager roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
         const string name = "admin@example.com";
         const string password = "Admin@123456";
         const string roleName = "Admin";

         //Create Role Admin if it does not exist
         IdentityRole role = roleManager.FindByName(roleName);
         if (role == null)
         {
            role = new IdentityRole(roleName);
            IdentityResult roleresult = roleManager.Create(role);
         }

         ApplicationUser user = userManager.FindByName(name);
         if (user == null)
         {
            user = new ApplicationUser(name) { Email = name };
            IdentityResult result = userManager.Create(user, password);
            result = userManager.SetLockoutEnabled(user.Id, false);
         }

         // Add user admin to Role Admin if not already added
         IList<string> rolesForUser = userManager.GetRoles(user.Id);
         if (!rolesForUser.Contains(role.Name))
         {
            IdentityResult result = userManager.AddToRole(user.Id, role.Name);
         }
      }
   }

   public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
   {
      public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
          base(userManager, authenticationManager)
      { }

      public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
      {
         return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
      }

      public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
      {
         return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
      }
   }
}