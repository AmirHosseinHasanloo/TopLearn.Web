using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserPanelService _panelService;
        private IUserService _userService;
        private ICourseService _courseService;
        public HomeController(IUserPanelService panelService, IUserService userService, ICourseService courseService)
        {
            _panelService = panelService;
            _userService = userService;
            _courseService = courseService;
        }
        public IActionResult Index()
        {
            ViewBag.PopularCourse = _courseService.GetPopularCourse();

            return View(_courseService.ShowCourse());
        }


        [Route("UserPanel/EditProfile")]
        [Authorize]
        public IActionResult EditProfile()
        {
            return View(_panelService.GetDataForUserProfileEdit(User.Identity.Name));
        }

        [HttpPost]
        [Route("UserPanel/EditProfile")]
        public IActionResult EditProfile(EditProfileViewModel edit)
        {
            if (!ModelState.IsValid)
            {
                return View(edit);
            }

            _panelService.EditProfile(User.Identity.Name, edit);

            //Logout User 
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Login?EditProfile=true");
        }

        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("UserPanel/ChangePassword")]
        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel change)
        {
            string CurrentUserName = User.Identity.Name;
            if (!ModelState.IsValid)
            {
                return View(change);
            }

            if (!_panelService.CompareOldPassword(CurrentUserName, change.OldPassword))
            {
                ModelState.AddModelError("OldPassword", errorMessage: "کلمه عبور فعلی صحیح نمی باشد");
                return View(change);
            }

            _panelService.ChangeUserPassword(CurrentUserName, change.Password);
            ViewBag.IsSuccess = true;
            return View();
        }


        [Route("OnlinePayment/{id}")]
        // int id ==> WalletId
        public IActionResult OnlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != ""
                && HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != ""
                )
            {

                string Authority = HttpContext.Request.Query["Authority"];

                var Wallet = _userService.GetWalletByWalletId(id);

                var Payment = new ZarinpalSandbox.Payment(Wallet.Amount);
                var Response = Payment.Verification(Authority).Result;

                if (Response.Status == 100)
                {
                    ViewBag.Code = Response.RefId;
                    ViewBag.IsSuccess = true;
                    Wallet.IsPayed = true;
                    _userService.UpdateWallet(Wallet);
                }
            }
            return View();
        }

        public IActionResult Error404()
        {
            return View();
        }
    }
}
