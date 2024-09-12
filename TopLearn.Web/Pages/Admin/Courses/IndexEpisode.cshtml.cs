using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(20)]
    public class IndexEpisodeModel : PageModel
    {
        ICourseService _courseService;

        public IndexEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [BindProperty]
        public List<CourseEpisode> courseEpisode { get; set; }

        public void OnGet(int id, string filterEpisode = "")
        {
            ViewData["CourseTitle"] = _courseService.GetCourseNamebyId(id);
            ViewData["CourseId"] = id;
            courseEpisode = _courseService.GetCourseEpisodesByCourseId(courseId: id, filterEpisode);
        }
    }
}
