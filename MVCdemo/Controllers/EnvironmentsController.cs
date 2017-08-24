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
    public class EnvironmentsController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Environments
        public ActionResult Index(int? app_id, string env)
        {// for maintaining state when redirect from create method

            try
            {
                Session["appid"] = app_id;
                Session["env"] = env;
                if (env == null || app_id == null)
                {
                    ViewBag.IsfromApplication = false;
                    //ViewBag.appid = app_id;
                    //ViewBag.AppName = db.Applications.Where(m => m.AppId == app_id).Select(app => app.AppName).First<string>();
                    //return View(db.Environments.ToList());
                    return RedirectToAction("Index","Applications");
                }
                else
                {
                    string[] envtype = { "Dev", "ITG", "Prod" };
                    if(!envtype.Contains(env))
                        return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable,"Something wrong with parameter");

                    if (db.Applications.Any(a => a.AppId == app_id) == false)
                            return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    ViewBag.appid = app_id;
                    ViewBag.AppName = ((db.Applications.ToList().Where(m => m.AppId == app_id).Select(e => e.AppName))).First<string>();
                    ViewBag.IsfromApplication = true;
                    return View(db.Environments.Where(e => e.AppId == app_id & e.EnvType == env));
                }

            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            } 
           
           
        }
        // GET: Environments/Details/5
        public ActionResult Details(int? id, int? app_id, string env)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Environment environment = db.Environments.Find(id);
            if (environment == null)
            {
                return HttpNotFound();
            }
            return View(environment);
        }

        // GET: Environments/Create
        public ActionResult Create(bool? Redirected)
        {
            if (Redirected != null)
            {
                ViewBag.Redirected = Redirected;
                if (TempData["filepath"] != null)
                {
                    ViewBag.Filepath = TempData["filepath"].ToString();

                }
            }
            else
            {
                ViewBag.Redirected = Redirected;

            }
            ViewBag.EnvTypeDL = getEnvType();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName");
            return View();
        }

        // POST: Environments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EnvID,EnvType,AppId")] Models.Environment environment)
        {
            if (ModelState.IsValid)
            {
                if (Session["Filepath"] != null)
                {

                    using (EnvironmentDBContext context = new EnvironmentDBContext())
                    {
                        environment.ArchDiagramPath = Session["filepath"].ToString();
                        environment.DBServerParameter = environment.EnvType;
                        environment.LinksParameter = environment.EnvType;
                        environment.ServerParameter = environment.EnvType;
                        environment.FileShareParameter = environment.EnvType;

                    }
                }

                db.Environments.Add(environment);
                db.SaveChanges();
                
                return RedirectToAction("Index", new { app_id = Convert.ToUInt32(Session["appid"].ToString()), env =  Session["env"].ToString()  });
            }
            ViewBag.EnvTypeDL = getEnvType();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", environment.AppId);
            return View(environment);
        }

        // GET: Environments/Edit/5
        public ActionResult Edit(int? id, bool? Redirected)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Environment environment = db.Environments.Find(id);
            if (environment == null)
            {
                return HttpNotFound();
            }
            if (Redirected != null)
            {
                ViewBag.Redirected = Redirected;
                if (TempData["filepath"] != null)
                {
                    ViewBag.Filepath = TempData["filepath"].ToString();

                }
            }
            else
            {
                ViewBag.Redirected = Redirected;

            }
            ViewBag.Editid = id;
            ViewBag.EnvTypeDL = getEnvType();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", environment.AppId);
            return View(environment);
        }

        // POST: Environments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EnvID,ArchDiagramPath,EnvType,AppId")] Models.Environment environment)
        {
            if (ModelState.IsValid)
            {
                if (Session["Filepath"] != null)
                {

                    using (EnvironmentDBContext context = new EnvironmentDBContext())
                    {
                        environment.ArchDiagramPath = Session["filepath"].ToString();
                        environment.DBServerParameter = environment.EnvType;
                        environment.LinksParameter = environment.EnvType;
                        environment.ServerParameter = environment.EnvType;
                        environment.FileShareParameter = environment.EnvType;
                    }
                }
                db.Entry(environment).State = EntityState.Modified;
                db.SaveChanges();
                return     RedirectToAction("Index", new { app_id = Convert.ToUInt32(Session["appid"].ToString()), env = Session["env"].ToString() }); 
            }
            ViewBag.EnvTypeDL = getEnvType();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", environment.AppId);
            return View(environment);
        }

        // GET: Environments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Environment environment = db.Environments.Find(id);
            if (environment == null)
            {
                return HttpNotFound();
            }
            return View(environment);
        }

        // POST: Environments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Models.Environment environment = db.Environments.Find(id);
            db.Environments.Remove(environment);
            db.SaveChanges();
            return RedirectToAction("Index", new { app_id = Convert.ToUInt32(Session["appid"].ToString()), env = Session["env"].ToString() });
        }
        public List<SelectListItem> getEnvType()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Dev", Value = "Dev" });
            items.Add(new SelectListItem { Text = "ITG", Value = "ITG" });
            items.Add(new SelectListItem { Text = "Prod", Value = "Prod" });
            return items;
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
