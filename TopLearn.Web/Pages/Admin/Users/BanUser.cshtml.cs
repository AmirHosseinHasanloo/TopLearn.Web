using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(5)]
    public class BanUserModel : PageModel
    {
        private IUserPanelService _userPanelService;

        public BanUserModel(IUserPanelService userPanelService)
        {
            this._userPanelService = userPanelService;
        }

        [BindProperty]
        public UserInformationViewModel UserInfo { get; set; }

        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            UserInfo = _userPanelService.GetUserInformation(id);
        }

        public IActionResult OnPost(int userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Ban User
            _userPanelService.BanUser(userId);

            return RedirectToPage("Index");
        }
    }
}
