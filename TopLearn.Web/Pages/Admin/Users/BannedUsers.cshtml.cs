using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(2)]
    public class BannedUsersModel : PageModel
    {
        private IUserPanelService _UserService;

        public BannedUsersModel(IUserPanelService UserService)
        {
            _UserService = UserService;
        }

        [BindProperty]
        public UsersForAdminViewModel UsersForAdmin { get; set; }

        public void OnGet(int pageId = 1, string filterUserName = "", string filterEmail = "")
        {
            UsersForAdmin = _UserService.GetBannedUsers(pageId, filterEmail, filterUserName);
        }
    }
}
