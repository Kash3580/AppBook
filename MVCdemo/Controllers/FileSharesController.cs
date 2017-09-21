using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCdemo.Models;

namespace MVCdemo.Controllers
{
    public class FileSharesController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();
        // GET: FileShares
        public ActionResult  Index(int? app_id, string env)
        {
            try
            {
                Session["appid"] = app_id;
                Session["env"] = env;
                if (env == null || app_id == null)
                {
                    return View(db.FileShare.ToList());
                }
                else
                {
                    string[] envtype = { "Dev", "ITG", "Prod" };
                    if (!envtype.Contains(env))
                        return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, "Something wrong with parameter");

                    if (db.Applications.Any(a => a.AppId == app_id) == false)
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    ViewBag.appid = app_id;
                    ViewBag.AppName = db.Applications.Where(m => m.AppId == app_id).Select(app => app.AppName).First<string>();

                    var server = db.FileShare.Include(s => s.Application);
                    return View(server.Where(m => m.AppId == app_id & m.EnvType == env).ToList());
                }
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //var fileShare = db.FileShare.Include(f => f.Application);
            //return View(fileShare.ToList());
        }

        // GET: FileShares/Details/5
        public ActionResult Details(int? id, int? app_id, string env)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileShare fileShare = db.FileShare.Find(id);
            if (fileShare == null)
            {
                return HttpNotFound();
            }
            return View(fileShare);
        }
        // generate env list
        public List<SelectListItem> getServerEnv()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Dev", Value = "Dev" });
            items.Add(new SelectListItem { Text = "ITG", Value = "ITG" });
            items.Add(new SelectListItem { Text = "Prod", Value = "Prod" });
            return items;
        }
        // GET: FileShares/Create
        public ActionResult Create()
        {
            ViewBag.ServerEnv = getServerEnv();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName");
            return View();
        }

        // POST: FileShares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FileShareId,FileSharePath,FileShareType,UserName,Password,PasswordExpiryDate,EnvType, AppId")] FileShare fileShare)
        {
            if (ModelState.IsValid)
            {
                using (FileShareDBContext context = new FileShareDBContext())
                {
                    fileShare.CreatedDate = DateTime.Now;
                    fileShare.ModifiedDate = DateTime.Now;
                }
                db.FileShare.Add(fileShare);
                db.SaveChanges();
               
                if (Session["appid"] == null || Session["env"] == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", new { app_id = Convert.ToInt32(Session["appid"].ToString()), env = Session["env"].ToString() });


                }
            }
            ViewBag.ServerEnv = getServerEnv();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", fileShare.AppId);
            return View(fileShare);
        }

        // GET: FileShares/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileShare fileShare = db.FileShare.Find(id);
            if (fileShare == null)
            {
                return HttpNotFound();
            }

            ViewBag.ServerEnv = getServerEnv();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", fileShare.AppId);
            return View(fileShare);
        }

        // POST: FileShares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FileShareId,FileSharePath,FileShareType,UserName,Password,PasswordExpiryDate,EnvType,AppId")] FileShare fileShare)
        {
            if (ModelState.IsValid)
            {
                using (FileShareDBContext context = new FileShareDBContext())
                {
                    fileShare.CreatedDate = DateTime.Now;
                    fileShare.ModifiedDate = DateTime.Now;
                }
                db.Entry(fileShare).State = EntityState.Modified;
                db.SaveChanges();
                if (Session["appid"] == null || Session["env"] == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", new { app_id = Convert.ToInt32(Session["appid"].ToString()), env = Session["env"].ToString() });


                }
            }
            ViewBag.ServerEnv = getServerEnv();

            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", fileShare.AppId);
            return View(fileShare);
        }

        // GET: FileShares/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FileShare fileShare = db.FileShare.Find(id);
            if (fileShare == null)
            {
                return HttpNotFound();
            }
            return View(fileShare);
        }

        // POST: FileShares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FileShare fileShare = db.FileShare.Find(id);
            db.FileShare.Remove(fileShare);
            db.SaveChanges();
            if (Session["appid"] == null || Session["env"] == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", new { app_id = Convert.ToInt32(Session["appid"].ToString()), env = Session["env"].ToString() });


            }
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
