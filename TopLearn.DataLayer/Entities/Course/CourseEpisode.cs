using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TopLearn.DataLayer.Entities.Course
{
    public class CourseEpisode
    {
        [Key]
        public int EpisodeId { get; set; }

        [Display(Name = "دوره")]
        public int CourseId { get; set; }

        [Display(Name = "عنوان بخش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کاراکتر باشد.")]
        public string EpisodeTitle { get; set; }

        [Display(Name = "زمان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan EpisodeTime { get; set; }

        [Display(Name = "فایل")]
        public string EpisodeFileName { get; set; }

        [Display(Name = "رایگان")]
        public bool IsFree { get; set; }


        #region Relation

        public Course Course { get; set; }

        #endregion

    }
}
