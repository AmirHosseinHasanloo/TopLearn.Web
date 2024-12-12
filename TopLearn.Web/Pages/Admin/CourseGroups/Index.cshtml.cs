using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    [PermissionChecker(27)]
    public class IndexModel : PageModel
    {
        public ICourseService _CourseService;

        public IndexModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        [BindProperty]
        public List<CourseGroup> CourseGroup { get; set; }

        public void OnGet()
        {
            CourseGroup = _CourseService.GetAllCourseGroups();
        }
    }
}
