using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(26)]
    public class RejectCommentModel : PageModel
    {
        ICourseService _courseService;

        public RejectCommentModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public void OnGet(int id)
        {
            _courseService.RejectComment(id);
        }
    }
}
