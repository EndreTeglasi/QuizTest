using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuizApplication.Models;
using System.IO;

namespace QuizApplication.Controllers
{
    public class AvatarController : Controller
    {
        private QuizDBEntities db = new QuizDBEntities();

        // GET: Avatar Index View
        public async Task<ActionResult> Index()
        {
            List<AvatarViewModel> avatarViewModel = new List<AvatarViewModel>();
            await db.Avatar.ForEachAsync(x => avatarViewModel.Add(new AvatarViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                SourceString = ConvertAvatarToSourceString(x.Data)
            }
            ));

            return View(avatarViewModel);
        }

        // GET: Avatar Details View
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avatar avatar = db.Avatar.Find(id);
            
            AvatarViewModel avatarViewModel = new AvatarViewModel();

            if (avatar == null)
            {
                return HttpNotFound();
            }
            else
            {
                avatarViewModel.Id = avatar.Id;
                avatarViewModel.Name = avatar.Name;
                avatarViewModel.SourceString = ConvertAvatarToSourceString(avatar.Data);
            }
            return View(avatarViewModel);
        }

        // GET: Avatar Create View
        public ActionResult Create()
        {
            return View();
        }

        // POST: Avatar Create View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DataInHttpPostedFileBase")] AvatarViewModel avatarViewModel)
        {
            if (ModelState.IsValid)
            {
                Avatar avatar = new Avatar()
                {
                    Name = avatarViewModel.Name,
                    Data = ConvertHttpPostedFileBaseToByteArray(avatarViewModel.DataInHttpPostedFileBase)
                };
                db.Avatar.Add(avatar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(avatarViewModel);
        }

        // GET: Avatar Edit View
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avatar avatar = db.Avatar.Find(id);

            AvatarViewModel avatarViewModel = new AvatarViewModel();

            if (avatar == null)
            {
                return HttpNotFound();
            }
            else
            {
                avatarViewModel.Id = avatar.Id;
                avatarViewModel.Name = avatar.Name;
            }
            return View(avatarViewModel);
        }

        // POST: Avatar Edit View
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DataInHttpPostedFileBase")] AvatarViewModel avatarViewModel)
        {
            if (ModelState.IsValid)
            {
                Avatar avatar = new Avatar();

                if (avatarViewModel.DataInHttpPostedFileBase != null)
                {
                    avatar.Data = ConvertHttpPostedFileBaseToByteArray(avatarViewModel.DataInHttpPostedFileBase);
                }

                avatar.Name = avatarViewModel.Name;

                db.Entry(avatar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(avatarViewModel);
        }

        // GET: Avatar Delete View
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Avatar avatar = db.Avatar.Find(id);

            AvatarViewModel avatarViewModel = new AvatarViewModel();    

            if (avatar == null)
            {
                return HttpNotFound();
            }
            else
            {
                avatarViewModel.Id = avatar.Id;
                avatarViewModel.Name = avatar.Name;
                avatarViewModel.SourceString = ConvertAvatarToSourceString(avatar.Data);
            }

            return View(avatarViewModel);
        }

        // POST: Avatar Delete View
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Avatar avatar = db.Avatar.Find(id);
            db.Avatar.Remove(avatar);
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

        /// <summary>
        /// Converts the image (byte[] data) into a string base64 format
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string ConvertAvatarToSourceString(byte[] data)
        {
            return String.Format($"data:image/jpg;base64,{Convert.ToBase64String(data)}");
        }
        /// <summary>
        /// Converts back the InputStream of a HttpPostedFileBase to a byte array
        /// </summary>
        /// <param name="fileBase"></param>
        /// <returns></returns>
        private byte[] ConvertHttpPostedFileBaseToByteArray(HttpPostedFileBase fileBase)
        {
            byte[] returnData;

            using (MemoryStream memoryStreamImage = new MemoryStream())
            {
                fileBase.InputStream.CopyTo(memoryStreamImage);
                returnData = memoryStreamImage.GetBuffer();
            }

            return returnData;
        }
    }
}
