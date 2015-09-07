using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MyFollow.DAL;
using MyFollow.Models;
using System.Net;

namespace MyFollow.Controllers
{
  //  [Authorize]
    public class AccountController : Controller
    {
        MyFollowContext db = new MyFollowContext();
        public List<RegisterViewModel> RegList;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        //
        // GET: /Account/Login
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

        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
      //  [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var user = await UserManager.FindByEmailAsync(model.Email);
                    var rolename= await UserManager.GetRolesAsync(user.Id);
                    if (rolename[0] == "ProductOwner" && user.EmailConfirmed)
                    {
                        return RedirectToAction("Index", "ProductOwners", new { email = user.Email });
                    }
                    if (rolename[0] == "Admin" )
                    {
                        return RedirectToAction("Index"); 
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }


         [Authorize(Roles = "Admin")]
        public ActionResult AllProductOwner()
        {
           var user = UserManager.Users.ToList().Where(r => r.EmailConfirmed == false && r.CompanyName != null);
            RegList = new List<RegisterViewModel>();
            foreach (var item in user)
            {
                var register = new RegisterViewModel();
                register.Id = item.Id;
                register.Name = item.Name;
                register.CompanyName = item.CompanyName;
                register.Email = item.Email;
                RegList.Add(register);
            }
            return View(RegList);
        }

         [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Approve(int id)
        {
            if (ModelState.IsValid)
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(id);
                if (code != null)
                {
                    if (Request.Url != null)
                    {
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = id, code }, protocol: Request.Url.Scheme);
                        await UserManager.SendEmailAsync(id, "Confirm your account", "Thank you For Confirming Your Details.Registrer With More Details Click this link: <a href=\"" + callbackUrl + "\">link</a>");
                    }
                }
                return RedirectToAction("AllProductOwner");

            }
            return RedirectToAction("AllProductOwner");
        }

         [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductOwner productOwner = db.ProductOwners.Find(id);
            if (productOwner == null)
            {
                return HttpNotFound();
            }
            return View(productOwner);
        }

        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            var user = await UserManager.FindByIdAsync(await SignInManager.GetVerifiedUserIdAsync());
            if (user != null)
            {
                ViewBag.Status = "Forthe current " + provider + " code is: " + await UserManager.GenerateTwoFactorTokenAsync(user.Id, provider);
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: false, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
           // RoleManager.CreateAsync(new ApplicationRole("Admin"));
           // RoleManager.CreateAsync(new ApplicationRole("ProductOwner"));
            //RoleManager.CreateAsync(new ApplicationRole("User"));
            return View();
        }

       // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,Name=model.Name,CompanyName=model.CompanyName };
               // var result = await UserManager.CreateAsync(user, model.Password);
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await UserManager.AddToRolesAsync(user.Id, "ProductOwner");
                    ViewBag.EmaiId = user.Email;
                    return View("ConfirmRegistration");
                }
                AddErrors(result);
            }

            return View(model);
        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(int userId, string code)
        {
            if (userId > 0 || code == null)
            {
                var result = await UserManager.ConfirmEmailAsync(userId, code);
                var user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var productowner = db.ProductOwners.FirstOrDefault(x => x.EmailId == user.Email);
                    if (productowner == null)
                    {
                        if (result.Succeeded)
                        {
                            return RedirectToAction("AddProductOwner", new { id = userId });
                        }
                    }
                    return View("AlreadyRegister");
                }
            }
            return View("Error");
        }

        public async Task<ActionResult> AddProductOwner(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.Email = user.Email;
            TempData["EmailId"] = user.Email;

            ViewBag.CompanyName = user.CompanyName;
            TempData["CompanyName"] = user.CompanyName;

            ViewBag.Name = user.Name;
            TempData["Name"] = user.Name;
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProductOwner(ProductOwner productOwner)
        {
            productOwner.EmailId = TempData["EmailId"].ToString();
            productOwner.OwnerName = TempData["Name"].ToString();
            productOwner.CompanyName = TempData["CompanyName"].ToString();
            if (ModelState.IsValid)
            {
                db.ProductOwners.Add(productOwner);
                db.SaveChanges();
               // var user = UserManager.FindByIdAsync(Convert.ToInt32(User.Identity.GetUserId()));
              //  UserManager.UpdateAsync();
                return View("ConfirmApproval");
            }

           return View("ConfirmApproval");
        }
        
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return View("ForgotPasswordConfirmation");
                }
                var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                if (Request.Url != null)
                {
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code1 = code }, protocol: Request.Url.Scheme);
                    await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>");
                    ViewBag.Link = callbackUrl;
                }
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

      
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("ViewPages", "EndUser");
                case SignInStatus.LockedOut:
                    return View("Lockout");
                //case SignInStatus.RequiresVerification:
                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

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
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, Name = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    await UserManager.AddToRolesAsync(user.Id, "User");
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ViewPages", "EndUser");
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
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
            foreach (var error in result.Errors)
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
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
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