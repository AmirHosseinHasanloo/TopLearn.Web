using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Web.Pages.Admin.Roles
{
    // permission Id => 6 is Role Manager
    [PermissionChecker(6)]
    public class IndexModel : PageModel
    {
        private IPermissionServices permissionService;

        public IndexModel(IPermissionServices permissionService)
        {
            this.permissionService = permissionService;
        }

        [BindProperty]
        public List<Role> Roles { get; set; }

        public void OnGet()
        {
            Roles = permissionService.GetRoles();
        }
    }
}
