using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UPProjects.Data;
using UPProjects.Models;
using UPProjects.Models.AccountViewModel;
namespace UPProjects.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signinManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly AppCommonMethod _appCommonMethod;
        private readonly UserStore _userStore;
        private readonly IDNTCaptchaValidatorService _validatorService;
        private List<Claim> claims = new List<Claim>();
        public AccountController(UserStore userStore, SignInManager<ApplicationUser> signinManager,
           UserManager<ApplicationUser> userManager, ILogger<ApplicationUser> logger,AppCommonMethod appCommonMethod, IDNTCaptchaValidatorService validatorService)
        {
            _signinManager = signinManager;
            _userManager = userManager;
            _userStore = userStore;
            _logger = logger;
            _userManager.PasswordHasher = new NewPwdHasher();
            _appCommonMethod= appCommonMethod;
            _validatorService = validatorService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
           // var result = (dynamic)null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (!_validatorService.HasRequestValidCaptchaEntry(Language.English, DisplayMode.ShowDigits))
                    {
                       // this.ModelState.AddModelError(DNTCaptchaTagHelper.CaptchaInputName, "Please Enter Valid Captcha.");
                        ModelState.AddModelError(string.Empty, "Please Enter Valid Captcha.");
                        return View();
                    }
                    var res = (dynamic)null;
                    AppCommonMethod objAppCommonMethod = new AppCommonMethod();
                    string password = "";
                    string input = loginViewModel.Password.ToString();
                    password = objAppCommonMethod.EncryptMD5(input);
                    res = await _signinManager.PasswordSignInAsync(loginViewModel.Email, password, loginViewModel.RememberMe, lockoutOnFailure: true);
                    if (res.Succeeded)
                    {
                       string Role=  _userStore.UpdateLoginInfo(loginViewModel.Email.ToString());
                        _logger.LogInformation("User logged in.");
                        if(Role=="unit")
                        return RedirectToAction("Dashboard", "User");
                        if (Role == "gm")
                            return RedirectToAction("GMDashboard", "User");
                        if (Role == "admin" )
                            return RedirectToAction("HQDashboard", "User");
                        if (Role == "srv")
                            return RedirectToAction("HQDashboard", "User");
                    }
                    else if (res.RequiresTwoFactor)
                    {
                        return RedirectToPage("./LoginWith2fa", new
                        {
                        });
                    }
                    else if (res.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return RedirectToPage("./Lockout");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Please enter valid UserId/Password.");
                        return View();
                    }
                }
            }
            catch (Exception ex)
            { await _appCommonMethod.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", loginViewModel.Email); }
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            Response.Cookies.Delete("idnajdiiskwwwkijijodwidiwjfffffmpu");
            await _signinManager.SignOutAsync();
            return RedirectToAction("Index", "Account");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel ChangePassword)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            if (ModelState.IsValid)
            {
                try
                {
                    string userid = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value;
                    innerresult = _userStore.ChangePassword(ChangePassword , userid);
                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Change Password"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                    await _appCommonMethod.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }


        public async Task<IActionResult> UserProfileEdit()
        {
            UserProfileEdit objprof = new UserProfileEdit();
            try
            {
                string userid = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value;
                objprof = _userStore.GetUserProfileEdit(userid);
                objprof.UserId = userid;
            }
            catch (Exception ex)
            {
                await _appCommonMethod.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }
            return View(objprof);
        }



        [HttpPost]
        public async Task<IActionResult> UserProfileEdit(UserProfileEdit usprof)
        {
            var Result = (dynamic)null;
            var innerresult = (dynamic)null;
            if (ModelState.IsValid)
            {
                try
                {
                    usprof.UserId = ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value;
                    innerresult = _userStore.UpdateUserProfile(usprof);
                    var DynamicResult = new
                    {
                        innerresult = innerresult,
                        status = true,
                        eventKey = "Update Profile"
                    };
                    Result = new { data = "", dynamicResult = DynamicResult };

                }
                catch (Exception ex)
                {
                    await _appCommonMethod.InsertException(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "", ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
                }
            }
            return Json(Result);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GenerateRandomPassword()
        {
            var pwd = "";
            try
            {
                PasswordOptions opts = null;
                opts = new PasswordOptions()
                {
                    RequiredLength = 10,
                    RequiredUniqueChars = 4,
                    RequireDigit = true,
                    RequireLowercase = true,
                    RequireNonAlphanumeric = true,
                    RequireUppercase = true
                };

                string[] randomChars = new[] {
                                "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
                                "abcdefghijkmnopqrstuvwxyz",    // lowercase
                                "0123456789",                   // digits
                                "@$!%*#^&"                        // non-alphanumeric
    };
                Random rand = new Random(Environment.TickCount);
                List<char> chars = new List<char>();

                if (opts.RequireUppercase)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[0][rand.Next(0, randomChars[0].Length)]);

                if (opts.RequireLowercase)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[1][rand.Next(0, randomChars[1].Length)]);

                if (opts.RequireDigit)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[2][rand.Next(0, randomChars[2].Length)]);

                if (opts.RequireNonAlphanumeric)
                    chars.Insert(rand.Next(0, chars.Count),
                        randomChars[3][rand.Next(0, randomChars[3].Length)]);

                for (int i = chars.Count; i < opts.RequiredLength
                    || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
                {
                    string rcs = randomChars[rand.Next(0, randomChars.Length)];
                    chars.Insert(rand.Next(0, chars.Count),
                        rcs[rand.Next(0, rcs.Length)]);
                }
                pwd = new string(chars.ToArray());
            }

            catch (Exception ex)
            {
                _appCommonMethod.InsertExceptionsync(ex.Message, this.ControllerContext.RouteData.Values["controller"].ToString(), this.ControllerContext.RouteData.Values["action"].ToString(), "",
              ((ClaimsIdentity)this.User.Identity).FindFirst("UserId").Value);
            }

            return Json(pwd);
        }

    }
}