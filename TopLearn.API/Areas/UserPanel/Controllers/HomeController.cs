using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.API.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private IUserPanelService _userService;

        public HomeController(IUserPanelService userService)
        {
            _userService = userService;
        }

        [HttpGet("[action]")]
        public IActionResult GetInformation()
        {
            var userInformation =
                _userService.GetUserInformation(User.Identity.Name);
            if (userInformation != null)
            {
                return Ok(userInformation);
            }

            return StatusCode(StatusCodes
                .Status404NotFound, "کاربر گرامی چنین حسابی وجود ندارد.");
        }
    }
}