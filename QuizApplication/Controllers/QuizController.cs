using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizApplication.Models;
using System.Data.Entity;
using System.Net;

namespace QuizApplication.Controllers
{
    public class QuizController : Controller
    {
        QuizDBEntities db = new QuizDBEntities();

        // GET: Quiz Start View
        [HttpGet]
        public ActionResult QuizStart()
        {
            List<Quiz> quizList = db.Quiz.ToList();

            return View(quizList);
        }

        // GET: New Quiz Start View
        [HttpGet]
        public ActionResult NewQuizStart(int id)
        {
            // Create unique model instance
            QuizTestViewModel modelList = new QuizTestViewModel();
            modelList.Question = new List<QuizTestQuestionViewModel>();

            // Create a list which is filtered by Quiz Id
            List<Question> questionList = db.Question.Include(q => q.Answer).Where(x => x.QuizId == id).ToList();

            for (int i = 0; i < questionList.Count; i++)
            {
                // Mapping the data from database
                QuizTestQuestionViewModel model = new QuizTestQuestionViewModel();
                model.QuestionId = questionList[i].Id;
                model.QuestionText = questionList[i].Question1;
                model.Point = questionList[i].Point;
                model.QuizId = id;
                model.UserId = Convert.ToInt32(Session["UserId"].ToString());
                model.Date = DateTime.Now;
                model.AnswerList = questionList[i].Answer.Select(x => new Answer { Id = x.Id, Answer1 = x.Answer1, QuestionId = x.QuestionId }).ToList();
                model.QuestionType = questionList[i].QuestionType.QuestionType1;
                modelList.Question.Add(model);
            }
            return View(modelList);
        }

        // POST: New Quiz Start View
        [HttpPost]
        public ActionResult NewQuizStart(QuizTestViewModel quiz)
        {
            // Create a list what is used for counting the total score per User per Quiz
            List<Question> questionList = db.Question.Include(q => q.Answer).AsEnumerable().Where(x => x.QuizId == quiz.Question[0].QuizId).ToList();
            
            var quizId = quiz.Question.FirstOrDefault().QuizId;
            var quizName = db.Quiz.Where(x => x.Id == quizId).Select(x => x.Name).FirstOrDefault();

            var userId = quiz.Question.FirstOrDefault().UserId;
            var userNickName = db.User.Where(x => x.Id == userId).Select(x => x.NickName).FirstOrDefault();

            // Create unique model instance
            CompletedQuiz completedQuiz = new CompletedQuiz();

            // Create database list to save User's answers
            var dbList = db.FinishedQuizByUser.ToList();
            int totalScore = 0;

            for (int i = 0; i < quiz.Question.Count; i++)
            {
                completedQuiz = new CompletedQuiz
                {
                    UserId = userId,
                    UserNickName = userNickName,
                    QuizId = quizId,
                    QuizName = quizName,
                    QuestionId = quiz.Question[i].QuestionId,
                    Date = quiz.Question[i].Date,
                    
                };

                if (quiz.Question[i].QuestionType == 1)
                {
                    if (quiz.Question[i].SelectedAnswer == questionList[i].Answer.Where(x => x.AnswerCheck == true).FirstOrDefault().Id)
                    {
                        totalScore += questionList[i].Point;
                        completedQuiz.Score = questionList[i].Point;
                    }
                }
                if (quiz.Question[i].QuestionType == 2)
                {
                    int trueAnswerCounterInDatabase = 0;
                    int correctAnswerCounterFromUser = 0;

                    foreach (var item in questionList[i].Answer)
                    {
                        if (item.AnswerCheck == true)
                        {
                            trueAnswerCounterInDatabase++;
                        }
                    }

                    for (int j = 0; j < quiz.Question[i].AnswerList.Count; j++)
                    {

                        if (quiz.Question[i].AnswerList[j].AnswerCheck == questionList[i].Answer.ElementAt(j).AnswerCheck && questionList[i].Answer.ElementAt(j).AnswerCheck == true)
                        {
                            correctAnswerCounterFromUser++;
                            
                            if (correctAnswerCounterFromUser == trueAnswerCounterInDatabase)
                            {
                                completedQuiz.Score = questionList[i].Point;
                                totalScore += questionList[i].Point;
                                break;
                            }
                        }
                    }
                }

                FinishedQuizByUser result = new FinishedQuizByUser();
                result.UserId = completedQuiz.UserId;
                result.QuizId = completedQuiz.QuizId;
                result.QuestionId = completedQuiz.QuestionId;
                result.Score = completedQuiz.Score;
                result.Date = completedQuiz.Date;

                var finishedQuiz = db.FinishedQuizByUser?.Where(q => q.QuestionId == completedQuiz.QuestionId && q.UserId == completedQuiz.UserId)?.FirstOrDefault();
                if (finishedQuiz == null)
                {
                    db.FinishedQuizByUser.Add(result);
                    db.SaveChanges();
                }
                else
                {
                    finishedQuiz.QuestionId = completedQuiz.QuestionId;
                    finishedQuiz.Score = completedQuiz.Score;
                    finishedQuiz.Date = completedQuiz.Date;
                    db.Entry(finishedQuiz).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            completedQuiz.TotalScore = totalScore;
            
            return View("FinishedQuizView", completedQuiz);
        }

        // GET
        public ActionResult ArchiveView(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Quiz quiz = db.Quiz.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }

        // POST
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveView([Bind(Include = "Id,Name,Archived")] Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("QuizStart");
            }
            return View(quiz);
        }

        // GET
        public ActionResult QuizListView()
        {
            QuizListViewModel quizList = new QuizListViewModel();
            quizList.QuizList = new List<Quiz>();
            var quiz = db.Quiz.ToList();
            for (int i = 0; i < quiz.Count; i++)
            {
                quizList.QuizList.Add(new Quiz()
                {
                    Id = quiz[i].Id,
                    Name = quiz[i].Name,
                    Archived = quiz[i].Archived
                });
            }
            
            return View(quizList);
        }

        // GET
        [HttpPost]
        public ActionResult QuizListView(QuizListViewModel quiz)
        {

            if (ModelState.IsValid)
            {
                foreach (var item in quiz.QuizList)
                {
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index", "Home");
            }

            return View(quiz);
        }
        
        // GET
        public ActionResult FinishedQuizListByUserView(int? id)
        {
            var finishedQuizByUserList = db.FinishedQuizByUser.Include(u => u.Quiz).Include(u => u.User).Where(x => x.QuizId == id).GroupBy(x => x.User.NickName).ToList();
            
            List<FinishedQuizByUser> forScoreCounting = new List<FinishedQuizByUser>();
            List<UserFinishedQuizViewModel> result = new List<UserFinishedQuizViewModel>();


            for (int i = 0; i < finishedQuizByUserList.Count; i++)
            {
                FinishedQuizByUser finishedQuizByUser = new FinishedQuizByUser();

                UserFinishedQuizViewModel userFinishedQuizViewModel = new UserFinishedQuizViewModel();

                forScoreCounting = finishedQuizByUserList[i].ToList();
                int score = 0;

                userFinishedQuizViewModel.QuestionName = new List<string>();
                userFinishedQuizViewModel.ScorePerQuestion = new List<int>();

                for (int j = 0; j < forScoreCounting.Count; j++)
                {
                    int scorePerQuestion = forScoreCounting[j].Score;
                    string questionName = forScoreCounting[j].Question.Question1;
                    score += forScoreCounting[j].Score;
                    userFinishedQuizViewModel.ScorePerQuestion.Add(scorePerQuestion);
                    userFinishedQuizViewModel.QuestionName.Add(questionName);
                }

                userFinishedQuizViewModel.Id = finishedQuizByUserList[i].FirstOrDefault().Id;
                userFinishedQuizViewModel.QuizName = finishedQuizByUserList[i].FirstOrDefault().Quiz.Name;
                userFinishedQuizViewModel.NickName = finishedQuizByUserList[i].FirstOrDefault().User.NickName;
                userFinishedQuizViewModel.Date = finishedQuizByUserList[i].FirstOrDefault().Date;
                userFinishedQuizViewModel.TotalScore = score;
                
                result.Add(userFinishedQuizViewModel);
            }
            return View(result);
        }
    }
}