using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private IUserPanelService _userPanel;

        public WalletController(IUserPanelService userPanel)
        {
            _userPanel = userPanel;
        }

        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ViewBag.WalletBalanceHistory = _userPanel.GetWalletBalanceHistoryOfUser(User.Identity.Name);
            return View();
        }

        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.WalletBalanceHistory = _userPanel.GetWalletBalanceHistoryOfUser(User.Identity.Name);
                return View(charge);
            }
            int WalletId = _userPanel.ChargeWallet(User.Identity.Name, charge.Amount, "شارژ کیف پول");


            #region Online Payment

            var Payment = new ZarinpalSandbox.Payment(charge.Amount);

            var response = Payment.PaymentRequest("شارژ کیف پول", "https://localhost:4001/OnlinePayment/" + WalletId,"amirhosseinhasanloo2005@gmail.com","09915995484");

            if (response.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + response.Result.Authority);
            }

            #endregion
            return null;
        }
    }
}
