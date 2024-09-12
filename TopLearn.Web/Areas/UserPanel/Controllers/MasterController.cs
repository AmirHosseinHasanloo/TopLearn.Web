using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    [PermissionChecker(17)]
    public class MasterController : Controller
    {
        #region Ctor

        private ICourseService _courseService;
        IUserPanelService _userPanelService;

        public MasterController(ICourseService courseService, IUserPanelService userPanelService)
        {
            _courseService = courseService;
            _userPanelService = userPanelService;
        }

        #endregion

        [HttpGet("master-Courses")]
        public IActionResult MasterCoursesList()
        {
            var Courses = _courseService.GetMastersCourse(User.Identity.Name);

            return View(Courses);
        }

        [HttpGet("episodesList/{courseId}")]
        public IActionResult EpisodesList(int courseId)
        {
            Course course = _courseService.GetCourseByCourseId(courseId);

            if (course == null)
            {
                return NotFound();
            }

            int userId = _userPanelService.GetUserIdByUserName(User.Identity.Name);

            if (course.TeacherId != userId)
            {
                return RedirectToAction("MasterCoursesList", "Master");
            }

            var episode = _courseService.GetEpisodesByCourseId(courseId);

            ViewBag.CourseId = courseId;
            return View(episode);
        }


        [HttpGet("add-Episode/{courseId}")]
        public IActionResult AddEpisode(int courseId)
        {
            var course = _courseService.GetCourseByCourseId(courseId);

            if (course == null)
            {
                return NotFound();
            }

            int userId = _userPanelService.GetUserIdByUserName(User.Identity.Name);

            if (course.TeacherId != userId)
            {
                return RedirectToAction("MasterCoursesList", "Master");
            }

            return View(new addEpisodeViewModel()
            {
                CourseId = course.CourseId,
                IsFree = true
            });
        }

        [HttpPost("add-Episode/{viewModel}")]
        public IActionResult AddEpisode(addEpisodeViewModel viewModel)
        {
            return View();
        }

        public IActionResult DropzoneTarget(List<IFormFile> files, int courseId)
        {
            return new JsonResult("Success");
        }
    }
}
