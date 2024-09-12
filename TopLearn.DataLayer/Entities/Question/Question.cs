using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Course;

namespace TopLearn.DataLayer.Entities.Question
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Display(Name = "عنوان پرسش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(120, ErrorMessage = "فیلد {0} نمی تواند بیش از {1} کرکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "پرسش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string QuestionBody { get; set; }

        [Display(Name = "تاریخ پرسش")]
        [Required]
        public DateTime CreateDate { get; set; }

        [Display(Name = "تاریخ بروزرسانی")]
        public DateTime ModifiedDate { get; set; }

        //navigation properties
        public Course.Course Course { get; set; }
        public User.User User { get; set; }
        public ICollection<Answer> Answers { get; set; }
    }
}
