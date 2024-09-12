using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(3)]
    public class CreateUserModel : PageModel
    {
        private IUserPanelService _userPanelService;
        private IPermissionServices _permissionService;

        public CreateUserModel(IUserPanelService userPanelService, IPermissionServices permissionService)
        {
            _userPanelService = userPanelService;
            _permissionService = permissionService;
        }

        [BindProperty]
        public CreateUserViewModel CreateUser { get; set; }
        public void OnGet()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
                return Page();

            int UserId = _userPanelService.AddUserFromAdminPanel(CreateUser);

            //Add Roles
            _permissionService.AddRolesForUser(SelectedRoles, UserId);

            return Redirect("/Admin/Users");
        }
    }
}
