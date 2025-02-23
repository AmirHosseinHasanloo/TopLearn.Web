﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TopLearn.Core.DTOs.Course
{
    public class ShowCourseListItemViewModel
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string ImageName { get; set; }
        public int Price { get; set; }
        public TimeSpan TotalTime { get; set; }
    }
}
