using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(17)]
    public class IndexModel : PageModel
    {
        private ICourseService _courseService;

        public IndexModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public ShowAdminPanelCourseViewModel CourseList { get; set; }

        public void OnGet(int pageId = 1, string FilterCourseName = "")
        {
            CourseList = _courseService.GetCoursesForAdminPanel(pageId, FilterCourseName);
        }
    }
}
