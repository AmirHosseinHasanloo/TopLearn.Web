using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Convertors;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.API.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Constractor Injection
        private IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        [HttpPost]
        [Route("Register")]
        public ActionResult Register()
        {

            return Ok();
        }
    }
}
