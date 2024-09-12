using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Diagnostics;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(8)]
    public class EditRoleModel : PageModel
    {
        private IPermissionServices _permissionService;

        public EditRoleModel(IPermissionServices permissionService)
        {
            _permissionService = permissionService;
        }


        [BindProperty]
        public Role Role { get; set; }

        public void OnGet(int id)
        {
            ViewData["Permissions"] = _permissionService.GetAllPermissions();
            ViewData["SelectedPermissions"] = _permissionService.GetPermissionsRole(id);
            Role = _permissionService.GetRoleById(id);
        }

        public IActionResult OnPost(List<int> selectedPermission)
        {
            if (!ModelState.IsValid)
                return Page();

            // Update Role
            _permissionService.UpdateRole(role: Role);

            //Update Role Permissions
            _permissionService.UpdateRolePermissions(Role.RoleId,selectedPermission);

            return RedirectToPage("Index");
        }


    }
}
