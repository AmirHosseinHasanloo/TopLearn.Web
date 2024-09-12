using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Forum;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Question;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IQuestionService
    {
        FilterQuestionVM GetAllQuestions(int courseId, int pageId = 1, string filter = "");
        int AddQuestion(Question question);
        int AddAnswer(Answer answer);
        ShowQuestionVM ShowQuestion(int questionId);
        bool IsUserCanChooseTrueAnswer(string userName, int questionId);
        bool IsQuestionClosed(int questionId);
        void ChooseTrueAnswer(int answerId, string userName, int questionId);
    }
}
