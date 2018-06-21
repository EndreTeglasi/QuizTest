using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using QuizApplication.Models;
using System.Web.SessionState;

namespace QuizApplication.Controllers
{
    public class UserAccountController : Controller
    {
        QuizDBEntities db = new QuizDBEntities();

        // GET: UserAccount
        public ActionResult Register()
        {
            UserViewModel newUser = new UserViewModel();
            Random r = new Random();
            var newAvatarId = r.Next(0, (db.Avatar.Count()));

            newUser.AvatarId = newAvatarId;
            newUser.AvatarImage = db.Avatar.Find(newAvatarId).Data;

            return View(newUser);
        }

        // POST: UserAccount
        [HttpPost]
        public ActionResult Register(UserViewModel user)
        {
            User userIntoDatabase = new User();

            try
            {
                if (ModelState.IsValid)
                {
                    userIntoDatabase.NickName = user.NickName;
                    userIntoDatabase.Password = user.PlainPassword;
                    userIntoDatabase.Email = user.Email;
                    userIntoDatabase.UserTypeId = 2;
                    userIntoDatabase.AvatarId = user.AvatarId;
                }
                db.User.Add(userIntoDatabase);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message.ToString());
            }
            return View(user);
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var details = (from userList in db.User
                                   where userList.Email == model.Email && userList.Password == model.Password
                                   select new
                                   {
                                       userList.Id,
                                       userList.NickName,
                                       userList.UserTypeId
                                   }).ToList();
                    
                    if (details.FirstOrDefault() != null)
                    {
                        Session["UserId"] = details.FirstOrDefault().Id;
                        Session["NickName"] = details.FirstOrDefault().NickName;
                        Session["UserTypeId"] = details.FirstOrDefault().UserTypeId;
                        Session["IsLoggedIn"] = "true";

                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nincs ilyen felhasználó!");
                }
            }
            catch (Exception exception)
            {
                ModelState.AddModelError("", exception.Message.ToString());
            }
            
            return View(model);
        }

        // GET: /Account/Logout
        [AllowAnonymous]
        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}