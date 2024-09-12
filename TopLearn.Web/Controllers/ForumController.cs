using Ganss.Xss;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Linq;
using System.Security.Claims;
using TopLearn.Core.DTOs.Forum;
using TopLearn.Core.Services.Interfaces;
using TopLearn.DataLayer.Entities.Question;

namespace TopLearn.Web.Controllers
{
    public class ForumController : Controller
    {
        private IQuestionService _questionService;
        private IUserPanelService _userService;
        private IUserService _UserCourseService;
        private ICourseService _courseService;

        public ForumController(IQuestionService questionService, IUserPanelService userService, ICourseService courseService, IUserService UserCourseService)
        {
            _questionService = questionService;
            _userService = userService;
            _courseService = courseService;
            _UserCourseService = UserCourseService;

        }

        // id => courseId
        [Authorize]
        public IActionResult Index(int id, int pageId = 1, string filter = "")
        {
            FilterQuestionVM questions = _questionService.GetAllQuestions(id, pageId, filter);

            ViewData["CourseTitle"] = questions.questions
                .Where(q => q.CourseId == id).FirstOrDefault().Course.CourseTitle;
            ViewBag.CourseId = id;

            return View(questions);
        }

        #region Create Question

        // id ==> CourseId
        [Authorize]
        public IActionResult CreateQuestion(int id)
        {
            Question question = new Question()
            {
                CourseId = id,
                UserId = _userService.GetUserIdByUserName(User.Identity.Name)
            };

            if (!_UserCourseService.IsUserBuyedThisCourse(id,User.Identity.Name))
            {
                return Redirect("/Forum/Index/" + id);
            }
            return View(question);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreateQuestion(Question question)
        {
            if (!ModelState.IsValid)
            {
                return View(question);
            }

            //Add the question =>
            _questionService.AddQuestion(question);

            return Redirect("/Forum/showQuestion/" + question.QuestionId);
        }
        #endregion


        #region showQuestion
        [Authorize]
        public IActionResult showQuestion(int id)
        {
            return View(_questionService.ShowQuestion(id));
        }

        [HttpPost]
        public IActionResult showQuestion(string answer, int questionId)
        {
            var sanitizer = new HtmlSanitizer();
            answer = sanitizer.Sanitize(answer);
            _questionService.AddAnswer(new Answer()
            {
                AnswerBody = answer,
                UserId = _userService.GetUserByName(User.Identity.Name).UserId,
                CreateDate = DateTime.Now,
                QuestionId = questionId,
            });

            return Redirect("/Forum/showQuestion/" + questionId);
        }
        #endregion

        [Authorize]
        public IActionResult SelectTrueAnswer(int answerId, int questionId)
        {
            _questionService.ChooseTrueAnswer(answerId, User.Identity.Name, questionId);

            return Redirect("/Forum/showQuestion/" + questionId);
        }
    }
}
