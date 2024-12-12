using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(25)]
    public class AcceptCommentModel : PageModel
    {
        ICourseService _courseService;

        public AcceptCommentModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public void OnGet(int id)
        {
            _courseService.AcceptComment(id);
        }
    }
}
