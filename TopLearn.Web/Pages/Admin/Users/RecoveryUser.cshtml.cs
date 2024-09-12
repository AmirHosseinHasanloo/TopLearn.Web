using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(6)]
    public class RecoveryUserModel : PageModel
    {
        private IUserPanelService _userPanelService;

        public RecoveryUserModel(IUserPanelService userPanelService)
        {
            this._userPanelService = userPanelService;
        }

        [BindProperty]
        public UserInformationViewModel UserInfo { get; set; }

        public void OnGet(int id)
        {
            ViewData["UserId"] = id;
            UserInfo = _userPanelService.GetBannedUserInformation(id);
        }

        public IActionResult OnPost(int userId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Ban User
            _userPanelService.RecoveryUser(userId);

            return RedirectToPage("Index");
        }
    }
}
