using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.Services.Interfaces;

namespace TopLearn.Web.Controllers
{
    public class ShowSubGroupController : Controller
    {
        private ICourseService _courseService;

        public ShowSubGroupController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public JsonResult Index(int id)
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "انتخاب کنید", Value = "" },
            };

            list.AddRange(_courseService.GetSubGroupsForManageCoursePanel(id));
            // return json 
            return Json(new SelectList(list.ToList(), "Value","Text"));
        }
    }
}
