using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    [PermissionChecker(24)]
    public class CreateModel : PageModel
    {
        ICourseService _CourseService;

        public CreateModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        [BindProperty]
        public CourseGroup CourseGroup { get; set; }

        public void OnGet(int id)
        {
            if (id != 0)
            {
                CourseGroup = new CourseGroup()
                {
                    ParentId = id
                };
            }
        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            //TODO : Add Course Group To Db

            _CourseService.AddCourseGroup(CourseGroup);

            return RedirectToPage("Index");
        }
    }
}
