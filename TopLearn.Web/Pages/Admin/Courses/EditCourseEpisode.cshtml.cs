using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(23)]
    [RequestSizeLimit(524288000)]
    public class EditCourseEpisodeModel : PageModel
    {
        ICourseService _courseService;

        public EditCourseEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [BindProperty]
        public CourseEpisode courseEpisode { get; set; }
        public void OnGet(int id)
        {
            courseEpisode = _courseService.GetEpisodeById(id);
        }

        public IActionResult OnPost(IFormFile VideoUP)
        {
            if (!ModelState.IsValid)
                return Page();

            ViewData["IsExistsFile"] = true;
            _courseService.EditCourseEpisode(courseEpisode, VideoUP);

            return Redirect("/Admin/Courses/IndexEpisode/" + courseEpisode.CourseId);
        }
    }
}
