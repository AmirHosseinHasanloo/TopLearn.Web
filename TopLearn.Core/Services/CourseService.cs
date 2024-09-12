using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using TopLearn.Core.Generators;
using TopLearn.Core.DTOs.Course;
using TopLearn.Core.Convertors;
using TopLearn.Core.Security;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace TopLearn.Core.Services
{
    public class CourseService : ICourseService
    {
        private TopLearnContext _context;

        public CourseService(TopLearnContext context)
        {
            _context = context;
        }



        #region Get

        public string GetCourseNamebyId(int courseId)
        {
            return GetCourseByCourseId(courseId).CourseTitle;
        }
        public List<CourseGroup> GetAllCourseGroups()
        {
            return _context.CourseGroup.ToList();
        }

        public Course GetCourseByCourseId(int courseId)
        {
            return _context.Course.Find(courseId);
        }
        public List<SelectListItem> GetGroupsForManageCoursePanel()
        {
            return _context.CourseGroup.
                Where(g => g.ParentId == null)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                })
                .ToList();
        }

        public List<SelectListItem> GetSubGroupsForManageCoursePanel(int groupId)
        {
            return _context.CourseGroup.
                Where(g => g.ParentId == groupId)
                .Select(g => new SelectListItem()
                {
                    Text = g.GroupTitle,
                    Value = g.GroupId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetTeachers()
        {
            return _context.UserRoles
                .Include(r => r.User)
                .Where(r => r.RoleId == 2)
                .Select(r => new SelectListItem()
                {
                    Text = r.User.UserName,
                    Value = r.UserId.ToString(),
                }).ToList();
        }

        public List<SelectListItem> GetCourseLevels()
        {
            return _context.CourseLevel
                .Select(c => new SelectListItem()
                {
                    Text = c.LevelTitle,
                    Value = c.LevelId.ToString()
                }).ToList();
        }

        public List<SelectListItem> GetStatuses()
        {
            return _context.CourseStatus
                .Select(s => new SelectListItem()
                {
                    Text = s.StatusTitle,
                    Value = s.StatusId.ToString()
                }).ToList();
        }

        #endregion

        public int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "course_no_image.png";

            if (imgCourse != null && imgCourse.IsImage())
            {
                // Check the image and Resize
                course.CourseImageName = SaveCourseFiles(imgCourse, path: "CourseImages", subPath: "Images");
            }
            //TODO : Upload demo

            if (courseDemo != null)
            {
                course.DemoFileName = SaveCourseFiles(courseDemo, path: "CourseImages", subPath: "demos");
            }

            _context.Course.Add(course);
            _context.SaveChanges();

            // return added course id
            return course.CourseId;
        }

        public CourseGroup GetCourseGroupById(int groupId)
        {
            return _context.CourseGroup.Find(groupId);
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
        {
            course.UpdateDate = DateTime.Now;


            if (imgCourse != null && imgCourse.IsImage())
            {
                if (course.CourseImageName != "course_no_image.png")
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot/CourseImages/Images", course.CourseImageName);

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    string thumbPath = Path.Combine(Directory.GetCurrentDirectory()
             , "wwwroot/CourseImages/Thumb", course.CourseImageName);

                    if (File.Exists(thumbPath))
                    {
                        File.Delete(thumbPath);
                    }
                }

                // Check the image and Resize
                course.CourseImageName = SaveCourseFiles(imgCourse, path: "CourseImages", subPath: "Images");
            }
            // Upload demo

            if (courseDemo != null)
            {
                if (course.DemoFileName != null)
                {
                    string filePath = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot/CourseImages/demos", course.DemoFileName);

                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                // Save Demo
                course.DemoFileName = SaveCourseFiles(courseDemo, path: "CourseImages", subPath: "demos");
            }

            _context.Course.Update(course);
            _context.SaveChanges();
        }

        public string SaveCourseFiles(IFormFile file, string path, string subPath, string? episodeName = "")
        {
            string fileName = GenerateFileNameWithPath(file);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot/" + path + "/" + subPath, fileName);


            if (path == "CourseEpisodes")
            {
                string episodePath = Path.Combine(Directory.GetCurrentDirectory(),
              "wwwroot/" + path + "/" + subPath, episodeName);

                using (var stream = new FileStream(episodePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            if (path != "CourseEpisodes")
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            if (subPath == "Images")
            {
                // Resize Image and make thumbnail and save it
                SaveThumbnailFile(filePath, fileName);
            }

            return fileName;
        }

        public string GenerateFileNameWithPath(IFormFile file)
        {
            return NameGenerator.GenerateName() + Path.GetExtension(file.FileName);
        }

        public ShowAdminPanelCourseViewModel GetCoursesForAdminPanel(int pageId = 1, string courseName = "")
        {
            IQueryable<Course> result = _context.Course;

            if (!string.IsNullOrEmpty(courseName))
            {
                result = result.Where(c => c.CourseTitle.Contains(courseName));
            }

            //show in page

            int take = 12;
            int skip = (pageId - 1) * take;

            ShowAdminPanelCourseViewModel courseViewModel = new ShowAdminPanelCourseViewModel();

            courseViewModel.CourseCount = result.Count() / take;
            courseViewModel.CurrentPage = pageId;
            courseViewModel.Course = result.OrderBy(c => c.CreateDate).Skip(skip).Take(take)
                .Select(c => new AdminPanelCourseViewModel()
                {
                    CourseTitle = c.CourseTitle,
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    EpisodeCount = c.CourseEpisodes.Count()
                }).ToList();

            return courseViewModel;
        }

        public void SaveThumbnailFile(string imagePath, string fileName)
        {
            ImageConvertor imgResizer = new ImageConvertor();

            string ThumbPath = Path.Combine(Directory.GetCurrentDirectory(),
            "wwwroot/CourseImages/Thumb", fileName);

            // 400 is good quality dont change it !

            imgResizer.Image_resize(imagePath, ThumbPath, 400);
        }


        #region Course

        public Tuple<List<ShowCourseListItemViewModel>, int> ShowCourse(int pageId = 1, string filter = "",
            string getType = "all", string orderByType = "date", int startPrice = 0,
            int endPrice = 0, List<int> selectedGroups = null, int take = 0)
        {
            if (take == 0)
                take = 8;


            IQueryable<Course> result = _context.Course;

            if (!string.IsNullOrEmpty(filter))
            {
                result = result.Where(c => c.CourseTitle.Contains(filter) || c.Tags.Contains(filter));
            }

            switch (getType)
            {
                case "all":
                    {
                        break;
                    }

                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice != 0);
                        break;
                    }

                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }
            }

            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }

                case "updatedate":
                    {
                        result = result.OrderByDescending(c => c.UpdateDate);
                        break;
                    }
            }

            if (startPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > startPrice);
            }

            if (endPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < endPrice);
            }

            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (int groupId in selectedGroups)
                {
                    result = result.Where(c => c.GroupId == groupId || c.SubGroupId == groupId);
                }
            }

            int skip = (pageId - 1) * take;

            int pageCount = result.Include(c => c.CourseEpisodes)
                .AsNoTracking().AsEnumerable()
                .Select(c => new ShowCourseListItemViewModel()
                {
                    CourseId = c.CourseId,
                    CourseTitle = c.CourseTitle,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    TotalTime = new TimeSpan((c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks) == 0)
                    ? 0 : c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
                }).Count() / take;

            var query = result.Include(c => c.CourseEpisodes)
                .AsNoTracking().AsEnumerable()
                .Select(c => new ShowCourseListItemViewModel()
                {
                    CourseId = c.CourseId,
                    CourseTitle = c.CourseTitle,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    TotalTime = new TimeSpan((c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks) == 0)
                    ? 0 : c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
                }).Skip(skip).Take(take).ToList();


            return Tuple.Create(query, pageCount);
        }

        public void AcceptComment(int commentId)
        {
            _context.CourseComment.IgnoreQueryFilters()
               .First(c => c.CommentId == commentId).IsAdminRead = true;

            _context.SaveChanges();
        }
        public void RejectComment(int commentId)
        {
            var comment = _context.CourseComment.IgnoreQueryFilters()
                .First(c => c.CommentId == commentId);
            _context.CourseComment.Remove(comment);
            _context.SaveChanges();
        }

        #endregion


        #region Course Episode

        public int AddCourseEpisode(CourseEpisode courseEpisode, IFormFile episodeFile)
        {
            SaveCourseFiles(episodeFile
                , path: "CourseEpisodes", subPath: "EpisodeVideo", episodeName: episodeFile.FileName);

            courseEpisode.EpisodeFileName = episodeFile.FileName;
            _context.Add(courseEpisode);
            _context.SaveChanges();

            return courseEpisode.EpisodeId;
        }

        public List<CourseEpisode> GetCourseEpisodesByCourseId(int courseId, string episodeName = "")
        {
            IQueryable<CourseEpisode> episodes = _context.CourseEpisode
                .Where(e => e.CourseId == courseId);

            if (!string.IsNullOrEmpty(episodeName))
            {
                episodes = episodes.Where(e => e.EpisodeTitle.Contains(episodeName));
            }

            return episodes.ToList();
        }
        public bool IsExistsEpisode(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(),
               "wwwroot/CourseEpisodes/EpisodeVideo", fileName);

            return File.Exists(filePath);
        }

        public CourseEpisode GetEpisodeById(int episodeId)
        {
            return _context.CourseEpisode.Find(episodeId);
        }

        public void EditCourseEpisode(CourseEpisode courseEpisode, IFormFile episodeFile)
        {
            if (episodeFile != null)
            {
                string DeletePath = Path.Combine(Directory.GetCurrentDirectory(),
               "wwwroot/CourseEpisodes/EpisodeVideo", courseEpisode.EpisodeFileName);

                if (File.Exists(DeletePath))
                {
                    File.Delete(DeletePath);
                }

                courseEpisode.EpisodeFileName = episodeFile.FileName;

                string filePath = Path.Combine(Directory.GetCurrentDirectory(),
           "wwwroot/CourseEpisodes/EpisodeVideo", episodeFile.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    episodeFile.CopyTo(stream);
                }
            }
            _context.CourseEpisode.Update(courseEpisode);
            _context.SaveChanges();
        }

        public Course GetCourseForShowInSite(int courseId)
        {
            return _context.Course
                 .Include(c => c.User)
                 .Include(c => c.CourseStatus)
                 .Include(c => c.CourseLevel)
                 .Include(c => c.CourseEpisodes)
                 .Include(c => c.CourseGroup)
                 .Include(c => c.UserCourses)
                 .FirstOrDefault(c => c.CourseId == courseId);
        }

        public void AddCourseComment(CourseComment courseComment, string UserName)
        {
            courseComment.CreateDate = DateTime.Now;
            courseComment.IsDeleted = false;
            courseComment.IsAdminRead = false;
            courseComment.UserId = _context.Users.Single(u => u.UserName == UserName).UserId;

            _context.CourseComment.Add(courseComment);
            _context.SaveChanges();
        }

        public Tuple<List<CourseComment>, int> GetAllCommentsForCourse(int courseId, int pageId = 1)
        {
            int take = 26;
            int skip = (pageId - 1) * take;
            int PageCount = _context.CourseComment.Where(c => c.CourseId == courseId).Count() / take;

            if ((PageCount % 2) != 0)
            {
                PageCount += 1;
            }

            return Tuple.Create(_context.CourseComment.Include(c => c.User).Where(c => c.CourseId == courseId)
                .Skip(skip).Take(take).OrderByDescending(c => c.CreateDate).ToList(), PageCount);
        }

        public List<ShowCourseListItemViewModel> GetPopularCourse()
        {
            return _context.Course.Include(c => c.OrderDetails)
                .Include(c => c.CourseEpisodes)
                .Where(od => od.OrderDetails.Any()).
                 OrderByDescending(od => od.OrderDetails.Count)
                 .AsNoTracking().AsEnumerable().
                 Select(c => new ShowCourseListItemViewModel()
                 {
                     CourseId = c.CourseId,
                     CourseTitle = c.CourseTitle,
                     ImageName = c.CourseImageName,
                     Price = c.CoursePrice,
                     TotalTime = new TimeSpan((c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks) == 0)
                    ? 0 : c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
                 }).Take(8).ToList();
        }
        public List<CourseComment> GetCourseCommentsForAdminPanel(int courseId)
        {
            return _context.CourseComment
                .Include(c => c.User).IgnoreQueryFilters()
                .Where(c => c.CourseId == courseId && !c.IsAdminRead).ToList();
        }

        #endregion

        #region Course Group
        public void AddCourseGroup(CourseGroup courseGroup)
        {
            _context.Add(courseGroup);
            _context.SaveChanges();
        }
        public void UpdateCourseGroup(CourseGroup courseGroup)
        {
            _context.CourseGroup.Update(courseGroup);
            _context.SaveChanges();
        }

        public CourseGroup GetDeletedCourseGroupById(int groupId)
        {
            return _context.CourseGroup.IgnoreQueryFilters().Single(cg => cg.GroupId == groupId);
        }

        public List<CourseGroup> GetAllDeletedCourseGroups()
        {
            return _context.CourseGroup.Where(cg => cg.IsDeleted).IgnoreQueryFilters().ToList();
        }

        public void DeleteCourseGroup(int groupId)
        {
            var group = GetCourseGroupById(groupId);

            group.IsDeleted = true;
            _context.Update(group);
            _context.SaveChanges();
        }

        public void RecoveryCourseGroup(int groupId)
        {
            var group = GetDeletedCourseGroupById(groupId);

            group.IsDeleted = false;
            _context.Update(group);
            _context.SaveChanges();
        }
        #endregion

        #region Course Vote

        public void AddVote(int courseId, int userId, bool vote)
        {
            var userVote = _context.CourseVote.FirstOrDefault(v => v.UserId == userId && v.CourseId == courseId);

            if (userVote != null)
            {
                userVote.Vote = vote;
            }
            else
            {
                userVote = new CourseVote
                {
                    CourseId = courseId,
                    UserId = userId,
                    Vote = vote,
                };
                _context.Add(userVote);
            }
            _context.SaveChanges();
        }

        public Tuple<int, int> GetCourseVotes(int courseId)
        {
            // first tuple ==> true votes
            // seconde tuple ==> false votes

            var votes = _context.CourseVote.Where(v => v.CourseId == courseId).ToList();

            return Tuple.Create(votes.Count(v => v.Vote), votes.Count(v => !v.Vote));
        }

        public bool CourseIsFree(int courseId)
        {
            var price = _context.Course
                .FirstOrDefault(c => c.CourseId == courseId).CoursePrice;

            if (price == 0)
                return true;

            return false;
        }

        public List<Course> GetMastersCourse(string userName)
        {
            int userId = _context.Users.SingleOrDefault(u => u.UserName == userName).UserId;

            return _context.Course.Include(c => c.CourseStatus)
                .Include(c => c.CourseEpisodes).
                Where(c => c.TeacherId == userId).ToList();
        }

        public List<CourseEpisode> GetEpisodesByCourseId(int courseId)
        {
            return _context.CourseEpisode.Include(c => c.Course)
                .Where(ce => ce.CourseId == courseId).ToList();
        }

        #endregion
    }
}
