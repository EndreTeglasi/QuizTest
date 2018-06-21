using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using QuizApplication.Models;

namespace QuizApplication.Controllers
{
    public class UserController : Controller
    {
        private QuizDBEntities db = new QuizDBEntities();

        // GET: User
        public ActionResult Index()
        {
            List<UserViewModel> userViewModelList = new List<UserViewModel>();
            foreach (var item in db.User)
            {
                UserViewModel userViewModel = new UserViewModel();

                userViewModel.Id = item.Id;
                userViewModel.Email = item.Email;
                userViewModel.PlainPassword = item.Password;
                userViewModel.AvatarId = item.AvatarId;
                userViewModel.AvatarImage = item.Avatar.Data;
                userViewModel.UserTypeId = item.UserTypeId;
                userViewModel.UserType = item.UserType;
                userViewModel.NickName = item.NickName;
                var selected = db.FinishedQuizByUser.Where(x => x.UserId == item.Id).ToList();
                if (selected.Count <= 0)
                {
                    userViewModel.NumberOfFinishedQuiz = 0;
                    userViewModel.TotalScore = 0;
                }
                else
                {
                    userViewModel.NumberOfFinishedQuiz = selected.GroupBy(x => x.QuizId).Count();
                    userViewModel.TotalScore = selected.Select(score => score.Score).Sum();
                }

                userViewModelList.Add(userViewModel);
            }

            return View(userViewModelList);
        }

        // GET
        public ActionResult Result(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinishedQuizByUserSearchListViewModel finishedQuizByUserSearchViewModelList = new FinishedQuizByUserSearchListViewModel();
            finishedQuizByUserSearchViewModelList.FinishedQuizByUserSearchViewModel = new List<FinishedQuizByUserSearchViewModel>();

            var query = db.FinishedQuizByUser.Include(u => u.Quiz).Include(u => u.User);
            if (query == null)
            {
                return HttpNotFound();
            }
            else
            {
                var selected = db.FinishedQuizByUser.Where(x => x.UserId == id).ToList();
                foreach (var item in selected)
                {
                    FinishedQuizByUserSearchViewModel finishedQuizByUserSearchViewModel = new FinishedQuizByUserSearchViewModel();

                    finishedQuizByUserSearchViewModel.Id = item.Id;
                    finishedQuizByUserSearchViewModel.NickName = item.User.NickName;
                    finishedQuizByUserSearchViewModel.QuestionId = item.QuestionId;
                    finishedQuizByUserSearchViewModel.QuestionName = item.Question.Question1;
                    finishedQuizByUserSearchViewModel.QuizCategory = item.Quiz.Name;
                    finishedQuizByUserSearchViewModel.QuizId = item.QuizId;
                    finishedQuizByUserSearchViewModel.Score = item.Score;
                    finishedQuizByUserSearchViewModel.UserId = item.UserId;
                    finishedQuizByUserSearchViewModel.Date = item.Date;

                    finishedQuizByUserSearchViewModelList.FinishedQuizByUserSearchViewModel.Add(finishedQuizByUserSearchViewModel);
                }
                finishedQuizByUserSearchViewModelList.QuizCategoryList = selected.Select(x => x.Quiz.Name).Distinct().ToList();
                finishedQuizByUserSearchViewModelList.UserIdForPassing = id;
            }
            return View(finishedQuizByUserSearchViewModelList);
        }

        // POST: User' Result
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Result(FinishedQuizByUserSearchListViewModel model)
        {
            
            var query = db.FinishedQuizByUser.Include(u => u.Quiz).Include(u => u.User).ToList();

            if (model?.QuizCategoryList.Count != 0)
            {
                query = query.Where(log => log.Quiz.Name == model.QuizCategoryList.FirstOrDefault()).ToList();
            }

            FinishedQuizByUserSearchListViewModel resultList = new FinishedQuizByUserSearchListViewModel();

            resultList.FinishedQuizByUserSearchViewModel = query.Select(x => new FinishedQuizByUserSearchViewModel()
            {
                Id = x.Id,
                NickName = x.User.NickName,
                QuestionId = x.QuestionId,
                QuestionName = x.Question.Question1,
                QuizCategory = x.Quiz.Name,
                QuizId = x.QuizId,
                Score = x.Score,
                UserId = x.UserId,
                Date = x.Date
            }).ToList();

            resultList.QuizCategoryList = db.FinishedQuizByUser.Where(x => x.UserId == model.UserIdForPassing).Select(x => x.Quiz.Name).Distinct().ToList();

            return View(resultList);
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);

            UserViewModel userViewModel = new UserViewModel();

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                userViewModel.Id = user.Id;
                userViewModel.Email = user.Email;
                userViewModel.PlainPassword = user.Password;
                userViewModel.AvatarId = user.AvatarId;
                userViewModel.AvatarImage = user.Avatar.Data;
                userViewModel.UserTypeId = user.UserTypeId;
                userViewModel.UserType = user.UserType;
                userViewModel.NickName = user.NickName;
                userViewModel.NumberOfFinishedQuiz = db.FinishedQuizByUser.Where(x => x.UserId == user.Id).GroupBy(x => x.QuizId).Count();
                userViewModel.TotalScore = db.FinishedQuizByUser.Where(item => item.UserId == user.Id).Select(score => score.Score).Sum();
            }
            return View(userViewModel);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.AvatarId = new SelectList(db.Avatar, "Id", "Name");
            ViewBag.UserTypeId = new SelectList(db.UserType, "Id", "Name");
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,PlainPassword,AvatarId,AvatarImage,UserTypeId,UserType,NickName,NumberOfFinishedQuiz,TotalScore")] UserViewModel userViewModel)
        {
            
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Id = userViewModel.Id,
                    Email = userViewModel.Email,
                    Password = userViewModel.PlainPassword,
                    AvatarId = userViewModel.AvatarId,
                    NickName = userViewModel.NickName
                };
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AvatarId = new SelectList(db.Avatar, "Id", "Name", userViewModel.AvatarId);
            ViewBag.UserTypeId = new SelectList(db.UserType, "Id", "Name", userViewModel.UserTypeId);
            return View(userViewModel);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);

            UserViewModel userViewModel = new UserViewModel();

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                userViewModel.Id = user.Id;
                userViewModel.Email = user.Email;
                userViewModel.PlainPassword = user.Password;
                userViewModel.AvatarId = user.AvatarId;
                userViewModel.AvatarImage = user.Avatar.Data;
                userViewModel.UserTypeId = user.UserTypeId;
                userViewModel.UserType = user.UserType;
                userViewModel.NickName = user.NickName;
                userViewModel.NumberOfFinishedQuiz = db.FinishedQuizByUser.Where(item => item.UserId == user.Id).Count();
                userViewModel.TotalScore = db.FinishedQuizByUser.Where(item => item.UserId == user.Id).Select(score => score.Score).Sum();
            }
            ViewBag.AvatarId = new SelectList(db.Avatar, "Id", "Name", userViewModel.AvatarId);
            ViewBag.UserTypeId = new SelectList(db.UserType, "Id", "Name", userViewModel.UserTypeId);
            return View(userViewModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,PlainPassword,AvatarId,AvatarImage,UserTypeId,UserType,NickName,NumberOfFinishedQuiz,TotalScore")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User();

                user.Email = userViewModel.Email;
                user.NickName = userViewModel.NickName;
                user.Password = userViewModel.PlainPassword;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AvatarId = new SelectList(db.Avatar, "Id", "Name", userViewModel.AvatarId);
            ViewBag.UserTypeId = new SelectList(db.UserType, "Id", "Name", userViewModel.UserTypeId);
            return View(userViewModel);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);

            UserViewModel userViewModel = new UserViewModel();

            if (user == null)
            {
                return HttpNotFound();
            }
            else
            {
                userViewModel.Id = user.Id;
                userViewModel.Email = user.Email;
                userViewModel.PlainPassword = user.Password;
                userViewModel.AvatarId = user.AvatarId;
                userViewModel.AvatarImage = user.Avatar.Data;
                userViewModel.UserTypeId = user.UserTypeId;
                userViewModel.UserType = user.UserType;
                userViewModel.NickName = user.NickName;
                userViewModel.NumberOfFinishedQuiz = db.FinishedQuizByUser.Where(item => item.UserId == user.Id).Count();
                userViewModel.TotalScore = db.FinishedQuizByUser.Where(item => item.UserId == user.Id).Select(score => score.Score).Sum();
            }
            return View(userViewModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
