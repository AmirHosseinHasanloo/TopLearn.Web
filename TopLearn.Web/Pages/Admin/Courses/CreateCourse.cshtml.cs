using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(18)]
    [RequestSizeLimit(524288000)]
    public class CreateCourseModel : PageModel
    {
        private ICourseService _courseService;

        public CreateCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }

        public void OnGet()
        {
            var groups = _courseService.GetGroupsForManageCoursePanel();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text");

            var subGroups = _courseService
                .GetSubGroupsForManageCoursePanel(int.Parse(groups.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text");

            var teacher = _courseService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teacher, "Value", "Text");

            var courseLevels = _courseService.GetCourseLevels();
            ViewData["CourseLevels"] = new SelectList(courseLevels, "Value", "Text");

            var courseStatuses = _courseService.GetStatuses();
            ViewData["CourseStatus"] = new SelectList(courseStatuses, "Value", "Text");


        }
        
        public IActionResult OnPost(IFormFile ImageUP, IFormFile demoUP)
        {
            if (!ModelState.IsValid)
                return Page();

            // add course to database and save image to directory
            _courseService.AddCourse(Course, ImageUP, demoUP);

            return RedirectToPage("index");
        }
    }
}
