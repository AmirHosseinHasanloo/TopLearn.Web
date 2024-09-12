using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Convertors;
using TopLearn.Core.DTOs;
using TopLearn.Core.Generators;
using TopLearn.Core.Security;
using TopLearn.Core.Senders;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Constractor Injection

        private IUserService _userService;

        private IViewRenderService _renderService;

        public AccountController(IUserService userService, IViewRenderService renderService)
        {
            _userService = userService;
            _renderService = renderService;
        }

        #endregion

        #region Register

        [Route("Register")]
        public IActionResult Register() => View();

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", errorMessage: "این نام کاربری توسط شخص دیگری استفاده شده است");
                return View(register);
            }

            if (_userService.IsExistEmail(register.Email))
            {
                ModelState.AddModelError("Email", errorMessage: "این ایمیل قبلا ثبت نام کرده است");
                return View(register);
            }

            User user = new User()
            {
                ActiveCode = NameGenerator.GenerateName(),
                Email = FixedText.FixedEmail(register.Email),
                IsActive = false,
                Password = PasswordHelper.EncodePasswordMD5(register.Password),
                RegisterDate = DateTime.Now,
                UserName = register.UserName,
                UserAvatar = "Defualt.png",
            };

            _userService.AddUser(user);

            #region Send Activation Email

            string body = _renderService.RenderToStringAsync("_ActiveEmail", user);
            SendEmail.Send(user.Email, "فعالسازی حساب", body);

            #endregion


            return View("SuccessRegister", user);
        }


        #endregion

        #region Login


        [Route("Login")]
        public IActionResult Login(bool EditProfile = false)
        {
            ViewBag.EditProfile = EditProfile;
            return View();
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel login, string ReturnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.LoginUser(login);

            if (user != null)
            {

                // Login User
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                    new Claim(ClaimTypes.Name,user.UserName),
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var principal = new ClaimsPrincipal(identity);

                var properties = new AuthenticationProperties()
                {
                    IsPersistent = login.RememberMe
                };

                HttpContext.SignInAsync(principal, properties);

                // End Login User

                if (user.IsActive)
                {
                    ViewBag.IsSuccess = true;

                    if (ReturnUrl != "/")
                        return Redirect(ReturnUrl);

                    return View();
                }
                else
                {
                    ModelState.AddModelError("Email", errorMessage: "حساب کاربری شما فعال نمی باشد.");
                }
            }
            ModelState.AddModelError("Email", errorMessage: "کاربری با مشخصات وارد شده یافت نشد.");
            return View(login);
        }

        #endregion

        #region Active Account

        public IActionResult ActiveAccount(string id)
        {
            ViewBag.IsActive = _userService.ActiveAccount(id);
            return View();
        }

        #endregion

        #region Forgot Password

        [Route("ForgotPassword")]
        public IActionResult ForgotPassword() => View();

        [Route("ForgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgot)
        {
            if (!ModelState.IsValid)
            {
                return View(forgot);
            }

            User user = _userService.GetUserByEmail(forgot.Email);

            if (user != null)
            {
                ViewBag.IsSuccess = true;

                #region Send Email

                string body = _renderService.RenderToStringAsync("_ForgotPassword", user);
                SendEmail.Send(user.Email, "بازیابی رمز عبور", body);

                return View();

                #endregion
            }
            else
            {
                ModelState.AddModelError("Email", errorMessage: "کاربری با این مشخصات یافت نشد");
                return View(forgot);
            }
            return View();
        }

        #endregion

        #region Reset Password

        public IActionResult ResetPassword(string id)
        {
            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel reset)
        {
            if (!ModelState.IsValid)
            {
                return View(reset);
            }

            DataLayer.Entities.User.User user = _userService.GetUserByActiveCode(reset.ActiveCode);

            if (user == null)
            {
                return NotFound();
            }

            string HashNewPassword = PasswordHelper.EncodePasswordMD5(reset.Password);

            user.Password = HashNewPassword;
            _userService.UpdateUser(user);

            return Redirect("/Login");
        }
        #endregion

        #region Logout

        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion

    }
}
