using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(11)]
    public class RecoveryRoleModel : PageModel
    {
        private IPermissionServices _permissionService;

        public RecoveryRoleModel(IPermissionServices permissionService)
        {
            _permissionService = permissionService;
        }


        [BindProperty]
        public Role Role { get; set; }

        public void OnGet(int id)
        {
            Role = _permissionService.GetDeletedRoleById(id);
        }

        public IActionResult OnPost()
        {
            // Delete Role
            _permissionService.RecoveryRole(Role.RoleId);

            return RedirectToPage("Index");
        }
    }
}
