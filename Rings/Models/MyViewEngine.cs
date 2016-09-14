using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rings.Models
{
    public class MyViewEngine : RazorViewEngine
    {
        public MyViewEngine()
        {

        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var account = HttpContext.Current.User.Identity.GetAccount();

                PartialViewLocationFormats = new[]
                {
                    "~/Views/"+account.ApplicationId+"/"+account.Language+"/{0}.cshtml",
                    "~/Views/"+account.ApplicationId+"/{0}.cshtml", 
                    "~/Views/Default/"+account.Language+"/{0}.cshtml" ,
                    "~/Views/Default/{0}.cshtml" ,
                    "~/Views/{1}/"+account.Language+"/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml"
                };
            }
            else
            {
                PartialViewLocationFormats = new[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/{1}/{0}.vbhtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Shared/{0}.vbhtml"
                };
            }

            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var account = HttpContext.Current.User.Identity.GetAccount();
                 
                ViewLocationFormats = new[]
                {
                    "~/Views/"+account.ApplicationId+"/"+account.Language+"/{0}.cshtml",
                    "~/Views/"+account.ApplicationId+"/{0}.cshtml", 
                    "~/Views/Default/"+account.Language+"/{0}.cshtml" ,
                    "~/Views/Default/{0}.cshtml" ,
                    "~/Views/{1}/"+account.Language+"/{0}.cshtml",
                    "~/Views/{1}/{0}.cshtml"
                };
            }
            else
            {
                ViewLocationFormats = new[]
                {
                    "~/Views/{1}/{0}.cshtml",
                    "~/Views/{1}/{0}.vbhtml",
                    "~/Views/Shared/{0}.cshtml",
                    "~/Views/Shared/{0}.vbhtml"
                };
            }

            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

    }
}