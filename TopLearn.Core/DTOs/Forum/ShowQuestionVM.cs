using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.DataLayer.Entities.Question;

namespace TopLearn.Core.DTOs.Forum
{
    public class ShowQuestionVM
    {
        public DataLayer.Entities.Question.Question Question { get; set; }
        public List<Answer> Answers { get; set; }
    }

    public class FilterQuestionVM
    {
        public List<Question> questions { get; set; }
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
    }
}
