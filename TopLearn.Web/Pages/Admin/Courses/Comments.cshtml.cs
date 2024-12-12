using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(24)]
    public class CommentsModel : PageModel
    {
        private ICourseService _CourseService;

        public CommentsModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        public List<CourseComment> Comments { get; set; }

        public void OnGet(int id)
        {
            Comments = _CourseService.GetCourseCommentsForAdminPanel(id);
        }
    }
}
