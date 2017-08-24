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
    public class DBServersController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: DBServers
        public ActionResult Index(int? app_id, string env)
        {
            try
            {
                Session["appid"] = app_id;
                Session["env"] = env;
                if (env == null || app_id == null)
                {
                    return View(db.DBServer.ToList());
                }
                else
                {
                    string[] envtype = { "Dev", "ITG", "Prod" };
                    if (!envtype.Contains(env))
                        return new HttpStatusCodeResult(HttpStatusCode.NotAcceptable, "Something wrong with parameter");

                    if (db.Applications.Any(a => a.AppId == app_id) == false)
                        return new HttpStatusCodeResult(HttpStatusCode.NotFound);
                    ViewBag.appid = app_id;
                    ViewBag.AppName = ((db.Applications.ToList().Where(m => m.AppId == app_id).Select(e => e.AppName))).First<string>();
                    ViewBag.IsfromApplication = true;
                    var dBServer = db.DBServer.Include(d => d.Application);
                    return View(dBServer.Where(e => e.AppId == app_id & e.DbserverEnv == env));

                }
            }
            catch (Exception)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: DBServers/Details/5
        public ActionResult Details(int? id, int? app_id, string env)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBServer dBServer = db.DBServer.Find(id);
            if (dBServer == null)
            {
                return HttpNotFound();
            }
            return View(dBServer);
        }

        // GET: DBServers/Create
        public ActionResult Create()
        {
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName");
            return View();
        }

        // POST: DBServers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DbserverID,DbserverName,DbserverVersion,DbuserName,Dbpassword,CreatedDate,ModifiedDate,DbserverSize,DbserverEnv,AppId,AppName")] DBServer dBServer)
        {
            if (ModelState.IsValid)
            {

                using (DBServerDbContext context = new DBServerDbContext())
                {
                    dBServer.CreatedDate = DateTime.Now;
                    dBServer.ModifiedDate = DateTime.Now;
                }
                    db.DBServer.Add(dBServer);
                db.SaveChanges();
                //  return RedirectToAction("Index");
                if (Session["appid"] == null || Session["env"] == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", new { app_id = Convert.ToInt32(Session["appid"].ToString()), env = Session["env"].ToString() });


                }
            }

            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", dBServer.AppId);
            return View(dBServer);
        }

        // GET: DBServers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBServer dBServer = db.DBServer.Find(id);
            if (dBServer == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", dBServer.AppId);
            return View(dBServer);
        }

        // POST: DBServers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DbserverID,DbserverName,DbserverVersion,DbuserName,Dbpassword,CreatedDate,ModifiedDate,DbserverSize,DbserverEnv,AppId,AppName")] DBServer dBServer)
        {
            if (ModelState.IsValid)
            {

                using (DBServerDbContext context = new DBServerDbContext())
                {
                    dBServer.CreatedDate = dBServer.CreatedDate;
                    dBServer.ModifiedDate = DateTime.Now;
                }
                db.Entry(dBServer).State = EntityState.Modified;
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
            ViewBag.AppId = new SelectList(db.Applications, "AppId", "AppName", dBServer.AppId);
            return View(dBServer);
        }

        // GET: DBServers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DBServer dBServer = db.DBServer.Find(id);
            if (dBServer == null)
            {
                return HttpNotFound();
            }
            return View(dBServer);
        }

        // POST: DBServers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DBServer dBServer = db.DBServer.Find(id);
            db.DBServer.Remove(dBServer);
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

        public ActionResult ConnectSQL(string ServerName, string Username)
        {
            Process rdcProcess = new Process();

            string executable = System.Environment.ExpandEnvironmentVariables(@"C:\Program Files (x86)\Microsoft SQL Server\120\Tools\Binn\ManagementStudio\Ssms.exe");
            if (executable != null)
            {

                rdcProcess.StartInfo.FileName = executable;
                rdcProcess.StartInfo.Arguments = " -S " + ServerName+ " -U "+ Username;   
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
    }
}
