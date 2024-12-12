using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    [PermissionChecker(30)]
    public class DeleteModel : PageModel
    {
        private ICourseService _CourseService;

        public DeleteModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        [BindProperty]
        public CourseGroup CourseGroup { get; set; }

        public void OnGet(int id)
        {
            CourseGroup = _CourseService.GetCourseGroupById(id);
        }

        public IActionResult OnPost()
        {
            _CourseService.DeleteCourseGroup(CourseGroup.GroupId);
            return RedirectToPage("Index");
        }
    }
}
