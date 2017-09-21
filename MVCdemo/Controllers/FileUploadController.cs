using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
namespace MVCdemo.Controllers
{
    public class FileUploadController : Controller
    {
        string accessKey = "AKIAIBOWU4YKYQFQVOIA";
        string secretKey = "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";
        AmazonS3Config config = new AmazonS3Config();
      
        // GET: FileUpload
        public ActionResult DisplayFile(string filename)
        {
            config.ServiceURL = "https://s3.us-east-2.amazonaws.com/";

            AmazonS3Client client = new AmazonS3Client(accessKey, secretKey, config);


            if (filename==null || filename=="")
            {

             //   return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
     
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
            request.BucketName = "elasticbeanstalk-us-east-2-613730444253";
            request.Key = filename;
            request.Expires = DateTime.Now.AddHours(1);
            request.Protocol = Protocol.HTTP;
            string url = client.GetPreSignedURL(request);
            ViewBag.URLRedirect = url;
            return RedirectPermanent(url);

            //var dir = Server.MapPath("/UploadedFiles");
            //var path = Path.Combine(dir, filename); //validate the path for security or use other means to generate the path.

            //return base.File(path, "image/jpeg");
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
                config.ServiceURL = "https://s3.us-east-2.amazonaws.com/";

                AmazonS3Client client = new AmazonS3Client(accessKey, secretKey, config);

                if (file.ContentLength > 0)
                {
                    string _FileName = Path.GetFileName(file.FileName);
                    string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
                    file.SaveAs(_path);

                    PutObjectRequest putrequest = new PutObjectRequest();
                    putrequest.BucketName = "elasticbeanstalk-us-east-2-613730444253";
                    putrequest.Key = file.FileName;
                    putrequest.ContentType = "image/jpeg";
                    putrequest.InputStream = file.InputStream;
                   
                    client.PutObject(putrequest);

                    GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
                    request.BucketName = "elasticbeanstalk-us-east-2-613730444253";
                    request.Key = file.FileName;
                    request.Expires = DateTime.Now.AddHours(1);
                    request.Protocol = Protocol.HTTP;
                    string url = client.GetPreSignedURL(request);
                    filepath = url;
                }

                ViewBag.Message = "File Uploaded Successfully!!";
                Session["filepath"] = file.FileName;
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