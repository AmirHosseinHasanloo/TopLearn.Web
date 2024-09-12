using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(7)]
    public class CreateRoleModel : PageModel
    {

        private IPermissionServices _permissionService;


        public CreateRoleModel(IPermissionServices permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public Role Role { get; set; }
        public void OnGet()
        {
            ViewData["Permissions"] = _permissionService.GetAllPermissions();
        }

        public IActionResult OnPost(List<int> selectedPermission)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // add role

            Role.IsDelete = false;
            int roleId = _permissionService.AddRole(role: Role);

            // Add Role Permission

            _permissionService.AddPermissionToRole(roleId, selectedPermission);

            return RedirectToPage("Index");
        }
    }
}
