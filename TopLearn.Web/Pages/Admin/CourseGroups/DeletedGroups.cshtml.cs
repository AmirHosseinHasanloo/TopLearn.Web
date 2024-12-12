using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Pages.Admin.CourseGroups
{
    [PermissionChecker(32)]
    public class DeletedGroupsModel : PageModel
    {
        private ICourseService _CourseService;

        public DeletedGroupsModel(ICourseService CourseService)
        {
            _CourseService = CourseService;
        }

        [BindProperty]
        public List<CourseGroup> Groups { get; set; }

        public void OnGet()
        {
            Groups = _CourseService.GetAllDeletedCourseGroups();
        }
    }
}
