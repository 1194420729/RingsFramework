﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rings.Controllers
{
    [Authorize]
    public class ContextServiceController : Controller
    { 
        public ActionResult MapPath(string path)
        { 
            return Content(Server.MapPath(path));
        }

    }
}