using IdentitySample.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace IdentitySample.Controllers
{
   [Authorize(Roles = "Admin")]
   public class UsersAdminController : Controller
   {
      public UsersAdminController()
      {
      }

      public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
      {
         UserManager = userManager;
         RoleManager = roleManager;
      }

      private ApplicationUserManager _userManager;
      public ApplicationUserManager UserManager
      {
         get
         {
            return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
         }
         private set
         {
            _userManager = value;
         }
      }

      private ApplicationRoleManager _roleManager;
      public ApplicationRoleManager RoleManager
      {
         get
         {
            return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
         }
         private set
         {
            _roleManager = value;
         }
      }

      //
      // GET: /Users/
      [HttpGet]
      public async Task<ActionResult> Index()
      {
         return View(await UserManager.Users.ToListAsync());
      }

      //
      // GET: /Users/Details/5
      [HttpGet]
      public async Task<ActionResult> Details(string id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ApplicationUser user = await UserManager.FindByIdAsync(id);

         ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

         return View(user);
      }

      //
      // GET: /Users/Create
      [HttpGet]
      public async Task<ActionResult> Create()
      {
         //Get the list of Roles
         ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
         return View();
      }

      //
      // POST: /Users/Create
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
      {
         if (ModelState.IsValid)
         {
            ApplicationUser user = new ApplicationUser(userViewModel.Email) { Email = userViewModel.Email };

            // Add the Address Info:
            user.Address = userViewModel.Address;
            user.City = userViewModel.City;
            user.State = userViewModel.State;
            user.PostalCode = userViewModel.PostalCode;

            Microsoft.AspNet.Identity.IdentityResult adminresult = await UserManager.CreateAsync(user, userViewModel.Password);

            //Add User to the selected Roles 
            if (adminresult.Succeeded)
            {
               if (selectedRoles != null)
               {
                  Microsoft.AspNet.Identity.IdentityResult result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                  if (!result.Succeeded)
                  {
                     ModelState.AddModelError("", result.Errors.First());
                     ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name");
                     return View();
                  }
               }
            }
            else
            {
               ModelState.AddModelError("", adminresult.Errors.First());
               ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
               return View();

            }
            return RedirectToAction("Index");
         }
         ViewBag.RoleId = new SelectList(RoleManager.Roles, "Name", "Name");
         return View();
      }

      //
      // GET: /Users/Edit/1
      [HttpGet]
      public async Task<ActionResult> Edit(string id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ApplicationUser user = await UserManager.FindByIdAsync(id);
         if (user == null)
         {
            return HttpNotFound();
         }

         System.Collections.Generic.IList<string> userRoles = await UserManager.GetRolesAsync(user.Id);

         return View(new EditUserViewModel()
         {
            Id = user.Id,
            Email = user.Email,
            RolesList = RoleManager.Roles.ToList().Select(x => new SelectListItem()
            {
               Selected = userRoles.Contains(x.Name),
               Text = x.Name,
               Value = x.Name
            })
         });
      }

      //
      // POST: /Users/Edit/5
      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Edit([Bind(Include = "Email,Id")] EditUserViewModel editUser, params string[] selectedRole)
      {
         if (ModelState.IsValid)
         {
            ApplicationUser user = await UserManager.FindByIdAsync(editUser.Id);
            if (user == null)
            {
               return HttpNotFound();
            }

            user.UserName = editUser.Email;
            user.Email = editUser.Email;

            System.Collections.Generic.IList<string> userRoles = await UserManager.GetRolesAsync(user.Id);

            selectedRole = selectedRole ?? new string[] { };

            Microsoft.AspNet.Identity.IdentityResult result = await UserManager.AddToRolesAsync(user.Id, selectedRole.Except(userRoles).ToArray<string>());

            if (!result.Succeeded)
            {
               ModelState.AddModelError("", result.Errors.First());
               return View();
            }
            result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRole).ToArray<string>());

            if (!result.Succeeded)
            {
               ModelState.AddModelError("", result.Errors.First());
               return View();
            }
            return RedirectToAction("Index");
         }
         ModelState.AddModelError("", "Something failed.");
         return View();
      }

      //
      // GET: /Users/Delete/5
      [HttpGet]
      public async Task<ActionResult> Delete(string id)
      {
         if (id == null)
         {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
         }
         ApplicationUser user = await UserManager.FindByIdAsync(id);
         if (user == null)
         {
            return HttpNotFound();
         }
         return View(user);
      }

      //
      // POST: /Users/Delete/5
      [HttpPost]
      [ActionName("Delete")]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> DeleteConfirmed(string id)
      {
         if (ModelState.IsValid)
         {
            if (id == null)
            {
               return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
               return HttpNotFound();
            }
            Microsoft.AspNet.Identity.IdentityResult result = await UserManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
               ModelState.AddModelError("", result.Errors.First());
               return View();
            }
            return RedirectToAction("Index");
         }
         return View();
      }
   }
}
