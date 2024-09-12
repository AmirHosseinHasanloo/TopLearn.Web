using System.Collections.Generic;
using System.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    [PermissionChecker(9)]
    public class DeletedRolesModel : PageModel
    {
        private IPermissionServices _permission;

        public DeletedRolesModel(IPermissionServices permission)
        {
            _permission = permission;
        }

        [BindProperty]
        public List<Role> Roles { get; set; }
        public void OnGet()
        {
            Roles = _permission.GetDeletedRoles();
        }
    }
}
