using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.DataLayer.Entities.Question
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Display(Name = "پاسخ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string AnswerBody { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public bool IsTrue { get; set; }

        //navigation properties
        public User.User User { get; set; }
        public Question Question { get; set; }
    }
}
