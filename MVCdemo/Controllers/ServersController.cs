using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCdemo.Models;
using System.Diagnostics;

namespace MVCdemo.Controllers
{
    public class ServersController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Servers
        public ActionResult Index(int? app_id, string env)
        {
            try
            {
                Session["appid"] = app_id;
                Session["env"] = env;
                if (env == null || app_id == null)
                {
                    return View(db.Server.ToList());
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

                    var server = db.Server.Include(s => s.Application);
                     return View(server.Where(m => m.AppId == app_id & m.ServerEnv == env).ToList());
                }
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Servers/Details/5
        public ActionResult Details(int? id, int? app_id, string env)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Server.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // GET: Servers/Create
        public ActionResult Create()
        {
            ViewBag.ServerEnv = getServerEnv();
            ViewBag.ServeTypeDL = getServerTypeDetails();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName");
            return View();
        }

        // POST: Servers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ServerID,ServerName,ServerType,ServerVersion,UserName,Password, ServerEnv,AppId")] Server server)
        {
            if (ModelState.IsValid)
            {
                using (ServerDbContext context = new ServerDbContext())
                {
                    server.CreatedDate = DateTime.Now;
                    server.ModifiedDate = DateTime.Now;
                }
                db.Server.Add(server);
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
            ViewBag.ServeTypeDL = getServerTypeDetails();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", server.AppId);
            return View(server);

        }

        // GET: Servers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Server.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServerEnv = getServerEnv();
            ViewBag.ServeTypeDL = getServerTypeDetails();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", server.AppId);
            return View(server);
        }

        // POST: Servers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ServerID,ServerName,ServerType,ServerVersion,UserName,Password, CreatedDate,ServerEnv,AppId")] Server server)
        {
            if (ModelState.IsValid)
            {
                using (ServerDbContext context = new ServerDbContext())
                {
                    server.CreatedDate = server.CreatedDate;
                    server.ModifiedDate = DateTime.Now;
                }
                db.Entry(server).State = EntityState.Modified;
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
            ViewBag.ServeTypeDL = getServerTypeDetails();
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", server.AppId);
            return View(server);
        }

        // GET: Servers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Server server = db.Server.Find(id);
            if (server == null)
            {
                return HttpNotFound();
            }
            return View(server);
        }

        // POST: Servers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Server server = db.Server.Find(id);
            db.Server.Remove(server);
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

        public ActionResult ConnectRDP(string ServerName)
        {
            Process rdcProcess = new Process();

            string executable = System.Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe");
            if (executable != null)
            {

                rdcProcess.StartInfo.FileName = executable;
                rdcProcess.StartInfo.Arguments = "/v " + "UAS-P-GEN-RDS-2" + " /prompt"; ;  // ip or name of computer to connect
                rdcProcess.Start();

            }
            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public List<SelectListItem> getServerEnv()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Dev", Value = "Dev" });
            items.Add(new SelectListItem { Text = "ITG", Value = "ITG" });
            items.Add(new SelectListItem { Text = "Prod", Value = "Prod" });
            return items;
        }

        public List<SelectListItem> getServerTypeDetails()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Windows", Value = "Windows" });
            items.Add(new SelectListItem { Text = "Linux", Value = "Linux" });
            items.Add(new SelectListItem { Text = "Unix", Value = "Prod" });
            return items;
        }
    }
}
