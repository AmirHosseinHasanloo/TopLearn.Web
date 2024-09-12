using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.ViewComponents
{
    public class LatestCoursesViewComponent : ViewComponent
    {
        private ICourseService _courseService;

        public LatestCoursesViewComponent(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("/Views/Shared/Components/LatestCoursesComponent/LatestCourses.cshtml", _courseService.ShowCourse()));
        }
    }
}
