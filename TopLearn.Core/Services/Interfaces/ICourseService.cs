using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopLearn.Core.DTOs.Course;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.Core.Services.Interfaces
{
    public interface ICourseService
    {
        #region Get

        List<CourseGroup> GetAllCourseGroups();
        Course GetCourseByCourseId(int courseId);
        CourseGroup GetCourseGroupById(int groupId);
        string GetCourseNamebyId(int courseId);
        List<SelectListItem> GetGroupsForManageCoursePanel();
        List<SelectListItem> GetSubGroupsForManageCoursePanel(int groupId);
        List<SelectListItem> GetTeachers();
        List<SelectListItem> GetStatuses();
        List<SelectListItem> GetCourseLevels();
        List<ShowCourseListItemViewModel> GetPopularCourse();
        List<CourseComment> GetCourseCommentsForAdminPanel(int courseId);

        #endregion

        #region Course

        int AddCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);

        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);
        string SaveCourseFiles(IFormFile file, string path, string subPath, string? episodeName = "");
        void SaveThumbnailFile(string imagePath, string fileName);
        string GenerateFileNameWithPath(IFormFile file);
        ShowAdminPanelCourseViewModel GetCoursesForAdminPanel(int pageId = 1, string courseName = "");
        Tuple<List<ShowCourseListItemViewModel>, int> ShowCourse(int pageId = 1, string filter = "", string getType = "all",
            string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null, int take = 0);

        Course GetCourseForShowInSite(int courseId);
        void AddCourseComment(CourseComment courseComment, string UserName);
        Tuple<List<CourseComment>, int> GetAllCommentsForCourse(int courseId, int pageId = 1);

        void AcceptComment(int commentId);
        void RejectComment(int commentId);

        List<Course> GetMastersCourse(string userName);
        List<CourseEpisode> GetEpisodesByCourseId(int courseId);
        #endregion

        #region CourseEpisode
        int AddCourseEpisode(CourseEpisode courseEpisode, IFormFile episodeFile);
        void EditCourseEpisode(CourseEpisode courseEpisode, IFormFile episodeFile);
        List<CourseEpisode> GetCourseEpisodesByCourseId(int courseId, string episodeName = "");
        CourseEpisode GetEpisodeById(int episodeId);
        bool IsExistsEpisode(string fileName);

        #endregion

        #region Course Group
        void AddCourseGroup(CourseGroup courseGroup);
        void UpdateCourseGroup(CourseGroup courseGroup);
        CourseGroup GetDeletedCourseGroupById(int groupId);
        List<CourseGroup> GetAllDeletedCourseGroups();
        void DeleteCourseGroup(int groupId);
        void RecoveryCourseGroup(int groupId);
        #endregion

        #region Course Vote

        void AddVote(int courseId, int userId, bool vote);
        Tuple<int, int> GetCourseVotes(int courseId);
        bool CourseIsFree(int courseId);

        #endregion
    }
}
