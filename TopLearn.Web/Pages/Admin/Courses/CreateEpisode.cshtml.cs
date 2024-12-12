using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.Courses
{
    [PermissionChecker(22)]
    [RequestSizeLimit(524288000)]
    public class CreateEpisodeModel : PageModel
    {
        ICourseService _courseService;

        public CreateEpisodeModel(ICourseService courseService)
        {
            _courseService = courseService;
        }


        [BindProperty]
        public CourseEpisode courseEpisode { get; set; }
        public void OnGet(int id)
        {
            courseEpisode = new CourseEpisode();
            courseEpisode.CourseId = id;
        }

        public IActionResult OnPost(IFormFile VideoUP)
        {
            if (!ModelState.IsValid || VideoUP == null)
                return Page();

            if (!_courseService.IsExistsEpisode(VideoUP.FileName))
            {
                // This service do : insert episode to database and save episode video file to directory
                ViewData["IsExistsFile"] = true;
                _courseService.AddCourseEpisode(courseEpisode, VideoUP);
            }
            else
            {
                ViewData["IsExistsFile"] = "false";
                return Page();
            }

            return Redirect("/Admin/Courses/IndexEpisode/" + courseEpisode.CourseId);
        }
    }
}
