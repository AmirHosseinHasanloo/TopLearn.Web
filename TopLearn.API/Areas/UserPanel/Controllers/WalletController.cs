using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.DTOs;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.API.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private IUserPanelService _userPanel;

        public WalletController(IUserPanelService userPanel)
        {
            _userPanel = userPanel;
        }

        [HttpGet("UserPanel/Wallet")]
        public IActionResult Index()
        {
          var history = _userPanel.GetWalletBalanceHistoryOfUser(User.Identity.Name);
          if (history == null)
          {
              return BadRequest();
          }
          return Ok(history);
        }

        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
            // if (!ModelState.IsValid)
            // {
            //     ViewBag.WalletBalanceHistory = _userPanel.GetWalletBalanceHistoryOfUser(User.Identity.Name);
            //     return View(charge);
            // }

            int WalletId = _userPanel.ChargeWallet(User.Identity.Name, charge.Amount, "شارژ کیف پول");


            #region Online Payment

            var Payment = new ZarinpalSandbox.Payment(charge.Amount);

            var response = Payment.PaymentRequest("شارژ کیف پول", "https://localhost:4001/OnlinePayment/" + WalletId,
                "amirhosseinhasanloo2005@gmail.com", "09915995484");

            if (response.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + response.Result.Authority);
            }

            #endregion

            return null;
        }
    }
}