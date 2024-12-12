using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    [PermissionChecker(29)]
    public class EditModel : PageModel
    {
        private ICourseService _CourseService;

        public EditModel(ICourseService CourseService)
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
            if (!ModelState.IsValid)
                return Page();

            _CourseService.UpdateCourseGroup(CourseGroup);

            return RedirectToPage("Index");
        }
    }
}
