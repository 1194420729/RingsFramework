﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rings.Models;

namespace Rings.Controllers
{
    [Authorize]
    public class UploadController : Controller
    {
        //todo:upload url
        private static string domain = "http://localhost/files";

        //todo:存储空间根目录
        private static string storageroot = "~/Files";

        [HttpPost]
        public ActionResult UploadPic(HttpPostedFileBase file)
        {

            if (file.ContentLength == 0)
            {
                return Json(new { message = "请选择文件" });
            }

            string type = file.ContentType.ToLower();
            if (type.StartsWith("image/") == false)
            {
                return Json(new { message = "图片格式不正确" });
            }

            int size = file.ContentLength / 1024;
            if (size > 512)
            {
                //return Json(new { message = "文件大小超出限制，请不要超过500K" });

            }

            string date = DateTime.Now.ToString("yyyyMMdd");
            string applicationid = User.Identity.GetAccount().ApplicationId;
            string path = Path.Combine(Server.MapPath(storageroot), applicationid, date);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            string newfilename = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);

            string fileName = Path.Combine(path, newfilename);

            try
            {
                file.SaveAs(fileName);

                return Json(new { message = "ok", pic = domain + "/" + applicationid + "/" + date + "/" + newfilename });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { message = "上传出错" });
            }

        }

        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase file)
        {

            if (file.ContentLength == 0)
            {
                return Json(new { message = "请选择文件" });
            }

            int size = file.ContentLength / 1024;
            if (size > 10240)
            {
                return Json(new { message = "文件大小超出限制，请不要超过10M" });
            }

            string date = DateTime.Now.ToString("yyyyMMdd");
            string applicationid = User.Identity.GetAccount().ApplicationId;
            string path = Path.Combine(Server.MapPath(storageroot), applicationid, date);
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            string newfilename = Guid.NewGuid().ToString("N") + Path.GetExtension(file.FileName);
            string fileName = Path.Combine(path, newfilename);

            try
            {
                file.SaveAs(fileName);

                return Json(new { message = "ok", url = domain + "/" + applicationid + "/" + date + "/" + newfilename, path = storageroot+"/" + applicationid + "/" + date + "/" + newfilename });
            }
            catch (Exception ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                return Json(new { message = "上传出错" });
            }

        }


    }
}
