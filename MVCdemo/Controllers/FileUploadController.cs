using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCdemo.Controllers
{
    public class FileUploadController : Controller
    {
        // GET: FileUpload
        public ActionResult DisplayFile(string filename)
        {
            if(filename==null || filename=="")
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var dir = Server.MapPath("/UploadedFiles");
            var path = Path.Combine(dir, filename); //validate the path for security or use other means to generate the path.
       
            return base.File(path, "image/jpeg");
        }
        [HttpGet]
        public ActionResult UploadFile(string reqType,string editid  )
        {
            Session["reqType"] = reqType;
            if (editid != null)
                Session["editid"] = editid;
            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            string filepath = "";
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);
                    filepath = _FileName;
                }

                ViewBag.Message = "File Uploaded Successfully!!";
                Session["filepath"]= filepath;
                if (Session["reqType"].ToString() == "Create")
                    return RedirectToAction(Session["reqType"].ToString(), "Environments", new {  Redirected = true });
                else
                return RedirectToAction(Session["reqType"].ToString(), "Environments", new { id = Convert.ToUInt32(Session["editid"]) ,Redirected = true });
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View();
            }
        }


    }
}