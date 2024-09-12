using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class HomeController : Controller
    {
        private IUserPanelService _panelService;

        public HomeController(IUserPanelService panelService)
        {
            _panelService = panelService;
        }

        public IActionResult Index()
        {
            return View(_panelService.GetUserInformation(User.Identity.Name));
        }
    }
}
