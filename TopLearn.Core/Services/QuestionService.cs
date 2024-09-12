using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TopLearn.Core.DTOs.Forum;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Context;
using TopLearn.DataLayer.Entities.Question;

namespace TopLearn.Core.Services
{
    public class QuestionService : IQuestionService
    {
        private TopLearnContext _context;

        public QuestionService(TopLearnContext context)
        {
            _context = context;
        }

        public int AddAnswer(Answer answer)
        {
            _context.Answer.Add(answer);
            _context.SaveChanges();

            return answer.AnswerId;
        }

        public int AddQuestion(Question question)
        {
            question.CreateDate = DateTime.Now;
            question.ModifiedDate = DateTime.Now;

            _context.Question.Add(question);
            _context.SaveChanges();

            return question.QuestionId;
        }

        public void ChooseTrueAnswer(int answerId, string userName, int questionId)
        {
            if (!IsQuestionClosed(questionId))
            {
                if (IsUserCanChooseTrueAnswer(userName, questionId))
                {
                    var answer = _context.Answer.Single(a => a.AnswerId == answerId);
                    answer.IsTrue = true;

                    _context.Update(answer);
                    _context.SaveChanges();
                }
            }
        }

        public FilterQuestionVM GetAllQuestions(int courseId, int pageId = 1, string filter = "")
        {
            IQueryable<Question> results = _context.Question
                .Include(u=>u.User).Include(c=>c.Course).Where(q => q.CourseId == courseId);

            if (!string.IsNullOrEmpty(filter))
            {
                results = results.Where(q => q.Title.Contains(filter));
            }
            FilterQuestionVM VM = new FilterQuestionVM();
            //paging sys =>
            int take = 20;
            int skip = (pageId - 1) * take;


            VM.PageCount = results.Count() / take;
            VM.CurrentPage = pageId;
            VM.questions = results.OrderBy(q => q.CreateDate).Skip(skip).Take(take).ToList();

            return VM;
        }

        public bool IsQuestionClosed(int questionId)
        {
            if (_context.Question.Any(q => q.QuestionId == questionId && q.Answers
            .Any(a => a.IsTrue && a.QuestionId == questionId)))
            {
                return true;
            }

            return false;
        }

        public bool IsUserCanChooseTrueAnswer(string userName, int questionId)
        {
            if (!IsQuestionClosed(questionId))
            {
                int userId = _context.Users.Single(u => u.UserName == userName).UserId;

                if (_context.Question.Any(q => q.UserId == userId && q.QuestionId == questionId))
                {
                    return true;
                }
            }

            return false;
        }

        public ShowQuestionVM ShowQuestion(int questionId)
        {
            var question = _context.Question.Include(q => q.User)
                .FirstOrDefault(q => q.QuestionId == questionId);

            ShowQuestionVM viewModel = new ShowQuestionVM()
            {
                Question = question,
                Answers = _context.Answer
                .Where(a => a.QuestionId == question.QuestionId)
                .OrderBy(a => a.CreateDate).ToList(),
            };

            return viewModel;
        }
    }
}
