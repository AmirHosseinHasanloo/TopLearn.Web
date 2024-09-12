using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    [PermissionChecker(27)]
    public class RecoveryModel : PageModel
    {
        private ICourseService _CourseService;

        public RecoveryModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        [BindProperty]
        public CourseGroup CourseGroup { get; set; }

        public void OnGet(int id)
        {
            CourseGroup = _CourseService.GetDeletedCourseGroupById(id);
        }

        public IActionResult OnPost()
        {
            _CourseService.RecoveryCourseGroup(CourseGroup.GroupId);

            return RedirectToPage("Index");
        }
    }
}
