using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;

namespace TopLearn.Web.Pages.Admin.Users
{
    [PermissionChecker(4)]
    public class EditUserModel : PageModel
    {
        private IUserPanelService _panelService;
        private IPermissionServices _permissionService; private TopLearnContext _context;

        public EditUserModel(IUserPanelService panelService, IPermissionServices permissionService, TopLearnContext context)
        {
            _panelService = panelService;
            _permissionService = permissionService;
            _context = context;
        }


        [BindProperty]
        public EditUserViewModel EditUser { get; set; }
        public void OnGet(int id)
        {
            ViewData["Role"] = _permissionService.GetRoles();
            EditUser = _panelService.GetUserForShowInEditMode(id);
        }

        public IActionResult OnPost(List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
                return Page();

            // Edit User
            _panelService.EditUserFromAdminPanel(EditUser);

            // Edit User Roles
            _permissionService.EditRolesForUser(SelectedRoles, EditUser.UserId);

            return RedirectToPage("Index");
        }
    }
}
