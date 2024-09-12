using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin
{
    [PermissionChecker(1)]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
