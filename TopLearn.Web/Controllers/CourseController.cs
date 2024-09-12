using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SharpCompress.Archives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TopLearn.Core.Security;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;
        private IOrderService _orderService;
        private IUserService _userService;
        private IUserPanelService _userPanelService;
        public CourseController(ICourseService courseService, IOrderService orderService, IUserService userService, IUserPanelService userPanelService)
        {
            _courseService = courseService;
            _orderService = orderService;
            _userService = userService;
            _userPanelService = userPanelService;
        }

        public IActionResult FilterCourse(int pageId = 1, string filter = "",
            string getType = "all", string orderByType = "date", int startPrice = 0,
            int endPrice = 0, List<int> selectedGroups = null, int take = 0)
        {
            ViewBag.Groups = _courseService.GetAllCourseGroups();
            ViewBag.selectedGroup = selectedGroups;
            ViewBag.pageId = pageId;

            return View(_courseService.ShowCourse(pageId, filter, getType, orderByType,
                startPrice, endPrice, selectedGroups, 12));
        }

        [Route("ShowCourse/{id}")]
        public IActionResult ShowCourse(int id, int episode = 0)
        {
            Course course = _courseService.GetCourseForShowInSite(id);

            if (course == null)
            {
                return NotFound();
            }

            if (episode != 0 && User.Identity.IsAuthenticated)
            {
                if (course.CourseEpisodes.All(e => e.EpisodeId != episode))
                {
                    return BadRequest();
                }

                if (!course.CourseEpisodes.First(e => e.EpisodeId == episode).IsFree)
                {
                    if (!_userService.IsUserBuyedThisCourse(id, User.Identity.Name))
                    {
                        return BadRequest();
                    }
                }

                var courseEpisode = course.CourseEpisodes.Single(e => e.EpisodeId == episode);
                ViewBag.Episode = courseEpisode;


                string filePath = "";
                string checkFilePath = "";

                if (courseEpisode.IsFree)
                {
                    filePath = "/OnlineCourse/" + courseEpisode.EpisodeFileName.Replace(".rar", ".mp4");
                    checkFilePath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/OnlineCourse", courseEpisode.EpisodeFileName.Replace(".rar", ".mp4"));
                }
                else
                {
                    filePath = "/CourseFilesOnline/" + courseEpisode.EpisodeFileName.Replace(".rar", ".mp4");
                    checkFilePath = Path.Combine(Directory.GetCurrentDirectory(),
        "wwwroot/CourseFilesOnline", courseEpisode.EpisodeFileName.Replace(".rar", ".mp4"));
                }

                if (!System.IO.File.Exists(checkFilePath))
                {
                    string targetPass = Directory.GetCurrentDirectory();

                    if (courseEpisode.IsFree)
                    {
                        targetPass = System.IO.Path.Combine(targetPass, "wwwroot/OnlineCourse");
                    }
                    else
                    {
                        targetPass = System.IO.Path.Combine(targetPass, "wwwroot/CourseFilesOnline");
                    }


                    var rarPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/CourseEpisodes/EpisodeVideo", courseEpisode.EpisodeFileName);

                    using (var archive = ArchiveFactory.Open(rarPath))
                    {
                        var Entries = archive.Entries.OrderBy(e => e.Key.Length);

                        foreach (var en in Entries)
                        {
                            if (Path.GetExtension(en.Key) == ".mp4")
                            {
                                using (var fileStream = System.IO.File.Create(Path.Combine(targetPass,
                                    courseEpisode.EpisodeFileName.Replace(".rar", ".mp4"))))
                                {
                                    en.WriteTo(fileStream);
                                }
                            }
                        }
                    }

                }
                ViewBag.EpisodeFilePath = filePath;
            }


            if (!_courseService.CourseIsFree(id))
            {
                ViewBag.IsFree = false;
            }

            ViewBag.CourseTime = new TimeSpan((course.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks) == 0) ?
                0 : course.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks));

            return View(course);
        }

        [Authorize]
        public IActionResult BuyCourse(int id)
        {
            int orderId = _orderService.AddOrder(User.Identity.Name, id);

            return Redirect("/UserPanel/ShowOrder/" + orderId);
        }

        [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var episode = _courseService.GetEpisodeById(episodeId);

            string fileName = episode.EpisodeFileName;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/CourseEpisodes/EpisodeVideo", fileName);

            if (episode.IsFree)
            {
                byte[] file = System.IO.File.ReadAllBytes(filePath);
                return File(file, "application/force-download", fileName);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_userService.IsUserBuyedThisCourse(episode.CourseId, User.Identity.Name))
                {
                    byte[] file = System.IO.File.ReadAllBytes(filePath);
                    return File(file, "application/force-download", fileName);
                }
            }

            return Forbid();
        }

        [HttpPost]
        public IActionResult CreateComment(CourseComment comment)
        {
            _courseService.AddCourseComment(comment, User.Identity.Name);
            return PartialView("ShowComments", _courseService.GetAllCommentsForCourse(comment.CourseId));
        }

        public IActionResult ShowComments(int id, int pageId = 1)
        {
            return PartialView(_courseService.GetAllCommentsForCourse(id, pageId));
        }


        public IActionResult CourseVote(int id)
        {
            if (!_courseService.CourseIsFree(id))
            {
                if (!_userService.IsUserBuyedThisCourse(id, User.Identity.Name))
                {
                    ViewBag.NotAccess = true;
                }
            }
            return PartialView(_courseService.GetCourseVotes(id));
        }

        public IActionResult AddVote(int id, bool vote)
        {
            _courseService.AddVote(id, _userPanelService.GetUserIdByUserName(User.Identity.Name), vote);
            return PartialView("CourseVote", _courseService.GetCourseVotes(id));
        }

    }
}
