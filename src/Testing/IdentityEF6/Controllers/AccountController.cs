using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace IdentitySample.Controllers
{
   [Authorize]
   public class AccountController : Controller
   {
      public AccountController()
      {
      }

      public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
      {
         UserManager = userManager;
         SignInManager = signInManager;
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

      //
      // GET: /Account/Login
      [HttpGet]
      [AllowAnonymous]
      public ActionResult Login(string returnUrl)
      {
         ViewBag.ReturnUrl = returnUrl;
         return View();
      }

      private ApplicationSignInManager _signInManager;

      public ApplicationSignInManager SignInManager
      {
         get
         {
            return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
         }
         private set { _signInManager = value; }
      }

      //
      // POST: /Account/Login
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
      {
         if (!ModelState.IsValid)
         {
            return View(model);
         }

         // This doen't count login failures towards lockout only two factor authentication
         // To enable password failures to trigger lockout, change to shouldLockout: true
         SignInStatus result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
         switch (result)
         {
            case SignInStatus.Success:
               return RedirectToLocal(returnUrl);
            case SignInStatus.LockedOut:
               return View("Lockout");
            case SignInStatus.RequiresVerification:
               return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
            case SignInStatus.Failure:
            default:
               ModelState.AddModelError("", "Invalid login attempt.");
               return View(model);
         }
      }

      //
      // GET: /Account/VerifyCode
      [HttpGet]
      [AllowAnonymous]
      public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
      {
         // Require that the user has already logged in via username/password or external login
         if (!await SignInManager.HasBeenVerifiedAsync())
         {
            return View("Error");
         }
         ApplicationUser user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
         if (user != null)
         {
            ViewBag.Status = "For DEMO purposes the current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
         }
         return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
      }

      //
      // POST: /Account/VerifyCode
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
      {
         if (!ModelState.IsValid)
         {
            return View(model);
         }

         SignInStatus result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
         switch (result)
         {
            case SignInStatus.Success:
               return RedirectToLocal(model.ReturnUrl);
            case SignInStatus.LockedOut:
               return View("Lockout");
            case SignInStatus.Failure:
            default:
               ModelState.AddModelError("", "Invalid code.");
               return View(model);
         }
      }

      //
      // GET: /Account/Register
      [HttpGet]
      [AllowAnonymous]
      public ActionResult Register()
      {
         return View();
      }

      //
      // POST: /Account/Register
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> Register(RegisterViewModel model)
      {
         if (ModelState.IsValid)
         {
            ApplicationUser user = new ApplicationUser(model.Email) { Email = model.Email };
            user.Address = model.Address;
            user.City = model.City;
            user.State = model.State;
            user.PostalCode = model.PostalCode;

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
               string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
               string callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
               await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
               ViewBag.Link = callbackUrl;
               return View("DisplayEmail");
            }
            AddErrors(result);
         }

         // If we got this far, something failed, redisplay form
         return View(model);
      }

      //
      // GET: /Account/ConfirmEmail
      [HttpGet]
      [AllowAnonymous]
      public async Task<ActionResult> ConfirmEmail(string userId, string code)
      {
         if (userId == null || code == null)
         {
            return View("Error");
         }
         IdentityResult result = await UserManager.ConfirmEmailAsync(userId, code);
         return View(result.Succeeded ? "ConfirmEmail" : "Error");
      }

      //
      // GET: /Account/ForgotPassword
      [HttpGet]
      [AllowAnonymous]
      public ActionResult ForgotPassword()
      {
         return View();
      }

      //
      // POST: /Account/ForgotPassword
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
      {
         if (ModelState.IsValid)
         {
            ApplicationUser user = await UserManager.FindByNameAsync(model.Email);
            if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
               // Don't reveal that the user does not exist or is not confirmed
               return View("ForgotPasswordConfirmation");
            }

            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            string callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
            ViewBag.Link = callbackUrl;
            return View("ForgotPasswordConfirmation");
         }

         // If we got this far, something failed, redisplay form
         return View(model);
      }

      //
      // GET: /Account/ForgotPasswordConfirmation
      [HttpGet]
      [AllowAnonymous]
      public ActionResult ForgotPasswordConfirmation()
      {
         return View();
      }

      //
      // GET: /Account/ResetPassword
      [HttpGet]
      [AllowAnonymous]
      public ActionResult ResetPassword(string code)
      {
         return code == null ? View("Error") : View();
      }

      //
      // POST: /Account/ResetPassword
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
      {
         if (!ModelState.IsValid)
         {
            return View(model);
         }
         ApplicationUser user = await UserManager.FindByNameAsync(model.Email);
         if (user == null)
         {
            // Don't reveal that the user does not exist
            return RedirectToAction("ResetPasswordConfirmation", "Account");
         }
         IdentityResult result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
         if (result.Succeeded)
         {
            return RedirectToAction("ResetPasswordConfirmation", "Account");
         }
         AddErrors(result);
         return View();
      }

      //
      // GET: /Account/ResetPasswordConfirmation
      [HttpGet]
      [AllowAnonymous]
      public ActionResult ResetPasswordConfirmation()
      {
         return View();
      }

      //
      // POST: /Account/ExternalLogin
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public ActionResult ExternalLogin(string provider, string returnUrl)
      {
         // Request a redirect to the external login provider
         return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
      }

      //
      // GET: /Account/SendCode
      [HttpGet]
      [AllowAnonymous]
      public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
      {
         string userId = await SignInManager.GetVerifiedUserIdAsync();
         if (userId == null)
         {
            return View("Error");
         }
         System.Collections.Generic.IList<string> userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
         System.Collections.Generic.List<SelectListItem> factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
         return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
      }

      //
      // POST: /Account/SendCode
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> SendCode(SendCodeViewModel model)
      {
         if (!ModelState.IsValid)
         {
            return View();
         }

         // Generate the token and send it
         if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
         {
            return View("Error");
         }
         return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
      }

      //
      // GET: /Account/ExternalLoginCallback
      [HttpGet]
      [AllowAnonymous]
      public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
      {
         ExternalLoginInfo loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
         if (loginInfo == null)
         {
            return RedirectToAction("Login");
         }

         // Sign in the user with this external login provider if the user already has a login
         SignInStatus result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
         switch (result)
         {
            case SignInStatus.Success:
               return RedirectToLocal(returnUrl);
            case SignInStatus.LockedOut:
               return View("Lockout");
            case SignInStatus.RequiresVerification:
               return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
            case SignInStatus.Failure:
            default:
               // If the user does not have an account, then prompt the user to create an account
               ViewBag.ReturnUrl = returnUrl;
               ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
               return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
         }
      }

      //
      // POST: /Account/ExternalLoginConfirmation
      [HttpPost]
      [AllowAnonymous]
      [ValidateAntiForgeryToken]
      public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
      {
         if (User.Identity.IsAuthenticated)
         {
            return RedirectToAction("Index", "Manage");
         }

         if (ModelState.IsValid)
         {
            // Get the information about the user from the external login provider
            ExternalLoginInfo info = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
               return View("ExternalLoginFailure");
            }
            ApplicationUser user = new ApplicationUser(model.Email) { Email = model.Email };
            IdentityResult result = await UserManager.CreateAsync(user);
            if (result.Succeeded)
            {
               result = await UserManager.AddLoginAsync(user.Id, info.Login);
               if (result.Succeeded)
               {
                  await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                  return RedirectToLocal(returnUrl);
               }
            }
            AddErrors(result);
         }

         ViewBag.ReturnUrl = returnUrl;
         return View(model);
      }

      //
      // POST: /Account/LogOff
      [HttpPost]
      [ValidateAntiForgeryToken]
      public ActionResult LogOff()
      {
         AuthenticationManager.SignOut();
         return RedirectToAction("Index", "Home");
      }

      //
      // GET: /Account/ExternalLoginFailure
      [HttpGet]
      [AllowAnonymous]
      public ActionResult ExternalLoginFailure()
      {
         return View();
      }

      #region Helpers
      // Used for XSRF protection when adding external logins
      private const string XsrfKey = "XsrfId";

      private IAuthenticationManager AuthenticationManager
      {
         get
         {
            return HttpContext.GetOwinContext().Authentication;
         }
      }

      private void AddErrors(IdentityResult result)
      {
         foreach (string error in result.Errors)
         {
            ModelState.AddModelError("", error);
         }
      }

      private ActionResult RedirectToLocal(string returnUrl)
      {
         if (Url.IsLocalUrl(returnUrl))
         {
            return Redirect(returnUrl);
         }
         return RedirectToAction("Index", "Home");
      }

      internal class ChallengeResult : HttpUnauthorizedResult
      {
         public ChallengeResult(string provider, string redirectUri)
             : this(provider, redirectUri, null)
         {
         }

         public ChallengeResult(string provider, string redirectUri, string userId)
         {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
         }

         public string LoginProvider { get; set; }
         public string RedirectUri { get; set; }
         public string UserId { get; set; }

         public override void ExecuteResult(ControllerContext context)
         {
            AuthenticationProperties properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null)
            {
               properties.Dictionary[XsrfKey] = UserId;
            }
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
         }
      }
      #endregion
   }
}