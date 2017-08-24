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
    public class LinksController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Links
        public ActionResult Index(int? app_id,string env)
        {     try
            {
            Session["appid"] = app_id;
            Session["env"] = env;
            if (env == null || app_id == null)
            {
                    return View(db.Links.ToList());
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

                var links = db.Links.Include(l => l.Application);
                return View(links.Where(m => m.AppId == app_id &  m.LinkType == env).ToList());
                }
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Links/Details/5
        public ActionResult Details(int? id, int? app_id, string env)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Links links = db.Links.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }
            return View(links);
        }
        public List<SelectListItem> getLinkType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Dev", Value = "Dev" });
            items.Add(new SelectListItem { Text = "ITG", Value = "ITG" });
            items.Add(new SelectListItem { Text = "Prod", Value = "Prod" });
       return items;
        }
        // GET: Links/Create
        public ActionResult Create()
        {
            ViewBag.LinktypeDL = getLinkType();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName");
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LinkId,LinkType,Link,Username,Password,CreatedDate,ModifiedDate,AppId")] Links links)
        {
            if (ModelState.IsValid)
            {
                using (LinksDBContext context=new LinksDBContext())
                {
                    links.CreatedDate = DateTime.Now;
                    links.ModifiedDate = DateTime.Now;
                }
                    db.Links.Add(links);
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

            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", links.AppId);
            return View(links);
        }

        // GET: Links/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Links links = db.Links.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }
            ViewBag.LinktypeDL = getLinkType();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", links.AppId);
            return View(links);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LinkId,LinkType,Link,Username,Password,CreatedDate,ModifiedDate,AppId")] Links links)
        {
            if (ModelState.IsValid)
            {
                using (LinksDBContext context = new LinksDBContext())
                {
                    links.CreatedDate = links.CreatedDate;
                    links.ModifiedDate = DateTime.Now;
                }
                db.Entry(links).State = EntityState.Modified;
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
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", links.AppId);
            return View(links);
        }

        // GET: Links/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Links links = db.Links.Find(id);
            if (links == null)
            {
                return HttpNotFound();
            }
            return View(links);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Links links = db.Links.Find(id);
            db.Links.Remove(links);
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
