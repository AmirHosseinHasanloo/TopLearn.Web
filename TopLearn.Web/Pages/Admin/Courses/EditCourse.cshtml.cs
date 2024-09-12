using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(19)]
    [RequestSizeLimit(524288000)]
    public class EditCourseModel : PageModel
    {
        private ICourseService _courseService;

        public EditCourseModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public Course Course { get; set; }
        public void OnGet(int id)
        {
            Course = _courseService.GetCourseByCourseId(id);

            var groups = _courseService.GetGroupsForManageCoursePanel();
            ViewData["Groups"] = new SelectList(groups, "Value", "Text", Course.GroupId);

            var subGroups = _courseService
                .GetSubGroupsForManageCoursePanel(Course.GroupId);
            ViewData["SubGroups"] = new SelectList(subGroups, "Value", "Text", Course.SubGroupId ?? 0);

            var teacher = _courseService.GetTeachers();
            ViewData["Teachers"] = new SelectList(teacher, "Value", "Text", Course.TeacherId);

            var courseLevels = _courseService.GetCourseLevels();
            ViewData["CourseLevels"] = new SelectList(courseLevels, "Value", "Text", Course.LevelId);

            var courseStatuses = _courseService.GetStatuses();
            ViewData["CourseStatus"] = new SelectList(courseStatuses, "Value", "Text", Course.StatusId);
        }

        public IActionResult OnPost(IFormFile ImageUP, IFormFile demoUP)
        {
            if (!ModelState.IsValid)
                return Page();


            // Edit Course

            _courseService.UpdateCourse(Course, ImageUP, demoUP);

            return RedirectToPage("Index");
        }

    }
}
