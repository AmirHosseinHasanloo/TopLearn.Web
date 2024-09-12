using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearn.Core.DTOs.Course
{
    public class AdminPanelCourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string ImageName { get; set; }
        public int EpisodeCount { get; set; }
    }
    public class ShowAdminPanelCourseViewModel
    {
        public List<AdminPanelCourseViewModel> Course { get; set; }
        public int CourseCount { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
    }
}
